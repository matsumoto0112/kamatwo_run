using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct SpawnObject
{
    [Tooltip("�X�|�[���Ώ�")]
    public GameObject prefab;
    [Tooltip("�����_���������ɃX�|�[���Ώۂ��I������銄��"), Range(0.0f, 1.0f)]
    public float selectRate;
}

/// <summary>
/// �I�u�W�F�N�g�̃X�|�[���Ɋւ���p�����[�^
/// </summary>
[CreateAssetMenu(menuName = "Parameters/SpawnObjects")]
public class SpawnObjectsParameter : ScriptableObject
{
    [Tooltip("�X�|�[���Ώۃ��X�g"), Header("selectRate�̍��v�l��1.0�ɂȂ�悤�ɂ��Ă�������")]
    public List<SpawnObject> spawnObjects;

    /// <summary>
    /// �����_���ȃI�u�W�F�N�g���擾����
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomObject()
    {
        float r = Random.Range(0.0f, 1.0f);
        foreach (var o in spawnObjects)
        {
            if (r <= o.selectRate)
            {
                return o.prefab;
            }
            r -= o.selectRate;
        }

        return spawnObjects.Last().prefab;
    }
}
