using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private EventManager eventManager = null;
    [SerializeField]
    private SoundManager soundManager = null;

    private List<ICharacterComponent> componentList;
    private PlayerStatus playerStatus = null;

    public GameObject ModelObject { get; private set; } = null;
    public GameObject LaneObject { get; private set; } = null;
    public GameObject DumplingObject { get; private set; } = null;
    public EventManager EventManager => eventManager;
    public SoundManager SoundManager => soundManager;

    // Start is called before the first frame update
    void Start()
    {
        componentList = new List<ICharacterComponent>();
        ICharacterComponent[] array = GetComponentsInChildren<ICharacterComponent>();
        foreach(var c in array)
        {
            componentList.Add(c);
        }

        foreach(var c in componentList)
        {
            c.OnCreate();
        }
        playerStatus = GetComponentInChildren<PlayerStatus>();
        ModelObject = playerStatus.gameObject;
        LaneObject = GetComponentInChildren<LanePositions>().gameObject;
        DumplingObject = GetComponentInChildren<DumplingSkin>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStatus.IsDead() == true)
        {
            return;
        }

        if(eventManager.EventFlag == true)
        {
            return;
        }

        foreach(var c in componentList)
        {
            c.OnUpdate();
        }
    }
}
