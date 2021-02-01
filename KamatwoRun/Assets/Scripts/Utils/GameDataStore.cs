using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���̃Q�[�����
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
/// �����L���O�Ƃ��Ď擾�ł���v���C���[�̃v���C���
/// </summary>
[System.Serializable]
public class RankingData
{
    /// <summary>
    /// �ۑ��\�ȃv���C���[��
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
/// �Q�[���v���C�f�[�^�̃f�[�^�X�g�A
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
    /// ����̃v���C���̃X�R�A
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// ����̃v���C�������[�h
    /// </summary>
    public PlayMode PlayedMode { get; set; }

    //�Z�[�u����t�@�C����
    private static readonly Dictionary<PlayMode, string> kSaveFileName = new Dictionary<PlayMode, string>() { { PlayMode.Weekday, "save1.bin" }, { PlayMode.Holiday, "save2.bin" }, };

    /// <summary>
    /// ����̃v���C�����Z�[�u����
    /// </summary>
    public void SaveGameData()
    {
        //�O�̃v���C�i�K�̏����擾����
        RankingData prevRanking = BinarySaveSystem.Load<RankingData>(kSaveFileName[PlayedMode]);

        //����̃v���C�̃X�R�A��ǉ�����
        var list = prevRanking.playerDatas.ToList();
        list.Add(new GameData(Score));

        //�ォ�����̐l���ɂȂ�悤�Ƀ��X�g�����
        //�~���Ƀ\�[�g���A �Ō�̗v�f���폜���邱�ƂŎ�������
        list.Sort((a, b) => b.score - a.score);
        list.RemoveAt(list.Count);

        //�V�����Ȃ��������L���O�f�[�^��ۑ�����
        RankingData currentRanking = new RankingData(list.ToArray());
        BinarySaveSystem.Save(currentRanking, kSaveFileName[PlayedMode]);
    }

    /// <summary>
    /// �Z�[�u����Ă��郉���L���O�����擾����
    /// </summary>
    /// <returns></returns>
    public RankingData GetSavedRankingData()
    {
        RankingData res = BinarySaveSystem.Load<RankingData>(kSaveFileName[PlayedMode]);
        return res;
    }

    /// <summary>
    /// mode�ɑΉ������Z�[�u����Ă��郉���L���O�����擾����
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public RankingData GetSavedRankingData(PlayMode mode)
    {
        RankingData res = BinarySaveSystem.Load<RankingData>(kSaveFileName[mode]);
        return res;
    }
}
