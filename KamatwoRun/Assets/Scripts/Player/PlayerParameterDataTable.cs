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
    private PlayerParameterData parameterData;

    public PlayerParameterData GetParameter() => parameterData;
}

[System.Serializable]
public class PlayerParameterData
{
    /// <summary>
    /// �W�����v���̃X�s�[�h�ɏ�Z����W��
    /// </summary>
    [Header("�W�����v���̃X�s�[�h�ɏ�Z����W��")]
    public float coefJumpSpeed = 1.0f;

    /// <summary>
    /// �ړ��ɂ����鎞��
    /// </summary>
    [Header("���E�ړ��ɂ����鎞��")]
    public float timeToMove = 0.5f;

    /// <summary>
    /// �v���C���[�̗̑�
    /// </summary>
    [Range(1, 10), Header("�v���C���[�̗̑�")]
    public int hp = 3;

    /// <summary>
    /// ���G����
    /// </summary>
    [Range(0.1f, 5.0f), Header("���G����")]
    public float invincibleTime = 2.0f;

    /// <summary>
    /// �؋󎞊�
    /// </summary>
    [Header("�W�����v���̑؋󎞊�")]
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
