using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEvent : BaseEvent
{
    private enum EventProgressType
    {
        StartFirstEvent,
        EndFirstEvent,
        StartSecondEvent,
        EndSecondEvent,
    }

    private EventTextDisplay eventTextDisplay = null;

    private Vector3 initPlayerPosition = Vector3.zero;
    private Vector3 initPlayerAngle = Vector3.zero;
    private Vector3 initCameraPosition = Vector3.zero;
    private Vector3 initCameraAngle = Vector3.zero;

    private GameObject startStage = null;
    private Timer timer;
    private EventProgressType type = EventProgressType.StartFirstEvent;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="playerModelObject"></param>
    public GameStartEvent(GameObject playerModelObject,EventManager eventManager)
        : base(playerModelObject,eventManager)
    {
        eventTextDisplay = eventManager.GetEventTextDisplay();
        startStage = null;
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
        type = EventProgressType.StartFirstEvent;
        eventTextDisplay.Initialize();

        eventManager.ChangeCanvasActive(false);

        //�v���C���[�̏��ۑ�
        initPlayerPosition = playerModelObject.transform.localPosition;
        initPlayerAngle = playerModelObject.transform.localEulerAngles;

        //�J�����̏��ۑ�
        initCameraPosition = Camera.main.transform.position;
        initCameraAngle = Camera.main.transform.eulerAngles;

        //���o���Ɏg�p����X�e�[�W�̐���
        startStage = eventManager.SpawnStartStage();
        playerModelObject.transform.position = startStage.transform.position + new Vector3(0.0f, 0.5f, 0.0f);

        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.position = playerModelObject.transform.position + (playerModelObject.transform.forward * 30.0f) + new Vector3(0.0f, 2.0f, 0.0f);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //�v���C���[��O�i������
        playerModelObject.transform.position += playerModelObject.transform.forward * Time.deltaTime * 10.0f;
        eventTextDisplay.ChangeAlpha();

        //�C�x���g�̐i���x
        switch (type)
        {
            case EventProgressType.StartFirstEvent:
                StartFirstEvent();
                break;
            case EventProgressType.EndFirstEvent:
                UpdateCameraInfo();
                break;
            case EventProgressType.StartSecondEvent:
                StartSecondEvent();
                break;
            case EventProgressType.EndSecondEvent:
                IsEnd = true;
                break;
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

        eventTextDisplay.Initialize();

        Camera.main.transform.parent = null;
        //�v���C���[�X�V�̏��
        playerModelObject.transform.localPosition = initPlayerPosition;
        playerModelObject.transform.localEulerAngles = initPlayerAngle;

        //�J�������̍X�V
        Camera.main.transform.position = initCameraPosition;
        Camera.main.transform.eulerAngles = initCameraAngle;
        Camera.main.transform.parent = playerModelObject.GetComponentInParent<Player>().LaneObject.transform;

        eventManager.DestroyObject(startStage);

        eventManager.ChangeCanvasActive(true);
        eventManager.StartEventFlag = true;

        return true;
    }

    private void StartFirstEvent()
    {
        if(eventTextDisplay.IsAlpha() == true)
        {
            eventTextDisplay.FirstText();
        }
        float distance = Camera.main.transform.position.z - playerModelObject.transform.position.z;
        if(distance <= 5.0f)
        {
            type = EventProgressType.EndFirstEvent;
        }
    }

    private void UpdateCameraInfo()
    {
        //�J�������v���C���[�̑��ʂ��ڂ��悤�ɂ���
        Camera.main.transform.position = playerModelObject.transform.position + new Vector3(5.0f, 2.5f, 0.0f);
        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.parent = playerModelObject.transform;

        eventTextDisplay.SecondText();
        type = EventProgressType.StartSecondEvent;
    }

    private void StartSecondEvent()
    {
        timer.UpdateTimer();
        if(timer.IsTime() == true)
        {
            type = EventProgressType.EndSecondEvent;
        }
    }
}
