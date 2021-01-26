using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 得点情報
/// </summary>
[CreateAssetMenu(menuName = "Parameters/ScoreObject")]
public class ScoreObjectParameter : ScriptableObject
{
    [Tooltip("配置タイプ")]
    public PlacementType type;
    [Tooltip("獲得スコア")]
    public int score;
}