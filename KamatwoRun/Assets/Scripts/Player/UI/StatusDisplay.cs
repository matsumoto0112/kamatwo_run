using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField]
    private HPImageDisplay hpDisplay = null;
    [SerializeField]
    private Text scoreText = null;

    private PlayerStatus playerStatus = null;

    public void OnEventEndInitialize()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStatus>();
        hpDisplay.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventManager.Instance.StartEventFlag == false)
        {
            return;
        }
        scoreText.text = "Score:" + playerStatus.Score;
        hpDisplay.OnUpdate();
    }
}
