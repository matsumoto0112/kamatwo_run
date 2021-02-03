using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    [SerializeField]
    private GameObject stageManagerObject = null;
    private StageManager stageManager = null;
    private GameSpeed gameSpeed = null;

    private IEnumerator coroutine = null;
    private GameObject playerModelObject = null;
    private Timer eventTimer;

    private const float curveCoef = 26.5f;

    //
    public bool StartEventFlag { get; private set; }

    //ゲーム中イベントフラグ
    public bool EventFlag
    {
        get
        {
            return coroutine != null;
        }
    }

    /// <summary>
    /// カーブポイントに差し掛かったかどうか
    /// </summary>
    public bool IsCurvePoint { get; set; }

    protected override void Awake()
    {
        coroutine = null;
        base.Awake();
        stageManager = stageManagerObject.GetComponent<StageManager>();
        gameSpeed = stageManagerObject.GetComponent<GameSpeed>();
        //プレイヤーモデル取得
        playerModelObject = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        eventTimer = new Timer(2.0f);
        IsCurvePoint = false;
    }

    private void Start()
    {
        StartEventFlag = false;
        coroutine = GameStartEvent();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// ゲームスタート時のイベント処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStartEvent()
    {
        yield return new WaitForSeconds(Time.deltaTime);



        StartEventFlag = true;
    }

    /// <summary>
    /// カーブイベント
    /// </summary>
    /// <param name="subStageObject"></param>
    public void CurveEvent(GameObject subStageObject)
    {
        coroutine = CurveEventCoroutine(subStageObject);
        StartCoroutine(coroutine);
    }

    private IEnumerator CurveEventCoroutine(GameObject subStageObject)
    {
        //演出中はステージを削除しない
        stageManager.stageDeletable = false;
        playerModelObject.GetComponent<PlayerInput>().OnEventInitialize();

        //カメラの親オブジェクトを保存
        Transform cameraParent = Camera.main.transform.parent;
        Vector3 localPosition = Camera.main.transform.localPosition;
        Vector3 localEulerAngle = Camera.main.transform.localEulerAngles;

        //カメラをサブステージの子オブジェクトにする
        Camera.main.transform.parent = subStageObject.transform;
        Camera.main.transform.position = subStageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;

        while (true)
        {
            //カメラをプレイヤーに向ける
            Camera.main.transform.LookAt(playerModelObject.transform);
            if (IsCurvePoint == true)
            {
                gameSpeed.Speed -= Time.deltaTime * curveCoef;
                gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 0.0f, gameSpeed.DefaultStageMoveSpeed);
                //減速させて曲がったら元の速度に戻す
                if (gameSpeed.Speed <= 0.0f)
                {
                    IsCurvePoint = false;
                    gameSpeed.Initialize();
                }
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }

            eventTimer.UpdateTimer();
            if (eventTimer.IsTime() == true)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //カメラを元の位置に
        Camera.main.transform.parent = cameraParent;
        Camera.main.transform.localPosition = localPosition;
        Camera.main.transform.localEulerAngles = localEulerAngle;

        coroutine = null;
        eventTimer.Initialize();
        stageManager.stageDeletable = true;
    }
}
