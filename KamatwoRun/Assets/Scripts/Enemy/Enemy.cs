using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �i��ɕ�܂�邱�Ƃ��ł���j�G�N���X
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
