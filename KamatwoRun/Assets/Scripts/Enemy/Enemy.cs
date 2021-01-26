using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// （皮に包まれることができる）敵クラス
/// </summary>
public class Enemy : WrappableObject
{
    [SerializeField]
    private EnemyParameter enemyParameter;

    public override PlacementType GetPlacementType()
    {
        return enemyParameter.type;
    }

    public override WrappedPoint Wrap()
    {
        return new WrappedPoint(enemyParameter.score, enemyParameter.recover);
    }
}
