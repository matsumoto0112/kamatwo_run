using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1���[�����Ǘ�����N���X
/// </summary>
public abstract class Lane : MonoBehaviour
{
    /// <summary>
    /// �X�|�[���|�C���g�����݂���΂��̊Ԃ̃����_���Ȓn�_��Ԃ�
    /// </summary>
    /// <returns></returns>
    public abstract Vector3? GetRandomSpawnPoint();
}
