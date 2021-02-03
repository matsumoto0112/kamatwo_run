using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveCameraEvent : MonoBehaviour
{
    [SerializeField]
    private Vector3 eventCameraPosition = Vector3.zero;

    public Vector3 EventCameraPosition
    {
        get
        {
            return eventCameraPosition + transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(EventCameraPosition, 0.5f);
    }
}
