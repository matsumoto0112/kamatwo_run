using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーステータスデータテーブル
/// </summary>
[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "ScriptableObject/PlayerStatusData", order = 0)]
public class PlayerStatusDataTable : ScriptableObject
{
    [SerializeField]
    private PlayerStatusData statusData;

    public PlayerStatusData GetStatus() => statusData;
}

[System.Serializable]
public class PlayerStatusData
{
    /// <summary>
    /// ジャンプ時のスピードに乗算する係数
    /// </summary>
    public float coefJumpSpeed = 1.0f;

    /// <summary>
    /// 滞空時間
    /// </summary>
    public float flightTime = 1.0f;

    public PlayerStatusData(PlayerStatusData other)
    {
        this.coefJumpSpeed = other.coefJumpSpeed;
    }
}
