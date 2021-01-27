using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterComponent
{
    /// <summary>
    /// ����������
    /// </summary>
    void OnCreate();

    /// <summary>
    /// �X�V����
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// �I������
    /// </summary>
    void OnEnd();

    Transform Parent { get; }
}
