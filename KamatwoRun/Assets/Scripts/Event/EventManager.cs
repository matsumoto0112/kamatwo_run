using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    [SerializeField]
    private GameObject stageManagerObject = null;
    [SerializeField]
    private GameObject playerCanvas = null;
    [SerializeField]
    private GameObject startEventStagePrefab = null;
    [SerializeField]
    private GameObject eventCanvasObject = null;
    [SerializeField]
    private SceneChangeRelay changeRelay = null;

    private StageManager stageManager = null;
    private GameSpeed gameSpeed = null;

    private IEnumerator coroutine = null;
    private GameObject playerModelObject = null;   //プレイヤーモデルオブジェクト
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
        eventTimer = new Timer(1.5f);
        IsCurvePoint = false;

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
        //演出中はステージを破棄させない
        stageManager.stageDeletable = false;
        playerCanvas.SetActive(false);

        EventTextDisplay eventTextDisplay = eventCanvasObject.GetComponent<EventTextDisplay>();
        eventTextDisplay.Initialize();

        //スタート時のプレイヤー情報保存
        Vector3 initialPlayerPosition = playerModelObject.transform.localPosition;
        Vector3 initialPlayerRotate = playerModelObject.transform.localEulerAngles;
        //初期のカメラ情報保存
        Vector3 initialCameraPosition = Camera.main.transform.position;
        Vector3 initialCameraRotate = Camera.main.transform.eulerAngles;

        Vector3 startPosition = new Vector3(0.0f, 0.0f, -50.0f);
        playerModelObject.transform.position = startPosition + new Vector3(0.0f, 0.5f, 0.0f);
        GameObject startStage1 = Instantiate(startEventStagePrefab, startPosition, Quaternion.identity);

        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.position = playerModelObject.transform.position + (playerModelObject.transform.forward * 30) + new Vector3(0.0f, 2.0f, 0.0f);

        float alpha = 0.0f;
        //プレイヤーがカメラに向かって走ってくる
        while (true)
        {
            playerModelObject.transform.position += playerModelObject.transform.forward * Time.deltaTime * 7.5f;
            alpha += Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            eventTextDisplay.SetFrameAlpha(alpha);
            yield return new WaitForSeconds(Time.deltaTime);
            if ((Camera.main.transform.position.z - playerModelObject.transform.position.z) <= 5.0f)
            {
                break;
            }
            if (alpha >= 1.0f)
            {
                eventTextDisplay.FirstText();
            }
        }

        //カメラをプレイヤーの側面を移すようにする
        Camera.main.transform.position = playerModelObject.transform.position + new Vector3(5.0f, 2.5f, 0.0f);
        Camera.main.transform.LookAt(playerModelObject.transform);
        Camera.main.transform.parent = playerModelObject.transform;

        Timer timer = new Timer(2.0f);
        eventTextDisplay.SecondText();
        //再度進む
        while (true)
        {
            playerModelObject.transform.position += playerModelObject.transform.forward * Time.deltaTime * 7.5f;
            timer.UpdateTimer();
            yield return new WaitForSeconds(Time.deltaTime);
            if (timer.IsTime() == true)
            {
                break;
            }
        }

        //演出終了処理
        eventTextDisplay.Initialize();

        Camera.main.transform.parent = null;
        playerModelObject.transform.localPosition = initialPlayerPosition;
        playerModelObject.transform.localEulerAngles = initialPlayerRotate;


        Camera.main.transform.position = initialCameraPosition;
        Camera.main.transform.eulerAngles = initialCameraRotate;
        Camera.main.transform.parent = playerModelObject.transform.parent.GetComponentInChildren<LanePositions>().transform;

        Destroy(startStage1);

        playerCanvas.SetActive(true);
        playerCanvas.GetComponent<StatusDisplay>().OnEventEndInitialize();

        stageManager.stageDeletable = true;
        StartEventFlag = true;
        coroutine = null;
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

    public void GoalEvent(GameObject target, GameObject stageObject)
    {
        coroutine = GoalCoroutine(target, stageObject);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// カーブ時の演出処理
    /// </summary>
    /// <param name="subStageObject"></param>
    /// <returns></returns>
    private IEnumerator CurveEventCoroutine(GameObject subStageObject)
    {
        //演出中はステージを削除しない
        stageManager.stageDeletable = false;
        playerModelObject.GetComponent<PlayerInput>().OnEventInitialize();
        playerModelObject.GetComponent<PlayerStatus>().OnEventInitialize();

        //カメラの親オブジェクトを保存
        Transform cameraParent = Camera.main.transform.parent;
        Vector3 localPosition = Camera.main.transform.localPosition;
        Vector3 localEulerAngle = Camera.main.transform.localEulerAngles;

        //カメラをサブステージの子オブジェクトにする
        Camera.main.transform.parent = subStageObject.transform;
        Camera.main.transform.position = subStageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;
        GameObject laneObject = playerModelObject.GetComponentInParent<Player>().LaneObject;
        LanePositions lane = laneObject.GetComponent<LanePositions>();
        while (true)
        {
            Camera.main.transform.LookAt(playerModelObject.transform);
            //カメラをプレイヤーに向ける
            if (IsCurvePoint == true)
            {
                gameSpeed.Speed -= Time.deltaTime * curveCoef;
                gameSpeed.Speed = Mathf.Clamp(gameSpeed.Speed, 0.0f, gameSpeed.DefaultStageMoveSpeed);
                //進行方向が変化したら
                if (lane.IsChangeDirection() == false)
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

    private IEnumerator GoalCoroutine(GameObject target, GameObject stageObject)
    {
        stageManager.stageDeletable = false;
        playerModelObject.GetComponent<PlayerInput>().OnEventInitialize();
        playerModelObject.GetComponent<PlayerStatus>().OnEventInitialize();

        //カメラ追跡停止
        Camera.main.transform.parent = stageObject.transform;
        Timer timer = new Timer(1.5f);
        while (true)
        {
            Camera.main.transform.LookAt(target.transform);
            timer.UpdateTimer();
            yield return new WaitForSeconds(Time.deltaTime);
            if (timer.IsTime() == true)
            {
                break;
            }
        }

        changeRelay.Next();
        coroutine = null;
    }
}
