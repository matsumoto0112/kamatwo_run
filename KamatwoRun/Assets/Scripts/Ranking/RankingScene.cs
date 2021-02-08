using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingScene : MonoBehaviour
{
    [SerializeField]
    private RankingBoard weekdayBoard;
    [SerializeField]
    private RankingBoard holidayBoard;

    [SerializeField]
    private TitleScene dispatcher;

    // Start is called before the first frame update
    void Start()
    {
        weekdayBoard.Init();
        holidayBoard.Init();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dispatcher.SwapMode();
        }
    }
}
