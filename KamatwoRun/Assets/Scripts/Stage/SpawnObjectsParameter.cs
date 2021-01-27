using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �X�|�[���I�u�W�F�N�g�̎��
/// </summary>
public enum SpawnObjectType
{
    ObstacleSingle,
    ObstacleWide,
    ObstacleHigh,
    Enemy,
    ScoreObject,
}

public class SpawnObjectTypeExtend
{
    public static SpawnObjectType GetRandom()
    {
        int max = System.Enum.GetValues(typeof(SpawnObjectType)).Length;
        int index = Random.Range(0, max);
        return (SpawnObjectType)index;
    }
}

[System.Serializable]
public struct SpawnObject
{
    [Tooltip("�X�|�[���Ώۂ̎��")]
    public SpawnObjectType type;
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
    public SpawnObjectType GetRandomObject()
    {
        float r = Random.Range(0.0f, 1.0f);
        foreach (var o in spawnObjects)
        {
            if (r <= o.selectRate)
            {
                return o.type;
            }
            r -= o.selectRate;
        }

        return spawnObjects.Last().type;
    }
}
