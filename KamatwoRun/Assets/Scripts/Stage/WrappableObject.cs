using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��܂ꂽ���̓��_������Ԃ�
/// </summary>
public struct WrappedPoint
{
    //�l���X�R�A
    int score;
    //�񕜗�
    float recover;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="score"></param>
    public WrappedPoint(int score, float recover)
    {
        this.score = score;
        this.recover = recover;
    }
}

/// <summary>
/// ��ɕ�܂�邱�Ƃ��\�ȃI�u�W�F�N�g
/// </summary>
public abstract class WrappableObject : StageObject
{
    /// <summary>
    /// ��܂��Ƃ��̏���
    /// </summary>
    /// <returns>��܂ꂽ��̏���Ԃ�</returns>
    public virtual WrappedPoint Wrap()
    {
        return new WrappedPoint(0, 0.0f);
    }
}
