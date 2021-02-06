using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    [SerializeField]
    private GameObject stageManagerObject = null;
    [SerializeField]
    private GameObject playerCanvas = null;
    [SerializeField]
    private GameObject startEventStagePrefab = null;
    [SerializeField]
    private GameObject eventCanvasObject = null;
    [SerializeField]
    private SceneChangeRelay changeRelay = null;

    private StageManager stageManager = null;
    private GameSpeed gameSpeed = null;

    private IEnumerator coroutine = null;
    private GameObject playerModelObject = null;   //�v���C���[���f���I�u�W�F�N�g
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
        eventTimer = new Timer(1.5f);
        IsCurvePoint = false;

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
        //���o���̓X�e�[�W��j�������Ȃ�
        stageManager.stageDeletable = false;
        playerCanvas.SetActive(false);

        EventTextDisplay eventTextDisplay = eventCanvasObject.GetComponent<EventTextDisplay>();
        eventTextDisplay.Initialize();

        //�X�^�[�g���̃v���C���[���ۑ�
        Vector3 initialPlayerPosition = playerModelObject.transform.localPosition;
        Vector3 initialPlayerRotate = playerModelObject.transform.localEulerAngles;
        //�����̃J�������ۑ�
        Vector3 initialCameraPosition = Camera.main.transform.position;
        Vector3 initialCameraRotate = Camera.main.transform.eulerAngles;

        Vector3 startPosition = new Vector3(0.0f, 0.0f, -50.0f);
        playerModelObject.transform.position = startPosition + new Vector3(0.0f, 0.5f, 0.0f);
        GameObject startStage1 = Instantiate(startEventStagePrefab, startPosition, Quaternion.identity);

        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.position = playerModelObject.transform.position + (playerModelObject.transform.forward * 30) + new Vector3(0.0f, 2.0f, 0.0f);

        float alpha = 0.0f;
        //�v���C���[���J�����Ɍ������đ����Ă���
        while (true)
        {
            playerModelObject.transform.position += playerModelObject.transform.forward * Time.deltaTime * 7.5f;
            alpha += Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            eventTextDisplay.SetFrameAlpha(alpha);
            yield return new WaitForSeconds(Time.deltaTime);
            if ((Camera.main.transform.position.z - playerModelObject.transform.position.z) <= 5.0f)
            {
                break;
            }
            if (alpha >= 1.0f)
            {
                eventTextDisplay.FirstText();
            }
        }

        //�J�������v���C���[�̑��ʂ��ڂ��悤�ɂ���
        Camera.main.transform.position = playerModelObject.transform.position + new Vector3(5.0f, 2.5f, 0.0f);
        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.parent = playerModelObject.transform;

        Timer timer = new Timer(2.0f);
        eventTextDisplay.SecondText();
        //�ēx�i��
        while (true)
        {
            playerModelObject.transform.position += playerModelObject.transform.forward * Time.deltaTime * 7.5f;
            timer.UpdateTimer();
            yield return new WaitForSeconds(Time.deltaTime);
            if (timer.IsTime() == true)
            {
                break;
            }
        }

        //���o�I������
        eventTextDisplay.Initialize();

        Camera.main.transform.parent = null;
        playerModelObject.transform.localPosition = initialPlayerPosition;
        playerModelObject.transform.localEulerAngles = initialPlayerRotate;


        Camera.main.transform.position = initialCameraPosition;
        Camera.main.transform.eulerAngles = initialCameraRotate;
        Camera.main.transform.parent = playerModelObject.transform.parent.GetComponentInChildren<LanePositions>().transform;

        Destroy(startStage1);

        playerCanvas.SetActive(true);
        playerCanvas.GetComponent<StatusDisplay>().OnEventEndInitialize();

        stageManager.stageDeletable = true;
        StartEventFlag = true;
        coroutine = null;
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

    public void GoalEvent(GameObject target, GameObject stageObject)
    {
        coroutine = GoalCoroutine(target, stageObject);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// �J�[�u���̉��o����
    /// </summary>
    /// <param name="subStageObject"></param>
    /// <returns></returns>
    private IEnumerator CurveEventCoroutine(GameObject subStageObject)
    {
        //���o���̓X�e�[�W���폜���Ȃ�
        stageManager.stageDeletable = false;
        playerModelObject.GetComponent<PlayerInput>().OnEventInitialize();
        playerModelObject.GetComponent<PlayerStatus>().OnEventInitialize();

        //�J�����̐e�I�u�W�F�N�g��ۑ�
        Transform cameraParent = Camera.main.transform.parent;
        Vector3 localPosition = Camera.main.transform.localPosition;
        Vector3 localEulerAngle = Camera.main.transform.localEulerAngles;

        //�J�������T�u�X�e�[�W�̎q�I�u�W�F�N�g�ɂ���
        Camera.main.transform.parent = subStageObject.transform;
        Camera.main.transform.position = subStageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;
        GameObject laneObject = playerModelObject.GetComponentInParent<Player>().LaneObject;
        LanePositions lane = laneObject.GetComponent<LanePositions>();
        while (true)
        {
            Camera.main.transform.LookAt(playerModelObject.transform);
            //�J�������v���C���[�Ɍ�����
            if (IsCurvePoint == true)
            {
                gameSpeed.Speed -= Time.deltaTime * curveCoef;
                gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 0.0f, gameSpeed.DefaultStageMoveSpeed);
                //�i�s�������ω�������
                if (lane.IsChangeDirection() == false)
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

    private IEnumerator GoalCoroutine(GameObject target, GameObject stageObject)
    {
        stageManager.stageDeletable = false;
        playerModelObject.GetComponent<PlayerInput>().OnEventInitialize();
        playerModelObject.GetComponent<PlayerStatus>().OnEventInitialize();

        //�J�����ǐՒ�~
        Camera.main.transform.parent = stageObject.transform;
        Timer timer = new Timer(1.5f);
        while (true)
        {
            Camera.main.transform.LookAt(target.transform);
            timer.UpdateTimer();
            yield return new WaitForSeconds(Time.deltaTime);
            if (timer.IsTime() == true)
            {
                break;
            }
        }

        changeRelay.Next();
        coroutine = null;
    }
}
