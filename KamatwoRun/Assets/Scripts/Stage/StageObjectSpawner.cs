using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct SpawnObjectInfo
{
    public SpawnObjectType type;
    public GameObject prefab;
}

public class StageObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<SpawnObjectInfo> prefabs;
    [SerializeField]
    private StageParameter stageParameter;

    private List<GameObject> spawnedObjects;

    private void Start()
    {
        spawnedObjects = new List<GameObject>();
    }

    /// <summary>
    /// スポーン判定に成功したらスポーンさせる処理
    /// </summary>
    /// <param name="type"></param>
    public GameObject SpawnIfSucceed(SpawnObjectType type, Vector3 basePoint, int laneNum)
    {
        GameObject prefab = GetPrefabFromType(type);
        PlacementType placementType = prefab.GetComponent<StageObject>().GetPlacementType();

        //幅が広いオブジェクトの場合、レーンが1番（真ん中）出ないと生成できない
        if (placementType == PlacementType.Wide && laneNum != 1) { return null; }

        Vector3 offset = new Vector3(0, GetYOffset(placementType), 0);
        Vector3 spawnPosition = basePoint + offset;

        //スポーンテストをする
        //まず、無効な値をすべて削除する
        spawnedObjects.RemoveAll(obj => obj == null);

        bool succeeded = true;
        //同じ座標に生成するのは無効にする
        foreach (var obj in spawnedObjects)
        {
            PlacementType obj_pracementType = obj.GetComponent<StageObject>().GetPlacementType();
            if (Vector3.Distance(obj.transform.position, spawnPosition) < Mathf.Epsilon) succeeded = false;
            //幅の広いオブジェクトの場合、距離で判定する
            if (obj_pracementType == PlacementType.Wide)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < stageParameter.wideObjectJudgeRadius) { succeeded = false; }
            }
            //高さのあるオブジェクトの場合、xz座標で判定する
            if (obj_pracementType == PlacementType.High)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < Mathf.Epsilon) { succeeded = false; }
            }
        }

        //幅の広いオブジェクトを配置するときは、左右のレーンも調べる
        if (placementType == PlacementType.Wide)
        {
            foreach (var obj in spawnedObjects)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < stageParameter.wideObjectJudgeRadius) { succeeded = false; }
                //幅の広いオブジェクト同士はもっと距離を開ける
                if (obj.GetComponent<StageObject>().GetPlacementType() == PlacementType.Wide && Vector3.Distance(a, b) < stageParameter.wideObjectJudgeRadius * 3.0f) { succeeded = false; }
            }
        }

        //高さのあるオブジェクトを配置するときは、xz座標で判定する
        if (placementType == PlacementType.High)
        {
            foreach (var obj in spawnedObjects)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < Mathf.Epsilon) { succeeded = false; }
                //高さのあるオブジェクト同士は隣接しないようにする
                if (obj.GetComponent<StageObject>().GetPlacementType() == PlacementType.High && Vector3.Distance(a, b) < stageParameter.highObjectJudgeDistance) { succeeded = false; }
            }
        }

        if (!succeeded) return null;

        GameObject res = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(res);
        return res;
    }

    /// <summary>
    /// スポーンオブジェクトの種類からプレハブを取得する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetPrefabFromType(SpawnObjectType type)
    {
        foreach (var p in prefabs)
        {
            if (p.type == type) return p.prefab;
        }

        Assert.IsTrue(false, "存在しないtypeが選択されました");
        return null;
    }

    /// <summary>
    /// 配置タイプから配置するときのY座標のオフセットを取得する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private float GetYOffset(PlacementType type)
    {
        switch (type)
        {
            case PlacementType.OnlyGround:
            case PlacementType.Wide:
                return stageParameter.groundPosition_Y;
            case PlacementType.OnlySky:
                return stageParameter.skyPosition_Y;
            case PlacementType.GroundOrSky:
                {
                    if (Random.Range(0.0f, 1.0f) < 0.5f)
                    {
                        return stageParameter.groundPosition_Y;
                    }
                    else
                    {
                        return stageParameter.skyPosition_Y;
                    }
                }
            case PlacementType.High:
                return stageParameter.groundPosition_Y + 1.0f;
            default:
                return 0.0f;
        }
    }
}
