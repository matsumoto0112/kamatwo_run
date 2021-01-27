using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Stage")]
public class StageParameter : ScriptableObject
{
    //�Q�[�����̃��[����
    public int laneNum = 3;

    //�X�e�[�W��
    public float stageWidth = 6.0f;

    //�f�t�H���g�ł̈ړ����x
    public float defaultStageMoveSpeed = 15.0f;

    //�n�ʂ̍���
    public float groundPosition_Y = 1.0f;

    //�󒆂̍���
    public float skyPosition_Y = 10.0f;

    //���ɒ����I�u�W�F�N�g�̔��a
    public float wideObjectJudgeRadius = 3.0f;

    //�����̂���I�u�W�F�N�g���m���אڂ��Ȃ��悤�ɂ��锻�苗��
    public float highObjectJudgeDistance = 3.0f;

}
