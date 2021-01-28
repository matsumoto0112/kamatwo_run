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
    private PlayerParameterData statusData;

    public PlayerParameterData GetStatus() => statusData;
}

[System.Serializable]
public class PlayerParameterData
{
    /// <summary>
    /// ジャンプ時のスピードに乗算する係数
    /// </summary>
    public float coefJumpSpeed = 1.0f;

    /// <summary>
    /// 滞空時間
    /// </summary>
    public float flightTime = 1.0f;

    public PlayerParameterData(PlayerParameterData other)
    {
        this.coefJumpSpeed = other.coefJumpSpeed;
    }
}
