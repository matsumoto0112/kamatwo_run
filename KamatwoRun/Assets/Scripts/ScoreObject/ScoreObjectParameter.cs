using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���_���
/// </summary>
[CreateAssetMenu(menuName = "Parameters/ScoreObject")]
public class ScoreObjectParameter : ScriptableObject
{
    [Tooltip("�z�u�^�C�v")]
    public PlacementType type;
    [Tooltip("�l���X�R�A")]
    public int score;
}