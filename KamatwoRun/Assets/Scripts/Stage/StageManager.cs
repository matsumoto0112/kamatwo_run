using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// サブステージの形状
/// </summary>
public enum SubStageShapeType
{
    //直線
    Straight,
    //L字
    L_Shape,
    //ゴール
    Goal,
}

/// <summary>
/// サブステージ情報
/// </summary>
[System.Serializable]
public struct SubStagePrefabInfo
{
    public SubStageShapeType type;
    public GameObject prefab;
}

/// <summary>
/// ステージ管理
/// </summary>
public class StageManager : MonoBehaviour
{
    [SerializeField]
    private StageParameter stageParameter;
    [SerializeField]
    private List<SubStagePrefabInfo> subStagePrefabs;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private EventManager eventManager = null;

    //プレイヤーのステータス管理オブジェクト
    private PlayerStatus playerStatus;
    private int currentWavePhase;
    private int spawnedSubStageNum;
    private bool goalGenerated;

    //次のステージの形状予約(nullの時は指定なし)
    private SubStageShapeType? reserveNextStageType;

    //ゲーム速度管理
    private GameSpeed gameSpeed;

    //生成されたサブステージ群
    private List<SubStage> subStages;

    //サブステージの正方形の一辺の長さ
    private static readonly float SubStageUnit = 50.0f;

    private Vector3 prevScrolledDirection;

    /// <summary>
    /// ステージを削除できる状態か
    /// </summary>
    public bool stageDeletable { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameDataStore.Instance.ResetWaveCount();
        gameSpeed = GetComponent<GameSpeed>();
        subStages = new List<SubStage>();
        playerStatus = player.GetComponent<PlayerStatus>();
        currentWavePhase = 0;
        spawnedSubStageNum = 0;
        goalGenerated = false;
        stageDeletable = true;

        //最初のステージ情報は固定のものを使用する
        {
            //ちょっとだけ手前にずらす
            Vector3 pos = new Vector3(0, 0, SubStageUnit / 2.0f - 5.0f);
            GameObject newObject = Instantiate(subStagePrefabs[0].prefab, pos, Quaternion.identity);
            subStages.Add(newObject.GetComponent<SubStage>());
        }

        //事前作成ステージ数
        const int preSpawnSubStageNum = 5;
        for (int i = 0; i < preSpawnSubStageNum; i++)
        {
            SpawnNextSubStage();
        }

        reserveNextStageType = null;
        NormalizeGameSpeed();
        Assert.IsNotNull(playerStatus, "PlayerStatusが取得できませんでした");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(player.transform.position);
        }
        //スタートイベント中だったら
        if (eventManager.StartEventFlag == false)
        {
            return;
        }

        Vector3 scrollDirection = GetForegroundDirection(player.transform.position);
        if (prevScrolledDirection != scrollDirection)
        {
            int index = -1;
            for (int i = 0; i < subStages.Count; i++)
            {
                if (subStages[i].IsInArea(player.transform.position))
                {
                    index = i;
                }
            }
            if (index == -1)
            {
                Debug.LogError("Playerのいるサブステージが取得できませんでした。");
            }

            subStages[index].transform.position = Vector3.zero;
            for (int i = index + 1; i < subStages.Count; i++)
            {
                subStages[i].transform.position = subStages[i - 1].transform.position + SubStageOffset(subStages[i - 1].ExitType);
            }
            for (int i = index - 1; i >= 0; i--)
            {
                subStages[i].transform.position = subStages[i + 1].transform.position + SubStageOffset(subStages[i + 1].EntranceType);
            }
        }

        prevScrolledDirection = scrollDirection;
        scrollDirection = scrollDirection.normalized;
        float speed = gameSpeed.Speed * Time.deltaTime;
        foreach (var st in subStages)
        {
            st.Move(scrollDirection, speed);
        }

        CalcToNeedNextStageReserve();

        SubStage frontStage = subStages.First();
        if (!frontStage.IsInsideCamera && stageDeletable)
        {
            //プレイヤーより前にいったので範囲外になったため、削除する
            Destroy(frontStage.gameObject);
            subStages.RemoveAt(0);

            //新しく追加する
            SpawnNextSubStage();

            //ゴールの生成する条件を満たしているかどうかの判定式
            System.Func<bool> IsFlaggedGenerateGoal = () =>
            {
                //すでに生成済みなら再度生成はしない
                if (goalGenerated) return false;
                //プレイモードがエンドレスならゴールは生成されない
                if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday) return false;
                //生成済みステージ数がゴール生成に必要なウェーブ数と一致しているときに、次のゴールを生成する
                return spawnedSubStageNum == stageParameter.stageGoalWaveNum;
            };

            if (IsFlaggedGenerateGoal())
            {
                ReserveNextSubstageShapeType(SubStageShapeType.Goal);
            }
        }
    }

    /// <summary>
    /// 次のステージの形状予約が必要かどうかを計算し、必要であれば予約する
    /// </summary>
    private void CalcToNeedNextStageReserve()
    {
        //ウェーブ段階に変化があれば形状が変わる
        if (currentWavePhase != stageParameter.GetPhaseByScore(playerStatus.Score))
        {
            currentWavePhase = stageParameter.GetPhaseByScore(playerStatus.Score);
            ReserveNextSubstageShapeType(SubStageShapeType.L_Shape);
        }
    }

    /// <summary>
    /// 次のステージプレハブを取得する
    /// </summary>
    /// <param name="type">次のステージの入口の種類</param>
    /// <returns>入口の種類が一致するステージのプレハブを返す</returns>
    private GameObject GetNextSubStagePrefab(GatewayType type)
    {
        List<SubStagePrefabInfo> sameTypePrefabs;
        //次ステージの種類の予約があればそれを採用する
        if (reserveNextStageType.HasValue)
        {
            sameTypePrefabs = subStagePrefabs.FindAll(obj => obj.type == reserveNextStageType.Value && obj.prefab.GetComponent<SubStage>().EntranceType == type);
        }
        else
        {
            sameTypePrefabs = subStagePrefabs.FindAll(obj => obj.type == SubStageShapeType.Straight && obj.prefab.GetComponent<SubStage>().EntranceType == type);
        }

        //その中から適当なプレハブを選ぶ
        int prefabIndex = Random.Range(0, sameTypePrefabs.Count());
        GameObject prefab = sameTypePrefabs.ElementAt(prefabIndex).prefab;
        SubStage prefabSubStage = prefab.GetComponent<SubStage>();
        return prefab;
    }

    /// <summary>
    /// サブステージを配置するオフセット値を取得する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Vector3 SubStageOffset(GatewayType type)
    {
        switch (type)
        {
            case GatewayType.North:
                return new Vector3(0, 0, SubStageUnit);
            case GatewayType.South:
                return new Vector3(0, 0, -SubStageUnit);
            case GatewayType.East:
                return new Vector3(SubStageUnit, 0, 0);
            default:
                return new Vector3(-SubStageUnit, 0, 0);
        }
    }

    /// <summary>
    /// 次のサブステージを生成する
    /// </summary>
    private void SpawnNextSubStage()
    {
        //直前のステージに合致するステージを生成する
        SubStage prevStage = subStages.Last();
        GameObject next = GetNextSubStagePrefab(GatewayTypeExtend.ChainableType(prevStage.ExitType));
        Vector3 spawnPosition = prevStage.transform.position + SubStageOffset(prevStage.ExitType);
        GameObject newObject = Instantiate(next, spawnPosition, Quaternion.identity);
        SubStage newSubStage = newObject.GetComponent<SubStage>();
        subStages.Add(newSubStage);

        if (reserveNextStageType.HasValue)
        {
            if (reserveNextStageType.Value == SubStageShapeType.Goal)
            {
                goalGenerated = true;
            }
            reserveNextStageType = null;
        }
        else
        {
            spawnedSubStageNum++;
        }

        if (!goalGenerated)
        {
            newSubStage.SpawnObjects(stageParameter.GetSpawnObjectsParameterByPhase(currentWavePhase), 5);
        }
    }

    /// <summary>
    /// 前方方向（ステージの移動方向）を取得する
    /// </summary>
    /// <param name="checkPosition"></param>
    /// <returns></returns>
    public Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        foreach (var st in subStages)
        {
            if (st.IsInArea(checkPosition))
            {
                return st.GetForegroundDirection(checkPosition);
            }
        }

        Debug.Log($"調べたい座標が生成済みのステージ内にありません。");
        return subStages[0].GetForegroundDirection(checkPosition);
    }

    /// <summary>
    /// 次のサブステージの形状を予約する
    /// </summary>
    /// <param name="type"></param>
    public void ReserveNextSubstageShapeType(SubStageShapeType type)
    {
        //ゴール予約は上書きしない
        if (reserveNextStageType.HasValue && reserveNextStageType.Value == SubStageShapeType.Goal) return;
        //ゴールが生成済みなら他の形状にしたくない
        if (goalGenerated) return;

        reserveNextStageType = type;
    }

    /// <summary>
    /// ゲームスピードを正常化する
    /// </summary>
    public void NormalizeGameSpeed()
    {
        gameSpeed.UpdateGameSpeed(stageParameter.GetGameSpeed(currentWavePhase));
    }
}
