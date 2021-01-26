using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Q���I�u�W�F�N�g
/// </summary>
public class Obstacle : StageObject
{
    [SerializeField]
    private ObstacleParameter obstacleParameter;

    public override PlacementType GetPlacementType()
    {
        return obstacleParameter.type;
    }

    public ObstacleParameter GetParameter { get { return obstacleParameter; } }
}
