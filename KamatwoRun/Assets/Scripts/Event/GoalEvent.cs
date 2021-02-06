using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �S�[�����̃C�x���g����
/// </summary>
public class GoalEvent : BaseEvent
{
    private SceneChangeRelay sceneChangeRelay = null;

    private Timer timer;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="playerModelObject"></param>
    public GoalEvent(GameObject playerModelObject, EventManager eventManager)
        : base(playerModelObject,eventManager)
    {
        sceneChangeRelay = eventManager.SceneChangeRelay;

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

        GameDataStore.Instance.Score = playerModelObject.GetComponent<PlayerStatus>().Score;
        GameDataStore.Instance.GameEndedType = GameEndType.Goal;

        //�J�����̒Ǐ]�Ώۂ�ύX
        Camera.main.transform.parent = eventManager.StageObject.transform;
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
