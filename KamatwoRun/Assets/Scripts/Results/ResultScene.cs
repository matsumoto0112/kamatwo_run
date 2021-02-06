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
        //�Z�[�u���������ăv���C�f�[�^���A�b�v���[�h���Ă���
        //�Z�[�u��ɂ̓f�[�^�����Z�b�g����邽�߁A������Ă�ł��f�[�^���j�󂳂�邱�Ƃ͂Ȃ�
        GameDataStore.Instance.SaveGameData();

        int score = GameDataStore.Instance.Score;
        var datas = GameDataStore.Instance.GetSavedRankingData().playerDatas;
        PlayMode playedMode = GameDataStore.Instance.PlayedMode;
        GameEndType gameEndType = GameDataStore.Instance.GameEndedType;

        //�����L���O�̎擾
        int ranking = 0;
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].score == score)
            {
                ranking = i + 1;
                break;
            }
        }

        //�Q�[�����ʕ\��

        //�x���̎��̓��U���g�\���Ȃ�
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

        //���_�\��
        rankingBoard.Init();
        rankingBoard.HighlightRanking(ranking);
        rankingBoard.SetBoardVisibility(playedMode);

        //���z�\��
        if (playedMode == PlayMode.Weekday)
        {
            //����
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

        //�������̃v���C�̃f�[�^�͕K�v�Ȃ��̂Ń��Z�b�g����
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
