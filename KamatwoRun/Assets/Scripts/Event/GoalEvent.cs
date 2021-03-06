using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゴール時のイベント処理
/// </summary>
public class GoalEvent : BaseEvent
{
    private SceneChangeRelay sceneChangeRelay = null;
    private EventCanvas eventCanvas = null;

    private Timer eventEndTimer;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerModelObject"></param>
    public GoalEvent(GameObject playerModelObject, EventManager eventManager)
        : base(playerModelObject,eventManager)
    {
        sceneChangeRelay = eventManager.SceneChangeRelay;
        eventCanvas = eventManager.GetEventCanvas();
        eventEndTimer = new Timer(5.0f);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        eventEndTimer.Initialize();

        GameDataStore.Instance.Score = playerModelObject.GetComponent<PlayerStatus>().Score;
        GameDataStore.Instance.GameEndedType = GameEndType.Goal;

        //カメラの追従対象を変更
        Camera.main.transform.parent = eventManager.StageObject.transform;
        eventCanvas.GoalEventInitialize();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //プレイヤーの方を向く
        Camera.main.transform.LookAt(playerModelObject.transform);
        eventEndTimer.UpdateTimer();

        if(eventCanvas.UpdateGoalImage() == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsEnd = true;
            }
        }
        if(eventEndTimer.IsTime() == true)
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
