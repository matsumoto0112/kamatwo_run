using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーステータスデータテーブル
/// </summary>
[CreateAssetMenu(fileName = "PlayerParameterDataTable", menuName = "ScriptableObject/PlayerParameterDataTable", order = 0)]
public class PlayerParameterDataTable : ScriptableObject
{
    [SerializeField]
    private PlayerParameterData parameterData;

    public PlayerParameterData GetParameter() => parameterData;
}

[System.Serializable]
public class PlayerParameterData
{
    /// <summary>
    /// ジャンプ時のスピードに乗算する係数
    /// </summary>
    [Header("ジャンプ時のスピードに乗算する係数")]
    public float coefJumpSpeed = 1.0f;

    /// <summary>
    /// 移動にかかる時間
    /// </summary>
    [Header("左右移動にかかる時間")]
    public float timeToMove = 0.5f;

    /// <summary>
    /// プレイヤーの体力
    /// </summary>
    [Range(1, 10), Header("プレイヤーの体力")]
    public int hp = 3;

    /// <summary>
    /// 無敵時間
    /// </summary>
    [Range(0.1f, 5.0f), Header("無敵時間")]
    public float invincibleTime = 2.0f;

    /// <summary>
    /// 滞空時間
    /// </summary>
    [Header("ジャンプ時の滞空時間")]
    public float flightTime = 1.0f;

    public PlayerParameterData(PlayerParameterData other)
    {
        this.coefJumpSpeed = other.coefJumpSpeed;
        this.timeToMove = other.timeToMove;
        this.hp = other.hp;
        this.invincibleTime = other.invincibleTime;
        this.flightTime = other.flightTime;
    }
}
