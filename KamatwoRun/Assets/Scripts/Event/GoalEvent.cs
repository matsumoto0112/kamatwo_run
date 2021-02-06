using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゴール時のイベント処理
/// </summary>
public class GoalEvent : BaseEvent
{
    private SceneChangeRelay sceneChangeRelay = null;

    private Timer timer;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerModelObject"></param>
    public GoalEvent(GameObject playerModelObject, EventManager eventManager)
        : base(playerModelObject,eventManager)
    {
        sceneChangeRelay = eventManager.SceneChangeRelay;

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

        GameDataStore.Instance.Score = playerModelObject.GetComponent<PlayerStatus>().Score;
        GameDataStore.Instance.GameEndedType = GameEndType.Goal;

        //カメラの追従対象を変更
        Camera.main.transform.parent = eventManager.StageObject.transform;
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
