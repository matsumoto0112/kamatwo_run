using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// �T�u�X�e�[�W�̌`��
/// </summary>
public enum SubStageShapeType
{
    //����
    Straight,
    //L��
    L_Shape,
}

/// <summary>
/// �T�u�X�e�[�W���
/// </summary>
[System.Serializable]
public struct SubStagePrefabInfo
{
    public SubStageShapeType type;
    public GameObject prefab;
}

/// <summary>
/// �X�e�[�W�Ǘ�
/// </summary>
public class StageManager : MonoBehaviour
{
    [SerializeField]
    private StageParameter stageParameter;
    [SerializeField]
    private List<SubStagePrefabInfo> subStagePrefabs;

    [SerializeField]
    private GameObject player;

    //�v���C���[�̃X�e�[�^�X�Ǘ��I�u�W�F�N�g
    private PlayerStatus playerStatus;
    private int currentWavePhase;

    //���̃X�e�[�W�̌`��\��(null�̎��͎w��Ȃ�)
    private SubStageShapeType? reserveNextStageType;

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
        playerStatus = player.GetComponent<PlayerStatus>();
        currentWavePhase = 0;

        //�ŏ��̃X�e�[�W���͌Œ�̂��̂��g�p����
        {
            //������Ƃ�����O�ɂ��炷
            Vector3 pos = new Vector3(0, 0, SubStageUnit / 2.0f - 5.0f);
            GameObject newObject = Instantiate(subStagePrefabs[0].prefab, pos, Quaternion.identity);
            subStages.Add(newObject.GetComponent<SubStage>());
        }

        //���O�쐬�X�e�[�W��
        const int preSpawnSubStageNum = 3;
        for (int i = 0; i < preSpawnSubStageNum; i++)
        {
            SpawnNextSubStage();
        }

        reserveNextStageType = null;

        Assert.IsNotNull(playerStatus, "PlayerStatus���擾�ł��܂���ł���");
    }

    void Update()
    {
        Vector3 scrollDirection = GetForegroundDirection(player.transform.position);
        float speed = gameSpeed.Speed * Time.deltaTime;
        foreach (var st in subStages)
        {
            st.Move(scrollDirection, speed);
        }

        CalcToNeedNextStageReserve();

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
    /// ���̃X�e�[�W�̌`��\�񂪕K�v���ǂ������v�Z���A�K�v�ł���Η\�񂷂�
    /// </summary>
    private void CalcToNeedNextStageReserve()
    {
        //�E�F�[�u�i�K�ɕω�������Ό`�󂪕ς��
        if (currentWavePhase != stageParameter.GetPhaseByScore(playerStatus.Score))
        {
            currentWavePhase = stageParameter.GetPhaseByScore(playerStatus.Score);
            ReserveNextSubstageShapeType(SubStageShapeType.L_Shape);
        }
    }

    /// <summary>
    /// ���̃X�e�[�W�v���n�u���擾����
    /// </summary>
    /// <param name="type">���̃X�e�[�W�̓����̎��</param>
    /// <returns>�����̎�ނ���v����X�e�[�W�̃v���n�u��Ԃ�</returns>
    private GameObject GetNextSubStagePrefab(GatewayType type)
    {
        List<SubStagePrefabInfo> sameTypePrefabs;
        //���X�e�[�W�̎�ނ̗\�񂪂���΂�����̗p����
        if (reserveNextStageType.HasValue)
        {
            sameTypePrefabs = subStagePrefabs.FindAll(obj => obj.type == reserveNextStageType.Value && obj.prefab.GetComponent<SubStage>().EntranceType == type);
            reserveNextStageType = null;
        }
        else
        {
            sameTypePrefabs = subStagePrefabs.FindAll(obj => obj.type == SubStageShapeType.Straight && obj.prefab.GetComponent<SubStage>().EntranceType == type);
        }

        //���̒�����K���ȃv���n�u��I��
        int prefabIndex = Random.Range(0, sameTypePrefabs.Count());
        GameObject prefab = sameTypePrefabs.ElementAt(prefabIndex).prefab;
        SubStage prefabSubStage = prefab.GetComponent<SubStage>();
        return prefab;
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
        //���O�̃X�e�[�W�ɍ��v����X�e�[�W�𐶐�����
        SubStage prevStage = subStages.Last();
        GameObject next = GetNextSubStagePrefab(GatewayTypeExtend.ChainableType(prevStage.ExitType));
        Vector3 spawnPosition = prevStage.transform.position + SubStageOffset(prevStage.ExitType);
        GameObject newObject = Instantiate(next, spawnPosition, Quaternion.identity);
        SubStage newSubStage = newObject.GetComponent<SubStage>();
        newSubStage.SpawnObjects(stageParameter.GetSpawnObjectsParameterByPhase(currentWavePhase), 5);
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

    /// <summary>
    /// ���̃T�u�X�e�[�W�̌`���\�񂷂�
    /// </summary>
    /// <param name="type"></param>
    public void ReserveNextSubstageShapeType(SubStageShapeType type)
    {
        reserveNextStageType = type;
    }
}
