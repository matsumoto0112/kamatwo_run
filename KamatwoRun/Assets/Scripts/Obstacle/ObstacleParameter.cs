using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Q���̃p�����[�^
/// </summary>
[CreateAssetMenu(menuName = "Parameters/Obstacle")]
public class ObstacleParameter : ScriptableObject
{
    [Tooltip("�z�u�^�C�v")]
    public PlacementType type;
    [Tooltip("�_���[�W��")]
    public float damage;
}
