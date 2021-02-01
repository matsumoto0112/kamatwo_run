using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 一回のゲーム情報
/// </summary>
[System.Serializable]
public struct GameData
{
    public int score;

    public GameData(int score)
    {
        this.score = score;
    }
}

/// <summary>
/// ランキングとして取得できるプレイヤーのプレイ情報
/// </summary>
[System.Serializable]
public class RankingData
{
    /// <summary>
    /// 保存可能なプレイヤー数
    /// </summary>
    public const int kSavedPlayerCount = 5;
    public GameData[] playerDatas;

    public RankingData()
    {
        this.playerDatas = new GameData[kSavedPlayerCount];
    }

    public RankingData(GameData[] datas)
    {
        this.playerDatas = datas;
    }
}

/// <summary>
/// ゲームプレイデータのデータストア
/// </summary>
public class GameDataStore
{
    public static GameDataStore Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameDataStore();
            }

            return _instance;
        }
    }

    private static GameDataStore _instance = null;


    /// <summary>
    /// 今回のプレイ時のスコア
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// 今回のプレイしたモード
    /// </summary>
    public PlayMode PlayedMode { get; set; }

    //セーブするファイル名
    private static readonly Dictionary<PlayMode, string> kSaveFileName = new Dictionary<PlayMode, string>() { { PlayMode.Weekday, "save1.bin" }, { PlayMode.Holiday, "save2.bin" }, };

    /// <summary>
    /// 今回のプレイ情報をセーブする
    /// </summary>
    public void SaveGameData()
    {
        //前のプレイ段階の情報を取得する
        RankingData prevRanking = BinarySaveSystem.Load<RankingData>(kSaveFileName[PlayedMode]);

        //今回のプレイのスコアを追加する
        var list = prevRanking.playerDatas.ToList();
        list.Add(new GameData(Score));

        //上から既定の人数になるようにリストを作る
        //降順にソートし、 最後の要素を削除することで実現する
        list.Sort((a, b) => b.score - a.score);
        list.RemoveAt(list.Count);

        //新しくなったランキングデータを保存する
        RankingData currentRanking = new RankingData(list.ToArray());
        BinarySaveSystem.Save(currentRanking, kSaveFileName[PlayedMode]);
    }

    /// <summary>
    /// セーブされているランキング情報を取得する
    /// </summary>
    /// <returns></returns>
    public RankingData GetSavedRankingData()
    {
        RankingData res = BinarySaveSystem.Load<RankingData>(kSaveFileName[PlayedMode]);
        return res;
    }

    /// <summary>
    /// modeに対応したセーブされているランキング情報を取得する
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public RankingData GetSavedRankingData(PlayMode mode)
    {
        RankingData res = BinarySaveSystem.Load<RankingData>(kSaveFileName[mode]);
        return res;
    }
}
