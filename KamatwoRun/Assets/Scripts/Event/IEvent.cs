using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    /// <summary>
    /// �C�x���g�J�n������������
    /// </summary>
    void OnInitialize();

    /// <summary>
    /// �C�x���g�X�V����
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// �C�x���g�I������
    /// </summary>
    bool OnEnd();
}
