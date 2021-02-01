using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// �����L���O�̎��
/// </summary>
public enum RankingType
{
    Weekday,
    Holiday,
    PlayedGameMode,
}

public class RankingBoard : MonoBehaviour
{
    [SerializeField, Header("�����L���O�̎��")]
    private RankingType type;
    [SerializeField, Header("���[�h����\������e�L�X�g")]
    private Text modeText;
    [SerializeField]
    private List<Text> startPoints;

    private void Start()
    {
        //�Z�[�u���������ăv���C�f�[�^���A�b�v���[�h���Ă���
        //�Z�[�u��ɂ̓f�[�^�����Z�b�g����邽�߁A������Ă�ł��f�[�^���j�󂳂�邱�Ƃ͂Ȃ�
        GameDataStore.Instance.SaveGameData();
        RankingData data; if (type == RankingType.PlayedGameMode)
        {

            data = GameDataStore.Instance.GetSavedRankingData();
            modeText.text = GameDataStore.Instance.PlayedMode.PlayModeText();
        }
        else
        {
            PlayMode mode = type == RankingType.Weekday ? PlayMode.Weekday : PlayMode.Holiday;
            data = GameDataStore.Instance.GetSavedRankingData(mode);
            modeText.text = mode.PlayModeText();
        }

        Assert.IsTrue(data.playerDatas.Length == startPoints.Count, "���U���g�̗v�f����UI�̗v�f������v���܂���");

        for (int i = 0; i < data.playerDatas.Length; i++)
        {
            startPoints[i].text = $"{ data.playerDatas[i].score}";
        }
    }
}
