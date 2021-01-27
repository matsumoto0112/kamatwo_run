using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1レーンを管理するクラス
/// </summary>
public abstract class Lane : MonoBehaviour
{
    public abstract Vector3 GetRandomSpawnPoint();
}
