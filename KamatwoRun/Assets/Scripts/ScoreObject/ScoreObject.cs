using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 得点オブジェクト
/// </summary>
public class ScoreObject : StageObject
{
    [SerializeField, Tooltip("スコア情報")]
    private ScoreObjectParameter scoreInfo;

    public override PlacementType GetPlacementType()
    {
        return scoreInfo.type;
    }

    public ScoreObjectParameter ScoreInfo { get { return scoreInfo; } }
}
