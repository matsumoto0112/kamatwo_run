using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanePositions : MonoBehaviour
{
    private Vector3 entranceDirection = Vector3.zero;
    private GameObject modelObject = null;


    public List<Transform> LanePositionList { get; private set; }

    public void Initialize()
    {
        LanePositionList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            LanePositionList.Add(transform.GetChild(i));
        }
        entranceDirection = Vector3.zero;
        modelObject = transform.parent.GetChild(0).gameObject;
        Camera.main.transform.parent = transform;
    }

    public void GetEntranceDirection(SubStage subStage)
    {
        entranceDirection = subStage.GetForegroundDirection(transform.parent.GetChild(0).position);
    }

    /// <summary>
    /// 曲がる際にレーン移動の位置を変更する
    /// </summary>
    /// <param name="subStage"></param>
    public bool CurveToChangeLanePosition(SubStage subStage)
    {
        //進む方向が変わったら
        if (entranceDirection == subStage.GetForegroundDirection(transform.parent.GetChild(0).position))
        {
            return false;
        }

        //レーンの位置をモデルの位置にする
        transform.position = modelObject.transform.position;
        LaneLocationType type = transform.parent.GetComponentInChildren<PlayerMove>().LocationType;
        switch (subStage.ExitType)
        {
            case GatewayType.East:
                modelObject.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition -= new Vector3(0.0f, 0.0f, 2.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition += new Vector3(0.0f, 0.0f, 2.0f);
                break;
            case GatewayType.West:
                modelObject.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition += new Vector3(0.0f, 0.0f, 2.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition -= new Vector3(0.0f, 0.0f, 2.0f);

                break;
            case GatewayType.South:
                modelObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition -= new Vector3(2.0f, 0.0f, 0.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition += new Vector3(2.0f, 0.0f, 0.0f);

                break;
            case GatewayType.North:
                modelObject.transform.eulerAngles = Vector3.zero;
                transform.eulerAngles = Vector3.zero;
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition += new Vector3(2.0f, 0.0f, 0.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition -= new Vector3(2.0f, 0.0f, 0.0f);
                break;
        }

        return true;
    }
}
