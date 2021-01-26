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
}

/// <summary>
/// ステージ内に配置可能なオブジェクト
/// </summary>
public abstract class StageObject : MonoBehaviour
{
    public abstract PlacementType GetPlacementType();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
