using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1���[�����Ǘ�����N���X
/// </summary>
public abstract class Lane : MonoBehaviour
{
    public abstract Vector3? GetRandomSpawnPoint();
}
