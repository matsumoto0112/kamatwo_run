using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField]
    private HPImageDisplay hpDisplay = null;
    [SerializeField]
    private LoadMapController loadMapController = null;
    [SerializeField]
    private EventManager eventManager = null;
    [SerializeField]
    private Text scoreText = null;

    private PlayerStatus playerStatus = null;

    public void OnEventEndInitialize()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStatus>();
        hpDisplay.Initialize();
        loadMapController.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(eventManager.StartEventFlag == false)
        {
            return;
        }
        scoreText.text = "“¾“_:" + playerStatus.Score;
        hpDisplay.OnUpdate();
        loadMapController.OnUpdate();
    }
}
