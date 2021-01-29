using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_ShapedDirection : DirectionChecker
{
    private float GetAngle(Vector3 dir, Vector3 p)
    {
        dir = dir.normalized;
        Vector3 axis = Vector3.Cross(dir, p);
        float angle = Vector3.Angle(dir, p) * axis.y < 0 ? -1 : 1;
        return angle;
    }

    public override Vector3 Directon(Vector3 checkPosition)
    {
        Vector3 p = (this.transform.position - checkPosition);
        //S->E
        if (entrance == GatewayType.South && exit == GatewayType.East)
        {
            if (GetAngle(new Vector3(1.0f, 0.0f, -1.0f), p) <= 0.0f)
            {
                return Vector3.forward;
            }
            else
            {

                return Vector3.right;
            }
        }
        //E->S
        else if (entrance == GatewayType.East && exit == GatewayType.South)
        {
            if (GetAngle(new Vector3(1.0f, 0.0f, -1.0f), p) <= 0.0f)
            {
                return Vector3.back;
            }
            else
            {
                return Vector3.left;
            }
        }
        //W->S
        else if (entrance == GatewayType.West && exit == GatewayType.South)
        {
            if (GetAngle(new Vector3(-1.0f, 0.0f, -1.0f), p) <= 0.0f)
            {
                return Vector3.right;
            }
            else
            {
                return Vector3.back;
            }
        }
        //S->W
        else if (entrance == GatewayType.South && exit == GatewayType.West)
        {
            if (GetAngle(new Vector3(-1.0f, 0.0f, -1.0f), p) <= 0.0f)
            {
                return Vector3.left;
            }
            else
            {
                return Vector3.forward;
            }
        }
        //N->W
        else if (entrance == GatewayType.North && exit == GatewayType.West)
        {
            if (GetAngle(new Vector3(-1.0f, 0.0f, 1.0f), p) <= 0.0f)
            {
                return Vector3.back;
            }
            else
            {
                return Vector3.left;
            }
        }
        //W->N
        else if (entrance == GatewayType.West && exit == GatewayType.North)
        {
            if (GetAngle(new Vector3(-1.0f, 0.0f, 1.0f), p) <= 0.0f)
            {
                return Vector3.forward;
            }
            else
            {
                return Vector3.right;
            }
        }
        //E->N
        else if (entrance == GatewayType.East && exit == GatewayType.North)
        {
            if (GetAngle(new Vector3(1.0f, 0.0f, 1.0f), p) <= 0.0f)
            {
                return Vector3.left;
            }
            else
            {
                return Vector3.forward;
            }
        }
        //N->E
        else if (entrance == GatewayType.East && exit == GatewayType.North)
        {
            if (GetAngle(new Vector3(1.0f, 0.0f, 1.0f), p) <= 0.0f)
            {
                return Vector3.right;
            }
            else
            {
                return Vector3.back;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
}
