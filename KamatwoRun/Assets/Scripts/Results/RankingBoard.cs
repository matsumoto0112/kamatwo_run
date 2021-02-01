using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// ランキングの種類
/// </summary>
public enum RankingType
{
    Weekday,
    Holiday,
    PlayedGameMode,
}

public class RankingBoard : MonoBehaviour
{
    [SerializeField, Header("ランキングの種類")]
    private RankingType type;
    [SerializeField, Header("モード名を表示するテキスト")]
    private Text modeText;
    [SerializeField]
    private List<Text> startPoints;

    private void Start()
    {
        //セーブ処理をしてプレイデータをアップロードしておく
        //セーブ後にはデータがリセットされるため、複数回呼んでもデータが破壊されることはない
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

        Assert.IsTrue(data.playerDatas.Length == startPoints.Count, "リザルトの要素数とUIの要素数が一致しません");

        for (int i = 0; i < data.playerDatas.Length; i++)
        {
            startPoints[i].text = $"{ data.playerDatas[i].score}";
        }
    }
}
