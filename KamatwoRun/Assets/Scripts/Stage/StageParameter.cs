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
}
