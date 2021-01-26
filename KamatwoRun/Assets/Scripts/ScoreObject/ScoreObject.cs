using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���_�I�u�W�F�N�g
/// </summary>
public class ScoreObject : StageObject
{
    [SerializeField, Tooltip("�X�R�A���")]
    private ScoreObjectParameter scoreInfo;

    public override PlacementType GetPlacementType()
    {
        return scoreInfo.type;
    }

    public ScoreObjectParameter ScoreInfo { get { return scoreInfo; } }
}
