using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    /// <summary>
    /// ����������
    /// </summary>
    void Initialize();

    /// <summary>
    /// ���s
    /// </summary>
    void Execution();

    /// <summary>
    /// �I�����m
    /// </summary>
    /// <returns></returns>
    bool IsEnd();

    /// <summary>
    /// �C�x���g���ɃR�}���h���s���̏ꍇ�ɏ��������s��
    /// </summary>
    void EventInitialize();
}
