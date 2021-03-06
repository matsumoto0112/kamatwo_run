using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChecker : MonoBehaviour
{
    private EventManager eventManager;
    private LanePositions lanePositions = null;
    private bool isCurve = false;

    // Start is called before the first frame update
    void Start()
    {
        lanePositions = transform.parent.GetComponentInChildren<LanePositions>();
        eventManager = transform.parent.GetComponent<Player>().EventManager;
        isCurve = false;
    }

    private void Update()
    {
        if (isCurve == false)
        {
            return;
        }

        if(eventManager.IsCurvePoint == true)
        {
            lanePositions.CurveTiltBody();
        }

        if (lanePositions.CurveToChangeLanePosition() == true)
        {
            isCurve = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //ステージに接触したら
        if (other.gameObject.GetComponentToNullCheck(out SubStage subStage) == true)
        {
            //まっすぐな直線の道なら
            if (IsNoCurveCheck(subStage) == true)
            {
                return;
            }

            //次のステージが曲がり角なのを表す
            isCurve = true;
            lanePositions.GetEntranceDirection(subStage);
        }
        else if (other.CompareTag("CurvePoint") == true)
        {
            eventManager.IsCurvePoint = true;
        }
    }

    /// <summary>
    /// 曲がれないステージの確認
    /// </summary>
    /// <param name="subStage"></param>
    /// <returns></returns>
    private bool IsNoCurveCheck(SubStage subStage)
    {
        //入口：北 出口：南
        bool ns = subStage.EntranceType == GatewayType.North && subStage.ExitType == GatewayType.South;
        //入口：南 出口：北
        bool sn = subStage.EntranceType == GatewayType.South && subStage.ExitType == GatewayType.North;
        //入口：東 出口：西
        bool ew = subStage.EntranceType == GatewayType.East && subStage.ExitType == GatewayType.West;
        //入口：西 出口：東
        bool we = subStage.EntranceType == GatewayType.West && subStage.ExitType == GatewayType.East;
        return ns || sn || ew || we;
    }
}
