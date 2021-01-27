using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> subStagePrefabs;

    [SerializeField]
    private GameObject player;

    //�Q�[�����x�Ǘ�
    private GameSpeed gameSpeed;

    //�������ꂽ�T�u�X�e�[�W�Q
    private List<SubStage> subStages;

    //�T�u�X�e�[�W�̐����`�̈�ӂ̒���
    private static readonly float SubStageUnit = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = GetComponent<GameSpeed>();
        subStages = new List<SubStage>();

        //�ŏ��̃X�e�[�W���͌Œ�̂��̂��g�p����
        {
            //������Ƃ�����O�ɂ��炷
            Vector3 pos = new Vector3(0, 0, SubStageUnit / 2.0f - 5.0f);
            GameObject newObject = Instantiate(subStagePrefabs[0], pos, Quaternion.identity);
            subStages.Add(newObject.GetComponent<SubStage>());
        }

        //���O�쐬�X�e�[�W��
        const int preSpawnSubStageNum = 5;
        for (int i = 0; i < preSpawnSubStageNum; i++)
        {
            SpawnNextSubStage();
        }
    }

    void Update()
    {
        Vector3 scrollDirection = GetForegroundDirection(player.transform.position);
        float speed = gameSpeed.Speed * Time.deltaTime;
        foreach (var st in subStages)
        {
            st.Move(scrollDirection, speed);
        }

        SubStage frontStage = subStages.First();
        if (!frontStage.IsInsideCamera)
        {
            //�v���C���[���O�ɂ������̂Ŕ͈͊O�ɂȂ������߁A�폜����
            Destroy(frontStage.gameObject);
            subStages.RemoveAt(0);

            //�V�����ǉ�����
            SpawnNextSubStage();
        }
    }

    /// <summary>
    /// ���̃X�e�[�W�v���n�u���擾����
    /// </summary>
    /// <param name="type">���̃X�e�[�W�̓����̎��</param>
    /// <returns>�����̎�ނ���v����X�e�[�W�̃v���n�u��Ԃ�</returns>
    private GameObject GetNextSubStagePrefab(GatewayType type)
    {
        //100�񂾂��e�X�g����
        for (int i = 0; i < 100; i++)
        {
            int prefabIndex = Random.Range(0, subStagePrefabs.Count);
            GameObject prefab = subStagePrefabs[prefabIndex];
            SubStage prefabSubStage = prefab.GetComponent<SubStage>();
            if (prefabSubStage.EntranceType == type)
            {
                return prefab;
            }
        }

        Debug.Log("Failed to GetNextSubStagePrefab. Maybe mismatched GatewayType.");
        return null;
    }

    /// <summary>
    /// �T�u�X�e�[�W��z�u����I�t�Z�b�g�l���擾����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Vector3 SubStageOffset(GatewayType type)
    {
        switch (type)
        {
            case GatewayType.North:
                return new Vector3(0, 0, SubStageUnit);
            case GatewayType.South:
                return new Vector3(0, 0, -SubStageUnit);
            case GatewayType.East:
                return new Vector3(SubStageUnit, 0, 0);
            default:
                return new Vector3(-SubStageUnit, 0, 0);
        }
    }

    /// <summary>
    /// ���̃T�u�X�e�[�W�𐶐�����
    /// </summary>
    private void SpawnNextSubStage()
    {
        //����ł͂܂������̃X�e�[�W�݂̂Ȃ̂ŉ����l�����ɐ�������
        SubStage prevStage = subStages.Last();
        GameObject next = GetNextSubStagePrefab(GatewayTypeExtend.ChainableType(prevStage.ExitType));
        Debug.Log(next.GetComponent<SubStage>().EntranceType + "" + next.GetComponent<SubStage>().ExitType);
        Vector3 spawnPosition = prevStage.transform.position + SubStageOffset(prevStage.ExitType);
        GameObject newObject = Instantiate(next, spawnPosition, Quaternion.identity);
        SubStage newSubStage = newObject.GetComponent<SubStage>();
        newSubStage.SpawnObjects(5);
        subStages.Add(newSubStage);
    }

    /// <summary>
    /// �O�������i�X�e�[�W�̈ړ������j���擾����
    /// </summary>
    /// <param name="checkPosition"></param>
    /// <returns></returns>
    public Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        foreach (var st in subStages)
        {
            if (st.IsInArea(checkPosition))
            {
                return st.GetForegroundDirection(checkPosition);
            }
        }

        Debug.Log("���ׂ������W�������ς݂̃X�e�[�W���ɂ���܂���");
        return Vector3.back;
    }
}
