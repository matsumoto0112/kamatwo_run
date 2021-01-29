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

    //プレイヤーのステータス管理オブジェクト
    private PlayerStatus playerStatus;
    private int currentWavePhase;

    //次のステージの形状予約(nullの時は指定なし)
    private SubStageShapeType? reserveNextStageType;

    //ゲーム速度管理
    private GameSpeed gameSpeed;

    //生成されたサブステージ群
    private List<SubStage> subStages;

    //サブステージの正方形の一辺の長さ
    private static readonly float SubStageUnit = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = GetComponent<GameSpeed>();
        subStages = new List<SubStage>();
        playerStatus = player.GetComponent<PlayerStatus>();
        currentWavePhase = 0;

        //最初のステージ情報は固定のものを使用する
        {
            //ちょっとだけ手前にずらす
            Vector3 pos = new Vector3(0, 0, SubStageUnit / 2.0f - 5.0f);
            GameObject newObject = Instantiate(subStagePrefabs[0].prefab, pos, Quaternion.identity);
            subStages.Add(newObject.GetComponent<SubStage>());
        }

        //事前作成ステージ数
        const int preSpawnSubStageNum = 3;
        for (int i = 0; i < preSpawnSubStageNum; i++)
        {
            SpawnNextSubStage();
        }

        reserveNextStageType = null;

        Assert.IsNotNull(playerStatus, "PlayerStatusが取得できませんでした");
    }

    void Update()
    {
        Vector3 scrollDirection = GetForegroundDirection(player.transform.position);
        float speed = gameSpeed.Speed * Time.deltaTime;
        foreach (var st in subStages)
        {
            st.Move(scrollDirection, speed);
        }

        CalcToNeedNextStageReserve();

        SubStage frontStage = subStages.First();
        if (!frontStage.IsInsideCamera)
        {
            //プレイヤーより前にいったので範囲外になったため、削除する
            Destroy(frontStage.gameObject);
            subStages.RemoveAt(0);

            //新しく追加する
            SpawnNextSubStage();
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
            reserveNextStageType = null;
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
        newSubStage.SpawnObjects(stageParameter.GetSpawnObjectsParameterByPhase(currentWavePhase), 5);
        subStages.Add(newSubStage);
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

        Debug.Log("調べたい座標が生成済みのステージ内にありません");
        return Vector3.back;
    }

    /// <summary>
    /// 次のサブステージの形状を予約する
    /// </summary>
    /// <param name="type"></param>
    public void ReserveNextSubstageShapeType(SubStageShapeType type)
    {
        reserveNextStageType = type;
    }
}
