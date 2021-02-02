using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField]
    private Text scoreText = null;

    private PlayerStatus playerStatus = null;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score:" + playerStatus.Score;
    }


}
