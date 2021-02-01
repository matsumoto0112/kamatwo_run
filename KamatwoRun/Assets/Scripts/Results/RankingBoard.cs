using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class RankingBoard : MonoBehaviour
{
    [SerializeField, Header("���[�h����\������e�L�X�g")]
    private Text modeText;
    [SerializeField]
    private List<Text> startPoints;

    private void Start()
    {
        //�Z�[�u���������ăv���C�f�[�^���A�b�v���[�h���Ă���
        //�Z�[�u��ɂ̓f�[�^�����Z�b�g����邽�߁A������Ă�ł��f�[�^���j�󂳂�邱�Ƃ͂Ȃ�
        GameDataStore.Instance.SaveGameData();
        RankingData data = GameDataStore.Instance.GetSavedRankingData();

        Assert.IsTrue(data.playerDatas.Length == startPoints.Count, "���U���g�̗v�f����UI�̗v�f������v���܂���");

        modeText.text = GameDataStore.Instance.PlayedMode.PlayModeText();
        for (int i = 0; i < data.playerDatas.Length; i++)
        {
            startPoints[i].text = $"{ data.playerDatas[i].score}";
        }
    }
}
