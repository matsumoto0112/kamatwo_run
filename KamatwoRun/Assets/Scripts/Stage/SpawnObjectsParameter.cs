using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct SpawnObject
{
    [Tooltip("スポーン対象")]
    public GameObject prefab;
    [Tooltip("ランダム生成時にスポーン対象が選択される割合"), Range(0.0f, 1.0f)]
    public float selectRate;
}

/// <summary>
/// オブジェクトのスポーンに関するパラメータ
/// </summary>
[CreateAssetMenu(menuName = "Parameters/SpawnObjects")]
public class SpawnObjectsParameter : ScriptableObject
{
    [Tooltip("スポーン対象リスト"), Header("selectRateの合計値は1.0になるようにしてください")]
    public List<SpawnObject> spawnObjects;

    /// <summary>
    /// ランダムなオブジェクトを取得する
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomObject()
    {
        float r = Random.Range(0.0f, 1.0f);
        foreach (var o in spawnObjects)
        {
            if (r <= o.selectRate)
            {
                return o.prefab;
            }
            r -= o.selectRate;
        }

        return spawnObjects.Last().prefab;
    }
}
