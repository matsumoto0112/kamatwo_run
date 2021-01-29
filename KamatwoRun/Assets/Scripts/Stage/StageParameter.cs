using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �E�F�[�u�Ɋւ�����
/// </summary>
[System.Serializable]
public struct WaveInfo
{
    public int scoreThreshold;
    public SpawnObjectsParameter spawnParameter;
}

[CreateAssetMenu(menuName = "Parameters/Stage")]
public class StageParameter : ScriptableObject
{
    [Header("�E�F�[�u�֌W�̏��", order = 0)]
    [Header("���X�g���̗v�f�͏ォ�珇��", order = 1)]
    [Header("�X�R�A��臒l", order = 2)]
    [Header("臒l�ȉ��̎��ɍ̗p����X�|�[�����", order = 3)]
    public List<WaveInfo> thresholds;

    [Header("�E�F�[�u���ς��P��"), Tooltip("���臒l���X�g�����ׂď������I�������ɁA\n�ǂ̒P�ʂŃE�F�[�u���ς�邩")]
    public WaveInfo step;

    [Space()]
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

    /// <summary>
    /// �X�R�A�ɑ΂���E�F�[�u�i�K���擾����
    /// </summary>
    /// <param name="currentScore"></param>
    /// <returns></returns>
    public int GetPhaseByScore(int score)
    {
        int res = 0;
        //�e�i�K�𒲂ׁA�X�R�A������𒴂��Ă�����㏑�����Ă���
        for (int i = 0; i < thresholds.Count; i++)
        {
            if (score >= thresholds[i].scoreThreshold)
            {
                res = i + 1;
            }
        }

        //����܂ŒB���Ă����炻��ȍ~�͈��Ԋu�ŏオ�邽�߁A����𒲂ׂ�
        if (res == thresholds.Count)
        {
            score -= thresholds.Last().scoreThreshold;
            res += score / step.scoreThreshold;
        }

        return res;
    }

    /// <summary>
    /// ���݂̒i�K�ɉ������X�|�[�������擾����
    /// </summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    public SpawnObjectsParameter GetSpawnObjectsParameterByPhase(int phase)
    {
        //臒l���X�g���̎�
        if (phase < thresholds.Count)
        {
            return thresholds[phase].spawnParameter;
        }
        else
        {
            return step.spawnParameter;
        }
    }
}
