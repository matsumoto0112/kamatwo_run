using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanePositions : MonoBehaviour
{
    private Vector3 entranceDirection = Vector3.zero;
    private GameObject modelObject = null;
    private SubStage subStageObject = null;
    private Vector3 initModelAngle = Vector3.zero;
    private Timer curveTimer;

    public List<Transform> LanePositionList { get; private set; }

    private const float tiltNum = 45.0f;

    public void Initialize()
    {
        LanePositionList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            LanePositionList.Add(transform.GetChild(i));
        }
        entranceDirection = Vector3.zero;
        modelObject = transform.parent.GetChild(0).gameObject;
        //Camera.main.transform.parent = transform;
        subStageObject = null;
        curveTimer = new Timer();
    }

    public void GetEntranceDirection(SubStage subStage)
    {
        entranceDirection = subStage.GetForegroundDirection(transform.parent.GetChild(0).position);
        subStageObject = subStage;
        initModelAngle = modelObject.transform.eulerAngles;
        curveTimer.Initialize();
    }

    /// <summary>
    /// カーブ中体を傾ける
    /// </summary>
    public void CurveTiltBody()
    {
        curveTimer.UpdateTimer();
        if (curveTimer.IsTime() == true)
        {
            return;
        }
        float y = Mathf.LerpAngle(initModelAngle.y, GetExitPlayerAngle().y, curveTimer.CurrentTime);
        float z = Mathf.LerpAngle(initModelAngle.z, GetTiltAngle().z, curveTimer.CurrentTime);
        modelObject.transform.eulerAngles = new Vector3(0, y, z);
    }

    /// <summary>
    /// 曲がる際にレーン移動の位置を変更する
    /// </summary>
    /// <param name="subStage"></param>
    public bool CurveToChangeLanePosition()
    {
        //進む方向が変わったら
        if (entranceDirection == subStageObject.GetForegroundDirection(transform.parent.GetChild(0).position))
        {
            return false;
        }

        //レーンの位置をモデルの位置にする
        transform.position = modelObject.transform.position;
        LaneLocationType type = transform.parent.GetComponentInChildren<PlayerMove>().LocationType;
        modelObject.transform.eulerAngles = GetExitPlayerAngle();
        transform.eulerAngles = GetExitPlayerAngle();

        switch (subStageObject.ExitType)
        {
            case GatewayType.East:
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition -= new Vector3(0.0f, 0.0f, 2.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition += new Vector3(0.0f, 0.0f, 2.0f);
                break;
            case GatewayType.West:
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition += new Vector3(0.0f, 0.0f, 2.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition -= new Vector3(0.0f, 0.0f, 2.0f);

                break;
            case GatewayType.South:
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition -= new Vector3(2.0f, 0.0f, 0.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition += new Vector3(2.0f, 0.0f, 0.0f);

                break;
            case GatewayType.North:
                if (type == LaneLocationType.LEFT_SIDE)
                    transform.localPosition += new Vector3(2.0f, 0.0f, 0.0f);
                else if (type == LaneLocationType.RIGHT_SIDE)
                    transform.localPosition -= new Vector3(2.0f, 0.0f, 0.0f);
                break;
        }
        subStageObject = null;
        return true;
    }

    /// <summary>
    /// 出口の方向から角度を返す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Vector3 GetExitPlayerAngle()
    {
        switch (subStageObject.ExitType)
        {
            case GatewayType.East:
                return new Vector3(0.0f, 90.0f, 0.0f);
            case GatewayType.West:
                return new Vector3(0.0f, -90.0f, 0.0f);
            case GatewayType.South:
                return new Vector3(0.0f, 180.0f, 0.0f);
            case GatewayType.North:
                return Vector3.zero;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 方向から傾きを返す
    /// </summary>
    /// <returns></returns>
    private Vector3 GetTiltAngle()
    {
        //N->E
        if (subStageObject.EntranceType == GatewayType.North &&
            subStageObject.ExitType == GatewayType.East)
        {
            return new Vector3(0.0f, 0.0f, tiltNum);
        }
        //N->W
        else if (subStageObject.EntranceType == GatewayType.North &&
            subStageObject.ExitType == GatewayType.West)
        {
            return new Vector3(0.0f, 0.0f, -tiltNum);
        }
        //E->N
        else if (subStageObject.EntranceType == GatewayType.East &&
            subStageObject.ExitType == GatewayType.North)
        {
            return new Vector3(0.0f, 0.0f, -tiltNum);
        }
        //E->S
        else if (subStageObject.EntranceType == GatewayType.East &&
           subStageObject.ExitType == GatewayType.South)
        {
            return new Vector3(0.0f, 0.0f, tiltNum);
        }
        //S->E
        else if (subStageObject.EntranceType == GatewayType.South &&
           subStageObject.ExitType == GatewayType.East)
        {
            return new Vector3(0.0f, 0.0f, -tiltNum);
        }
        //S->W
        else if (subStageObject.EntranceType == GatewayType.South &&
            subStageObject.ExitType == GatewayType.West)
        {
            return new Vector3(0.0f, 0.0f, tiltNum);
        }
        //W->N
        else if (subStageObject.EntranceType == GatewayType.West &&
            subStageObject.ExitType == GatewayType.North)
        {
            return new Vector3(0.0f, 0.0f, tiltNum);
        }
        //W->E
        else if (subStageObject.EntranceType == GatewayType.West &&
             subStageObject.ExitType == GatewayType.East)
        {
            return new Vector3(0.0f, 0.0f, -tiltNum);
        }
        return Vector3.zero;
    }
}
