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
}

/// <summary>
/// �X�e�[�W���ɔz�u�\�ȃI�u�W�F�N�g
/// </summary>
public class StageObject : MonoBehaviour
{
    [SerializeField]
    private PlacementType placementType;

    public PlacementType PlacementType { get { return placementType; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
