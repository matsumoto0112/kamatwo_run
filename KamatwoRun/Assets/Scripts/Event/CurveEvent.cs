using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カーブ時のイベント
/// </summary>
public class CurveEvent : BaseEvent
{
    private PlayerMove playerMove = null;
    private LanePositions lanePositions = null;

    private GameSpeed gameSpeed = null;

    private Transform cameraParent = null;
    private Vector3 initCameraPosition = Vector3.zero;
    private Vector3 initCameraAngle = Vector3.zero;
    private Vector3 initPlayerPosition = Vector3.zero;

    private Timer middleCheckTimer;

    private  float curveCoef = 0.0f;

    /// <summary>
    ///コンストラクタ
    /// </summary>
    /// <param name="playerModelObject"></param>
    public CurveEvent(GameObject playerModelObject, EventManager eventManager)
        : base(playerModelObject, eventManager)
    {
        playerMove = playerModelObject.GetComponent<PlayerMove>();
        GameObject laneObject = this.playerModelObject.GetComponentInParent<Player>().LaneObject;
        lanePositions = laneObject.GetComponent<LanePositions>();
        gameSpeed = eventManager.GameSpeed;
        middleCheckTimer = new Timer(0.5f);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        middleCheckTimer.Initialize();

        //カメラの情報保存
        cameraParent = Camera.main.transform.parent;
        initCameraPosition = Camera.main.transform.localPosition;
        initCameraAngle = Camera.main.transform.localEulerAngles;

        //カメラ情報の更新
        Camera.main.transform.parent = eventManager.StageObject.transform;
        Camera.main.transform.position = eventManager.StageObject.GetComponent<CurveCameraEvent>().EventCameraPosition + Vector3.up;

        initPlayerPosition = playerModelObject.transform.position;
        curveCoef = gameSpeed.Speed;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //カメラをプレイヤーに向ける
        Camera.main.transform.LookAt(playerModelObject.transform);

        //チェックポイントに到達しておらず、進行方向が変化していないなら
        if((eventManager.IsCurvePoint == false && 
            lanePositions.IsChangeDirection() == true) || 
            middleCheckTimer.IsTime() == false)
        {
            middleCheckTimer.UpdateTimer();
            playerMove.OffsetPosition(initPlayerPosition, middleCheckTimer.CurrentTime / middleCheckTimer.LimitTime);
            if(middleCheckTimer.IsTime() == true)
            {
                playerModelObject.transform.position = playerMove.NextMovePosition();
            }
        }

        //体を傾けるポイントに到達したら
        if (eventManager.IsCurvePoint == true)
        {
            gameSpeed.Speed -= Time.deltaTime * curveCoef;
            gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 8.0f, gameSpeed.DefaultStageMoveSpeed);
            //進行方向が変化したら
            if (lanePositions.IsChangeDirection() == false)
            {
                eventManager.IsCurvePoint = false;
                eventManager.StageManager.NormalizeGameSpeed();
            }
            return;
        }

        if(Camera.main.transform.localEulerAngles.x  >= 85.0f)
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
        if (IsEnd == false)
        {
            return false;
        }

        //カメラの情報を元に戻す
        Camera.main.transform.parent = cameraParent;
        Camera.main.transform.localPosition = initCameraPosition;
        Camera.main.transform.localEulerAngles = initCameraAngle;

        return true;
    }
}
