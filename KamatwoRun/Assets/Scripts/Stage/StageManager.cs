using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> subStagePrefabs;

    [SerializeField]
    private GameObject player;

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

        //最初のステージ情報は固定のものを使用する
        {
            //ちょっとだけ手前にずらす
            Vector3 pos = new Vector3(0, 0, SubStageUnit / 2.0f - 5.0f);
            GameObject newObject = Instantiate(subStagePrefabs[0], pos, Quaternion.identity);
            subStages.Add(newObject.GetComponent<SubStage>());
        }

        //事前作成ステージ数
        const int preSpawnSubStageNum = 5;
        for (int i = 0; i < preSpawnSubStageNum; i++)
        {
            SpawnNextSubStage();
        }
    }

    void Update()
    {
        Vector3 scrollDirection = GetForegroundDirection(player.transform.position);
        float speed = gameSpeed.Speed * Time.deltaTime;
        foreach (var st in subStages)
        {
            st.Move(scrollDirection, speed);
        }

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
    /// 次のステージプレハブを取得する
    /// </summary>
    /// <param name="type">次のステージの入口の種類</param>
    /// <returns>入口の種類が一致するステージのプレハブを返す</returns>
    private GameObject GetNextSubStagePrefab(GatewayType type)
    {
        //100回だけテストする
        for (int i = 0; i < 100; i++)
        {
            int prefabIndex = Random.Range(0, subStagePrefabs.Count);
            GameObject prefab = subStagePrefabs[prefabIndex];
            SubStage prefabSubStage = prefab.GetComponent<SubStage>();
            if (prefabSubStage.EntranceType == type)
            {
                return prefab;
            }
        }

        Debug.Log("Failed to GetNextSubStagePrefab. Maybe mismatched GatewayType.");
        return null;
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
        //現状ではまっすぐのステージのみなので何も考えずに生成する
        SubStage prevStage = subStages.Last();
        GameObject next = GetNextSubStagePrefab(GatewayTypeExtend.ChainableType(prevStage.ExitType));
        Debug.Log(next.GetComponent<SubStage>().EntranceType + "" + next.GetComponent<SubStage>().ExitType);
        Vector3 spawnPosition = prevStage.transform.position + SubStageOffset(prevStage.ExitType);
        GameObject newObject = Instantiate(next, spawnPosition, Quaternion.identity);
        SubStage newSubStage = newObject.GetComponent<SubStage>();
        newSubStage.SpawnObjects(5);
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
}
