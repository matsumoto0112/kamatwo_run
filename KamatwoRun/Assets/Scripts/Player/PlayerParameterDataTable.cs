using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�X�e�[�^�X�f�[�^�e�[�u��
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
    /// �W�����v���̃X�s�[�h�ɏ�Z����W��
    /// </summary>
    public float coefJumpSpeed = 1.0f;

    /// <summary>
    /// �؋󎞊�
    /// </summary>
    public float flightTime = 1.0f;

    public PlayerParameterData(PlayerParameterData other)
    {
        this.coefJumpSpeed = other.coefJumpSpeed;
    }
}
