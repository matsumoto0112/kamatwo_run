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
    /// コンストラクタ
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
    /// 初期化処理
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        timer.Initialize();
        type = EventProgressType.StartFirstEvent;
        eventTextDisplay.Initialize();

        eventManager.ChangeCanvasActive(false);

        //プレイヤーの情報保存
        initPlayerPosition = playerModelObject.transform.localPosition;
        initPlayerAngle = playerModelObject.transform.localEulerAngles;

        //カメラの情報保存
        initCameraPosition = Camera.main.transform.position;
        initCameraAngle = Camera.main.transform.eulerAngles;

        //演出時に使用するステージの生成
        startStage = eventManager.SpawnStartStage();
        playerModelObject.transform.position = startStage.transform.position + new Vector3(0.0f, 0.5f, 0.0f);

        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.position = playerModelObject.transform.position + (playerModelObject.transform.forward * 30.0f) + new Vector3(0.0f, 2.0f, 0.0f);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //プレイヤーを前進させる
        playerModelObject.transform.position += playerModelObject.transform.forward * Time.deltaTime * 10.0f;
        eventTextDisplay.ChangeAlpha();

        //イベントの進捗度
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
    /// 終了処理
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
        //プレイヤー更新の情報
        playerModelObject.transform.localPosition = initPlayerPosition;
        playerModelObject.transform.localEulerAngles = initPlayerAngle;

        //カメラ情報の更新
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
        //カメラをプレイヤーの側面を移すようにする
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
