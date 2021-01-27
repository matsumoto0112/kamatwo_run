using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの配置タイプ
/// </summary>
public enum PlacementType
{
    //地面に接地する必要あり
    OnlyGround,
    //空中のみ
    OnlySky,
    //地面、空中どちらもあり
    GroundOrSky,
    //幅広
    Wide,
    //高さがある
    High,
}

/// <summary>
/// ステージ内に配置可能なオブジェクト
/// </summary>
public abstract class StageObject : MonoBehaviour
{
    public abstract PlacementType GetPlacementType();
}
