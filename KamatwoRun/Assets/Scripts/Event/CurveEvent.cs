using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�u���̃C�x���g
/// </summary>
public class CurveEvent : BaseEvent
{
    private PlayerInput playerInput = null;
    private PlayerStatus playerStatus = null;
    private LanePositions lanePositions = null;

    private GameSpeed gameSpeed = null;

    private Transform cameraParent = null;
    private Vector3 initCameraPosition = Vector3.zero;
    private Vector3 initCameraAngle = Vector3.zero;

    private Timer timer;

    private const float CURVE_COEF = 15f;

    /// <summary>
    ///�R���X�g���N�^
    /// </summary>
    /// <param name="playerModelObject"></param>
    public CurveEvent(GameObject playerModelObject)
        : base(playerModelObject)
    {
        playerInput = this.playerModelObject.GetComponent<PlayerInput>();
        playerStatus = this.playerModelObject.GetComponent<PlayerStatus>();
        GameObject laneObject = this.playerModelObject.GetComponentInParent<Player>().LaneObject;
        lanePositions = laneObject.GetComponent<LanePositions>();
        gameSpeed = EventManager.Instance.GameSpeed;
        timer = new Timer(2.0f);
    }

    /// <summary>
    /// ����������
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        timer.Initialize();

        //�v���C���[�̍s����������
        playerInput.OnEventInitialize();
        playerStatus.OnEventInitialize();

        //�J�����̏��ۑ�
        cameraParent = Camera.main.transform.parent;
        initCameraPosition = Camera.main.transform.localPosition;
        initCameraAngle = Camera.main.transform.localEulerAngles;

        //�J�������̍X�V
        Camera.main.transform.parent = EventManager.Instance.StageObject.transform;
        Camera.main.transform.position = EventManager.Instance.StageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //�J�������v���C���[�Ɍ�����
        Camera.main.transform.LookAt(playerModelObject.transform);

        //�̂��X����|�C���g�ɓ��B������
        if(EventManager.Instance.IsCurvePoint == true)
        {
            gameSpeed.Speed -= Time.deltaTime * CURVE_COEF;
            gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 5.0f, gameSpeed.DefaultStageMoveSpeed);
            //�i�s�������ω�������
            if(lanePositions.IsChangeDirection() == false)
            {
                EventManager.Instance.IsCurvePoint = false;
                gameSpeed.Initialize();
            }
            return;
        }

        timer.UpdateTimer();
        if(timer.IsTime() == true)
        {
            IsEnd = true;
        }
    }

    /// <summary>
    /// �I������
    /// </summary>
    /// <returns></returns>
    public override bool OnEnd()
    {
        if(IsEnd == false)
        {
            return false;
        }

        //�J�����̏������ɖ߂�
        Camera.main.transform.parent = cameraParent;
        Camera.main.transform.localPosition = initCameraPosition;
        Camera.main.transform.localEulerAngles = initCameraAngle;

        return true;
    }
}
