using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�̔z�u�^�C�v
/// </summary>
public enum PlacementType
{
    //�n�ʂɐڒn����K�v����
    OnlyGround,
    //�󒆂̂�
    OnlySky,
    //�n�ʁA�󒆂ǂ��������
    GroundOrSky,
    //���L
    Wide,
    //����������
    High,
}

/// <summary>
/// �X�e�[�W���ɔz�u�\�ȃI�u�W�F�N�g
/// </summary>
public abstract class StageObject : MonoBehaviour
{
    public abstract PlacementType GetPlacementType();
}
