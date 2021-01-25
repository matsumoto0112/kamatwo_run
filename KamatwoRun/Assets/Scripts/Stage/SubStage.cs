using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// �������ꂽ��X�e�[�W���Ǘ�����
/// </summary>
public class SubStage : MonoBehaviour
{
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

    [SerializeField, Tooltip("�z�u�\�ȃI�u�W�F�N�g���X�g")]
    private List<GameObject> stageObjects;

    //�����ς݃I�u�W�F�N�g
    private List<GameObject> spawnedObjects;

    [SerializeField]
    private StageParameter stageParameter;
    [SerializeField, Tooltip("�����̎��")]
    private GatewayType entranceType;
    [SerializeField, Tooltip("�����̎��")]
    private GatewayType exitType;

    public GatewayType EntranceType { get { return entranceType; } }
    public GatewayType ExitType { get { return exitType; } }

    //�R���W����
    private BoxCollider boxCollider;


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
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        spawnedObjects = new List<GameObject>();

        //�����_���Ȑ��A�G�𐶐�����
        int spawnNum = Random.Range(1, 2);
        for (int i = 0; i < spawnNum; i++)
        {
            //�����_���ȃ��[���ԍ����擾����
            int laneNum = Random.Range(0, 3);
            Vector3 basePosition = this.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 laneOffset = (laneNum - (stageParameter.laneNum / 2)) * new Vector3(stageParameter.stageWidth / 4.0f, 0, 0);
            Vector3 spawnPosition = basePosition + laneOffset;

            GameObject obj = Instantiate(stageObjects[0], spawnPosition, Quaternion.identity, this.transform);
            spawnedObjects.Add(obj);
        }

        Assert.IsNotNull(boxCollider, "BoxCollider���A�^�b�`����Ă��܂���");
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    /// <summary>
    /// ��O�̕������擾����
    /// �X�e�[�W�̌`��ɂ���Ď�O�̕������ς�邽��
    /// </summary>
    /// <param name="checkPosition">���ׂ����n�_�̍��W</param>
    /// <returns></returns>
    public virtual Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        return Vector3.back;
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
