using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedDirection : DirectionChecker
{
    [SerializeField]
    private Vector3 fixedDirection;

    public override Vector3 Directon(Vector3 checkPosition)
    {
        return fixedDirection.normalized;
    }
}
