using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public enum EventType
{
    None,
    GameStart,
    Curve,
    Goal,
}

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
    private GameObject playerModelObject = null;

    private EventType eventType = EventType.None;
    private Dictionary<EventType, BaseEvent> eventList;

    public StageManager StageManager { get; private set; } = null;
    public GameSpeed GameSpeed { get; private set; } = null;
    public GameObject StageObject { get; private set; } = null;
    public SceneChangeRelay SceneChangeRelay { get; private set; } = null;

    [HideInInspector]
    public bool StartEventFlag = false;
    [HideInInspector]
    public bool IsCurvePoint = false;

    //ゲーム中イベントフラグ
    public bool EventFlag => eventType != EventType.None;

    protected override void Awake()
    {
        base.Awake();
        StageManager = stageManagerObject.GetComponent<StageManager>();
        GameSpeed = stageManagerObject.GetComponent<GameSpeed>();
        SceneChangeRelay = GetComponent<SceneChangeRelay>();
        StageObject = null;
        IsCurvePoint = false;

        StartEventFlag = false;
    }

    private void Start()
    {
        eventList = new Dictionary<EventType, BaseEvent>();
        eventList.Add(EventType.GameStart, new GameStartEvent(playerModelObject));
        eventList.Add(EventType.Curve, new CurveEvent(playerModelObject));
        eventList.Add(EventType.Goal, new GoalEvent(playerModelObject));

        eventType = EventType.GameStart;
        eventList[eventType].OnInitialize();
    }

    private void Update()
    {
        if(eventType != EventType.None)
        {
            StageManager.stageDeletable = false;
            eventList[eventType].OnUpdate();
            if(eventList[eventType].OnEnd() == true)
            {
                StageManager.stageDeletable = true;
                eventType = EventType.None;
            }
        }
    }

    /// <summary>
    /// プレイヤーUIの表示変更
    /// </summary>
    /// <param name="active"></param>
    public void ChangeCanvasActive(bool active)
    {
        if (active == true)
        {
            playerCanvas.SetActive(true);
            playerCanvas.GetComponent<StatusDisplay>().OnEventEndInitialize();
        }
        else if (active == false)
        {
            playerCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// カーブイベント
    /// </summary>
    /// <param name="subStageObject"></param>
    public void CurveEvent(GameObject stageObject)
    {
        StageObject = stageObject;
        eventType = EventType.Curve;
        eventList[eventType].OnInitialize();
    }

    public void GoalEvent(GameObject stageObject)
    {
        StageObject = stageObject;
        eventType = EventType.Goal;
        eventList[eventType].OnInitialize();
    }

    public EventTextDisplay GetEventTextDisplay()
    {
        return eventCanvasObject.GetComponent<EventTextDisplay>();
    }

    public GameObject SpawnStartStage()
    {
        Vector3 startPosition = new Vector3(0.0f, 0.0f, -50.0f);
        return Instantiate(startEventStagePrefab, startPosition, Quaternion.identity);
    }

    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
