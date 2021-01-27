using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_ShapedLane : Lane
{
    [SerializeField, Tooltip("�X�|�[���\�Ȓn�_�̊J�n�n�_")]
    private GameObject lineStartPoint;
    [SerializeField, Tooltip("�X�|�[���\�Ȓn�_�̒��p�n�_")]
    private GameObject lineMiddlePoint;
    [SerializeField, Tooltip("�X�|�[���\�Ȓn�_�̏I���n�_")]
    private GameObject lineEndPoint;

    [SerializeField, Tooltip("�����n�_�����Ԋu�ɂ��邽�߂̃X�e�b�v�l")]
    private int step = 5;

    /// <summary>
    /// �����_���ȃX�|�[���n�_��Ԃ�
    /// </summary>
    /// <returns></returns>
    public override Vector3 GetRandomSpawnPoint()
    {
        if (!(lineStartPoint && lineEndPoint)) { return Vector3.zero; }
        float t = (Random.Range(0, step + 1) * 1.0f / step) * 2.0f;
        if (t <= 1.0f)
        {
            return Vector3.Lerp(lineStartPoint.transform.position, lineMiddlePoint.transform.position, t);
        }
        else
        {
            return Vector3.Lerp(lineMiddlePoint.transform.position, lineEndPoint.transform.position, t - 1.0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!(lineStartPoint && lineEndPoint && lineMiddlePoint)) { return; }

        //�X�|�[���\�Ȓn�_�����������̕\��
        {
            Vector3 from = lineStartPoint.transform.position;
            Vector3 to = lineMiddlePoint.transform.position;
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawLine(from, to);
        }
        {
            Vector3 from = lineMiddlePoint.transform.position;
            Vector3 to = lineEndPoint.transform.position;
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawLine(from, to);
        }
    }
}
