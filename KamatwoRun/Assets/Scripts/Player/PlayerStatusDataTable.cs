using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�X�e�[�^�X�f�[�^�e�[�u��
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
    /// �W�����v���̃X�s�[�h�ɏ�Z����W��
    /// </summary>
    public float coefJumpSpeed = 1.0f;

    /// <summary>
    /// �؋󎞊�
    /// </summary>
    public float flightTime = 1.0f;

    public PlayerStatusData(PlayerStatusData other)
    {
        this.coefJumpSpeed = other.coefJumpSpeed;
    }
}
