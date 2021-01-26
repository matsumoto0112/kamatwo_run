using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 障害物オブジェクト
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
