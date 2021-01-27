using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_ShapedDirection : DirectionChecker
{
    public override Vector3 Directon(Vector3 checkPosition)
    {
        //“ì‚©‚ç“Œ‚ÉŒü‚©‚¤Žž‚Ì•ûŒü
        if (entrance == GatewayType.South && exit == GatewayType.East)
        {
            Vector3 dir = new Vector3(1.0f, 0.0f, -1.0f).normalized;
            Vector3 p = (this.transform.position - checkPosition);
            Vector3 axis = Vector3.Cross(dir, p);
            float angle = Vector3.Angle(dir, p) * axis.y < 0 ? -1 : 1;
            if (angle <= 0.0f)
            {
                return Vector3.forward;
            }
            else
            {
                return Vector3.right;
            }
        }

        return Vector3.zero;
    }
}
