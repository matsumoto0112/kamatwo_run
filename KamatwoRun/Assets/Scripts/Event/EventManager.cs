using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    [SerializeField]
    private GameObject stageManagerObject = null;
    private StageManager stageManager = null;
    private GameSpeed gameSpeed = null;

    private IEnumerator coroutine = null;
    private GameObject playerModelObject = null;
    private Timer eventTimer;

    private const float curveCoef = 26.5f;

    //
    public bool StartEventFlag { get; private set; }

    //�Q�[�����C�x���g�t���O
    public bool EventFlag
    {
        get
        {
            return coroutine != null;
        }
    }

    /// <summary>
    /// �J�[�u�|�C���g�ɍ����|���������ǂ���
    /// </summary>
    public bool IsCurvePoint { get; set; }

    protected override void Awake()
    {
        coroutine = null;
        base.Awake();
        stageManager = stageManagerObject.GetComponent<StageManager>();
        gameSpeed = stageManagerObject.GetComponent<GameSpeed>();
        //�v���C���[���f���擾
        playerModelObject = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        eventTimer = new Timer(2.0f);
        IsCurvePoint = false;
    }

    private void Start()
    {
        StartEventFlag = false;
        coroutine = GameStartEvent();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// �Q�[���X�^�[�g���̃C�x���g����
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStartEvent()
    {
        yield return new WaitForSeconds(Time.deltaTime);



        StartEventFlag = true;
    }

    /// <summary>
    /// �J�[�u�C�x���g
    /// </summary>
    /// <param name="subStageObject"></param>
    public void CurveEvent(GameObject subStageObject)
    {
        coroutine = CurveEventCoroutine(subStageObject);
        StartCoroutine(coroutine);
    }

    private IEnumerator CurveEventCoroutine(GameObject subStageObject)
    {
        //���o���̓X�e�[�W���폜���Ȃ�
        stageManager.stageDeletable = false;
        playerModelObject.GetComponent<PlayerInput>().OnEventInitialize();

        //�J�����̐e�I�u�W�F�N�g��ۑ�
        Transform cameraParent = Camera.main.transform.parent;
        Vector3 localPosition = Camera.main.transform.localPosition;
        Vector3 localEulerAngle = Camera.main.transform.localEulerAngles;

        //�J�������T�u�X�e�[�W�̎q�I�u�W�F�N�g�ɂ���
        Camera.main.transform.parent = subStageObject.transform;
        Camera.main.transform.position = subStageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;

        while (true)
        {
            //�J�������v���C���[�Ɍ�����
            Camera.main.transform.LookAt(playerModelObject.transform);
            if (IsCurvePoint == true)
            {
                gameSpeed.Speed -= Time.deltaTime * curveCoef;
                gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 0.0f, gameSpeed.DefaultStageMoveSpeed);
                //���������ċȂ������猳�̑��x�ɖ߂�
                if (gameSpeed.Speed <= 0.0f)
                {
                    IsCurvePoint = false;
                    gameSpeed.Initialize();
                }
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }

            eventTimer.UpdateTimer();
            if (eventTimer.IsTime() == true)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //�J���������̈ʒu��
        Camera.main.transform.parent = cameraParent;
        Camera.main.transform.localPosition = localPosition;
        Camera.main.transform.localEulerAngles = localEulerAngle;

        coroutine = null;
        eventTimer.Initialize();
        stageManager.stageDeletable = true;
    }
}
