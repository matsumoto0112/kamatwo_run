using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�p�����[�^
/// </summary>
[CreateAssetMenu(menuName = "Parameters/Enemy")]
public class EnemyParameter : ScriptableObject
{
    [Tooltip("�z�u�^�C�v")]
    public PlacementType type;
    [Tooltip("�|�������̃X�R�A")]
    public int score;
    [Tooltip("�G�ꂽ���̃_���[�W��")]
    public float damage;
    [Tooltip("�|�������̉񕜗�")]
    public float recover;
}
