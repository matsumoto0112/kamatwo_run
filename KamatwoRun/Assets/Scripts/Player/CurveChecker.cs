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
        //�X�e�[�W�ɐڐG������
        if (other.gameObject.GetComponentToNullCheck(out SubStage subStage) == true)
        {
            //�܂������Ȓ����̓��Ȃ�
            if (IsNoCurveCheck(subStage) == true)
            {
                return;
            }

            //���̃X�e�[�W���Ȃ���p�Ȃ̂�\��
            isCurve = true;
            lanePositions.GetEntranceDirection(subStage);
        }
        else if (other.CompareTag("CurvePoint") == true)
        {
            eventManager.IsCurvePoint = true;
        }
    }

    /// <summary>
    /// �Ȃ���Ȃ��X�e�[�W�̊m�F
    /// </summary>
    /// <param name="subStage"></param>
    /// <returns></returns>
    private bool IsNoCurveCheck(SubStage subStage)
    {
        //�����F�k �o���F��
        bool ns = subStage.EntranceType == GatewayType.North && subStage.ExitType == GatewayType.South;
        //�����F�� �o���F�k
        bool sn = subStage.EntranceType == GatewayType.South && subStage.ExitType == GatewayType.North;
        //�����F�� �o���F��
        bool ew = subStage.EntranceType == GatewayType.East && subStage.ExitType == GatewayType.West;
        //�����F�� �o���F��
        bool we = subStage.EntranceType == GatewayType.West && subStage.ExitType == GatewayType.East;
        return ns || sn || ew || we;
    }
}
