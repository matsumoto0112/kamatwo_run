using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChecker : MonoBehaviour
{
    private LanePositions lanePositions = null;
    private SubStage subStage = null;

    // Start is called before the first frame update
    void Start()
    {
        lanePositions = transform.parent.GetComponentInChildren<LanePositions>();
        subStage = null;
    }

    private void Update()
    {
        if (subStage == null)
        {
            return;
        }

        if(lanePositions.CurveToChangeLanePosition(subStage) == true)
        {
            subStage = null;
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
            this.subStage = subStage;
            lanePositions.GetEntranceDirection(subStage);
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
