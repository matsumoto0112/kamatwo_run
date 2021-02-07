using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvent : BaseEvent
{
    private SceneChangeRelay sceneChangeRelay = null;
    private EventCanvas eventCanvas = null;
    private GameSpeed gameSpeed = null;

    private Timer eventEndTimer;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerModelObject"></param>
    public GameOverEvent(GameObject playerModelObject, EventManager eventManager)
        : base(playerModelObject, eventManager)
    {
        eventCanvas = eventManager.GetEventCanvas();
        gameSpeed = eventManager.GameSpeed;
        sceneChangeRelay = eventManager.SceneChangeRelay;
        eventEndTimer = new Timer(5.0f);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        eventCanvas.GameOverEventInitialize();
        gameSpeed.Speed = 0.0f;
        eventEndTimer.Initialize();
        GameDataStore.Instance.Score = playerModelObject.GetComponent<PlayerStatus>().Score;
        GameDataStore.Instance.GameEndedType = GameEndType.GameOver;

        if(playerModelObject.GetComponentToNullCheck(out PlayerInput playerInput)  == true)
        {
            //ジャンプ中に死亡したら
            if(playerInput.CommandType == CommandType.JUMP)
            {
                Vector3 position = playerModelObject.transform.localPosition;
                position.y = 0.5f;
                playerModelObject.transform.localPosition = position;
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        eventEndTimer.UpdateTimer();

        if (eventCanvas.UpdateGameOverImage() == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsEnd = true;
            }
        }
        if (eventEndTimer.IsTime() == true)
        {
            IsEnd = true;
        }
    }

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
