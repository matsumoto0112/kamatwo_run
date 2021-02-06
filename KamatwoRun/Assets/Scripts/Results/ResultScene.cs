using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    [SerializeField]
    private Image goalImage;
    [SerializeField]
    private Image gameOverImage;
    [SerializeField]
    private RankingBoard rankingBoard;

    [SerializeField]
    private Text comment;

    [SerializeField, TextArea()]
    private string[] comments;

    private void Start()
    {
        //セーブ処理をしてプレイデータをアップロードしておく
        //セーブ後にはデータがリセットされるため、複数回呼んでもデータが破壊されることはない
        GameDataStore.Instance.SaveGameData();

        int score = GameDataStore.Instance.Score;
        var datas = GameDataStore.Instance.GetSavedRankingData().playerDatas;
        PlayMode playedMode = GameDataStore.Instance.PlayedMode;
        GameEndType gameEndType = GameDataStore.Instance.GameEndedType;

        //ランキングの取得
        int ranking = 0;
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].score == score)
            {
                ranking = i + 1;
                break;
            }
        }

        //ゲーム結果表示

        //休日の時はリザルト表示なし
        if (playedMode == PlayMode.Holiday)
        {
            goalImage.enabled = false;
            gameOverImage.enabled = false;
        }
        else
        {
            if (gameEndType == GameEndType.Goal)
            {
                goalImage.enabled = true;
                gameOverImage.enabled = false;
            }
            else
            {
                goalImage.enabled = false;
                gameOverImage.enabled = true;
            }
        }

        //得点表示
        rankingBoard.Init();
        rankingBoard.HighlightRanking(ranking);
        rankingBoard.SetBoardVisibility(playedMode);

        //感想表示
        if (playedMode == PlayMode.Weekday)
        {
            //平日
            if (gameEndType == GameEndType.Goal)
            {
                comment.text = comments[0];
            }
            else
            {
                comment.text = comments[1];
            }
        }
        else
        {
            if (score < 10000)
            {
                comment.text = comments[2];
            }
            else if (score < 20000)
            {
                comment.text = comments[3];
            }
            else
            {
                comment.text = comments[4];
            }
        }

        //もうこのプレイのデータは必要ないのでリセットする
        GameDataStore.Instance.ResetPlayDatas();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
