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
    [SerializeField]
    private List<Text> startPoints;
    [SerializeField]
    private Image holidayBoard;
    [SerializeField]
    private Image weekdayBoard;

    public void Init()
    {
        RankingData data;
        if (type == RankingType.PlayedGameMode)
        {

            data = GameDataStore.Instance.GetSavedRankingData();
        }
        else
        {
            PlayMode mode = type == RankingType.Weekday ? PlayMode.Weekday : PlayMode.Holiday;
            data = GameDataStore.Instance.GetSavedRankingData(mode);
            SetBoardVisibility(mode);
        }

        Assert.IsTrue(data.playerDatas.Length == startPoints.Count, "リザルトの要素数とUIの要素数が一致しません");

        for (int i = 0; i < data.playerDatas.Length; i++)
        {
            startPoints[i].text = $"{ data.playerDatas[i].score}";
        }
    }

    public void HighlightRanking(int ranking)
    {
        ranking = ranking - 1;
        if (ranking < 0 || ranking >= startPoints.Count) return;
        startPoints[ranking].color = Color.red;
    }

    public void SetBoardVisibility(PlayMode playmode)
    {
        switch (playmode)
        {
            case PlayMode.Weekday:
                weekdayBoard.enabled = true;
                holidayBoard.enabled = false;
                break;
            case PlayMode.Holiday:
                weekdayBoard.enabled = false;
                holidayBoard.enabled = true;
                break;
            default:
                break;
        }
    }
}
