using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �S�[�����̃C�x���g����
/// </summary>
public class GoalEvent : BaseEvent
{
    private PlayerInput playerInput = null;
    private PlayerStatus playerStatus = null;

    private SceneChangeRelay sceneChangeRelay = null;

    private Timer timer;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="playerModelObject"></param>
    public GoalEvent(GameObject playerModelObject)
        : base(playerModelObject)
    {
        playerInput = this.playerModelObject.GetComponent<PlayerInput>();
        playerStatus = this.playerModelObject.GetComponent<PlayerStatus>();

        sceneChangeRelay = EventManager.Instance.SceneChangeRelay;

        timer = new Timer(1.5f);
    }

    /// <summary>
    /// ����������
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        timer.Initialize();

        //�v���C���[�̏�Ԃ�������
        playerInput.OnEventInitialize();
        playerStatus.OnEventInitialize();

        //�J�����̒Ǐ]�Ώۂ�ύX
        Camera.main.transform.parent = EventManager.Instance.StageObject.transform;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //�v���C���[�̕�������
        Camera.main.transform.LookAt(playerModelObject.transform);
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

        sceneChangeRelay.Next();

        return true;
    }
}
