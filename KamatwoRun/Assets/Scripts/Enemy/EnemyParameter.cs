using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵パラメータ
/// </summary>
[CreateAssetMenu(menuName = "Parameters/Enemy")]
public class EnemyParameter : ScriptableObject
{
    [Tooltip("配置タイプ")]
    public PlacementType type;
    [Tooltip("倒した時のスコア")]
    public int score;
    [Tooltip("触れた時のダメージ量")]
    public float damage;
    [Tooltip("倒した時の回復量")]
    public float recover;
}
