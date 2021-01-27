using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLane : Lane
{
    [SerializeField, Tooltip("�X�|�[���\�Ȓn�_�̊J�n�n�_")]
    private GameObject lineStartPoint;
    [SerializeField, Tooltip("�X�|�[���\�Ȓn�_�̏I���n�_")]
    private GameObject lineEndPoint;

    [SerializeField, Tooltip("�����n�_�����Ԋu�ɂ��邽�߂̃X�e�b�v�l")]
    private int step = 5;

    /// <summary>
    /// �����_���ȃX�|�[���n�_��Ԃ�
    /// </summary>
    /// <returns></returns>
    public override Vector3? GetRandomSpawnPoint()
    {
        if (!(lineStartPoint && lineEndPoint)) { return Vector3.zero; }
        float t = Random.Range(0, step + 1) * 1.0f / step;
        return Vector3.Lerp(lineStartPoint.transform.position, lineEndPoint.transform.position, t);
    }

    private void OnDrawGizmos()
    {
        if (!(lineStartPoint && lineEndPoint)) { return; }

        //�X�|�[���\�Ȓn�_�����������̕\��
        Vector3 from = lineStartPoint.transform.position;
        Vector3 to = lineEndPoint.transform.position;
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Gizmos.DrawLine(from, to);
    }
}
