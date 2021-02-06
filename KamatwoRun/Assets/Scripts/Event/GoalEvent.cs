using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゴール時のイベント処理
/// </summary>
public class GoalEvent : BaseEvent
{
    private PlayerInput playerInput = null;
    private PlayerStatus playerStatus = null;

    private SceneChangeRelay sceneChangeRelay = null;

    private Timer timer;

    /// <summary>
    /// コンストラクタ
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
    /// 初期化処理
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        timer.Initialize();

        //プレイヤーの状態を初期化
        playerInput.OnEventInitialize();
        playerStatus.OnEventInitialize();

        //カメラの追従対象を変更
        Camera.main.transform.parent = EventManager.Instance.StageObject.transform;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //プレイヤーの方を向く
        Camera.main.transform.LookAt(playerModelObject.transform);
        timer.UpdateTimer();

        if(timer.IsTime() == true)
        {
            IsEnd = true;
        }
    }

    /// <summary>
    /// 終了処理
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
