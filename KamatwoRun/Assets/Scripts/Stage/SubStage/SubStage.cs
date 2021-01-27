using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// �o�����̎��
/// �o���̃^�C�v�ƈ�v�������������T�u�X�e�[�W���A���\�ł���
/// </summary>
public enum GatewayType
{
    North,
    South,
    East,
    West,
}

public static class GatewayTypeExtend
{
    public static GatewayType ChainableType(GatewayType type)
    {
        switch (type)
        {
            case GatewayType.North:
                return GatewayType.South;
            case GatewayType.South:
                return GatewayType.North;
            case GatewayType.East:
                return GatewayType.West;
            default:
                return GatewayType.East;
        }
    }
}

/// <summary>
/// �������ꂽ��X�e�[�W���Ǘ�����
/// </summary>
public class SubStage : MonoBehaviour
{


    [SerializeField, Tooltip("�z�u�\�ȃI�u�W�F�N�g���X�g")]
    private SpawnObjectsParameter stageObjects;

    //�����ς݃I�u�W�F�N�g
    private List<GameObject> spawnedObjects;

    [SerializeField]
    private StageParameter stageParameter;
    [SerializeField, Tooltip("�����̎��")]
    private GatewayType entranceType;
    [SerializeField, Tooltip("�o���̎��")]
    private GatewayType exitType;

    public GatewayType EntranceType { get { return entranceType; } }
    public GatewayType ExitType { get { return exitType; } }

    //�R���W����
    private BoxCollider boxCollider;
    //���[���z��
    private List<Lane> lanes;

    //�i�s�����𒲂ׂ�R���|�[�l���g
    private DirectionChecker directionChecker;

    /// <summary>
    /// �J�����͈͓̔����ǂ���
    /// </summary>
    public bool IsInsideCamera
    {
        get
        {
            //���g�̒��S����\���ɐL�т�_�����ƂɁA���̓_���J�����͈͓̔����ǂ������ׂ�
            Vector3 halfSize = boxCollider.size / 2;
            Vector3[] offsets = new Vector3[] {
                new Vector3(-halfSize.x, 0.0f, 0.0f),
                new Vector3(halfSize.x, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, halfSize.z),
                new Vector3(0.0f, 0.0f, -halfSize.x), };

            foreach (var offset in offsets)
            {
                if (CheckInScreen(transform.position + offset)) { return true; }
            }
            return false;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        directionChecker = GetComponent<DirectionChecker>();
        directionChecker.Init(EntranceType, ExitType);
        spawnedObjects = new List<GameObject>();
        lanes = new List<Lane>();
        foreach (var lane in GetComponentsInChildren<Lane>())
        {
            lanes.Add(lane);
        };

        Assert.IsNotNull(boxCollider, "BoxCollider���A�^�b�`����Ă��܂���");
    }

    private float GetYOffset(PlacementType type)
    {
        switch (type)
        {
            case PlacementType.OnlyGround:
                return stageParameter.groundPosition_Y;
            case PlacementType.OnlySky:
                return stageParameter.skyPosition_Y;
            case PlacementType.GroundOrSky:
                {
                    if (Random.Range(0.0f, 1.0f) < 0.5f)
                    {
                        return stageParameter.groundPosition_Y;
                    }
                    else
                    {
                        return stageParameter.skyPosition_Y;
                    }
                }
            default:
                return 0.0f;
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g���X�|�[������
    /// </summary>
    /// <param name="spawnNum"></param>
    public void SpawnObjects(int spawnNum)
    {
        for (int i = 0; i < spawnNum; i++)
        {
            //�����_���ȃ��[���ԍ����擾����
            int laneNum = Random.Range(0, lanes.Count);

            //�X�|�[������Ώۂɉ����Ĕz�u���W���قȂ�ꍇ�����邽�߁A������v�Z����
            GameObject spawnPrefab = stageObjects.GetRandomObject();
            Assert.IsNotNull(spawnPrefab.GetComponent<StageObject>(), "�z�u�\�ȃI�u�W�F�N�g�ł͂���܂���");
            Vector3 offset = new Vector3(0.0f, GetYOffset(spawnPrefab.GetComponent<StageObject>().GetPlacementType()), 0.0f);

            //���̃��[���Ő����\�ȍ��W���擾����
            Vector3? pointOrNull = lanes[laneNum].GetRandomSpawnPoint();
            if (pointOrNull == null) continue;
            Vector3 spawnPosition = pointOrNull.Value +offset;

            //�������W�ɐ������Ȃ��悤�Ƀe�X�g����
            bool spawnTestSucceeded = true;
            foreach (var spawned in spawnedObjects)
            {
                //�������W�l�Ȃ玸�s�Ƃ���
                if (Vector3.Distance(spawned.transform.position, spawnPosition) < Mathf.Epsilon) { spawnTestSucceeded = false; }
            }
            if (spawnTestSucceeded)
            {
                GameObject obj = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity, this.transform);
                spawnedObjects.Add(obj);
            }
        }
    }

    /// <summary>
    /// �����x�N�g������I�u�W�F�N�g�̉�]�������擾����
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Quaternion RotationFromDirectionVector(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == -1)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else if (dir.x == 0 && dir.z == 1)
        {
            return Quaternion.Euler(0, 180, 0);
        }
        else if (dir.x == -1 && dir.z == 0)
        {
            return Quaternion.Euler(0, 90, 0);
        }
        else
        {
            return Quaternion.Euler(0, 270, 0);
        }
    }

    /// <summary>
    /// �T�u�X�e�[�W�I�u�W�F�N�g�̈ړ�����
    /// </summary>
    /// <param name="direction">�ړ�����</param>
    /// <param name="speed">�ړ����x(�������ԍl���ς�)</param>
    public void Move(Vector3 direction, float speed)
    {
        Vector3 moveAmount = direction * speed;
        this.transform.position += moveAmount;

        foreach (var obj in spawnedObjects)
        {
            obj.transform.rotation = RotationFromDirectionVector(direction);
        }
    }

    /// <summary>
    /// ��O�̕������擾����
    /// �X�e�[�W�̌`��ɂ���Ď�O�̕������ς�邽��
    /// </summary>
    /// <param name="checkPosition">���ׂ����n�_�̍��W</param>
    /// <returns></returns>
    public virtual Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        return -directionChecker.Directon(checkPosition);
    }

    /// <summary>
    /// �T�u�X�e�[�W�͈͓̔����ǂ������ׂ�
    /// </summary>
    /// <param name="checkPosition">���ׂ������W</param>
    /// <returns>�͈͓��ł����true��Ԃ�</returns>
    public bool IsInArea(Vector3 checkPosition)
    {
        return boxCollider.bounds.Contains(checkPosition);
    }

    /// <summary>
    /// �w����W���J�����͈͓̔��Ɏ��܂��Ă��邩
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private bool CheckInScreen(Vector3 p)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(p);
        if (viewPos.x < 0.0f || viewPos.x > 1.0f || viewPos.y < 0.0f || viewPos.y > 1.0f)
        {
            return false;
        }
        return true;
    }
}
