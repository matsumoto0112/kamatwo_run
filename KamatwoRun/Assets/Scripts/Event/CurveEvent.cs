using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カーブ時のイベント
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
    ///コンストラクタ
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
    /// 初期化処理
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        IsEnd = false;
        timer.Initialize();

        //プレイヤーの行動を初期化
        playerInput.OnEventInitialize();
        playerStatus.OnEventInitialize();

        //カメラの情報保存
        cameraParent = Camera.main.transform.parent;
        initCameraPosition = Camera.main.transform.localPosition;
        initCameraAngle = Camera.main.transform.localEulerAngles;

        //カメラ情報の更新
        Camera.main.transform.parent = EventManager.Instance.StageObject.transform;
        Camera.main.transform.position = EventManager.Instance.StageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        //カメラをプレイヤーに向ける
        Camera.main.transform.LookAt(playerModelObject.transform);

        //体を傾けるポイントに到達したら
        if(EventManager.Instance.IsCurvePoint == true)
        {
            gameSpeed.Speed -= Time.deltaTime * CURVE_COEF;
            gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 5.0f, gameSpeed.DefaultStageMoveSpeed);
            //進行方向が変化したら
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
    /// 終了処理
    /// </summary>
    /// <returns></returns>
    public override bool OnEnd()
    {
        if(IsEnd == false)
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
