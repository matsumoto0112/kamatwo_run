using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1レーンを管理するクラス
/// </summary>
public abstract class Lane : MonoBehaviour
{
    /// <summary>
    /// スポーンポイントが存在すればその間のランダムな地点を返す
    /// </summary>
    /// <returns></returns>
    public abstract Vector3? GetRandomSpawnPoint();
}
