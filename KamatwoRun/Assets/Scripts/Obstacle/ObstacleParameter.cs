using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 障害物のパラメータ
/// </summary>
[CreateAssetMenu(menuName = "Parameters/Obstacle")]
public class ObstacleParameter : ScriptableObject
{
    [Tooltip("配置タイプ")]
    public PlacementType type;
    [Tooltip("ダメージ量")]
    public float damage;
}
