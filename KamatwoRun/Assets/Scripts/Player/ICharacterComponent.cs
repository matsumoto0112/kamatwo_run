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

    /// <summary>
    /// �e�I�u�W�F�N�g
    /// </summary>
    Transform Parent { get; }

    /// <summary>
    /// ���f���̃I�u�W�F�N�g
    /// </summary>
    Transform CharacterTransform { get; }
}
