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

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="score"></param>
    public WrappedPoint(int score)
    {
        this.score = score;
    }
}

/// <summary>
/// ��ɕ�܂�邱�Ƃ��\�ȃI�u�W�F�N�g
/// </summary>
public class WrappableObject : StageObject
{
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// ��܂��Ƃ��̏���
    /// </summary>
    /// <returns>��܂ꂽ��̏���Ԃ�</returns>
    public virtual WrappedPoint Wrap()
    {
        return new WrappedPoint(0);
    }
}
