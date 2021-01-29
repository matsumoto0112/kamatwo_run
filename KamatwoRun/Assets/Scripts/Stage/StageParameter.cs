using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ウェーブに関する情報
/// </summary>
[System.Serializable]
public struct WaveInfo
{
    public int scoreThreshold;
    public SpawnObjectsParameter spawnParameter;
}

[CreateAssetMenu(menuName = "Parameters/Stage")]
public class StageParameter : ScriptableObject
{
    [Header("ウェーブ関係の情報", order = 0)]
    [Header("リスト内の要素は上から順に", order = 1)]
    [Header("スコアの閾値", order = 2)]
    [Header("閾値以下の時に採用するスポーン情報", order = 3)]
    public List<WaveInfo> thresholds;

    [Header("ウェーブが変わる単位"), Tooltip("上の閾値リストがすべて処理し終わった後に、\nどの単位でウェーブが変わるか")]
    public WaveInfo step;

    [Space()]
    //ゲーム内のレーン数
    public int laneNum = 3;

    //ステージ幅
    public float stageWidth = 6.0f;

    //デフォルトでの移動速度
    public float defaultStageMoveSpeed = 15.0f;

    //地面の高さ
    public float groundPosition_Y = 1.0f;

    //空中の高さ
    public float skyPosition_Y = 10.0f;

    //横に長いオブジェクトの半径
    public float wideObjectJudgeRadius = 3.0f;

    //高さのあるオブジェクト同士が隣接しないようにする判定距離
    public float highObjectJudgeDistance = 3.0f;

    /// <summary>
    /// スコアに対するウェーブ段階を取得する
    /// </summary>
    /// <param name="currentScore"></param>
    /// <returns></returns>
    public int GetPhaseByScore(int score)
    {
        int res = 0;
        //各段階を調べ、スコアがそれを超えていたら上書きしていく
        for (int i = 0; i < thresholds.Count; i++)
        {
            if (score >= thresholds[i].scoreThreshold)
            {
                res = i + 1;
            }
        }

        //上限まで達していたらそれ以降は一定間隔で上がるため、それを調べる
        if (res == thresholds.Count)
        {
            score -= thresholds.Last().scoreThreshold;
            res += score / step.scoreThreshold;
        }

        return res;
    }

    /// <summary>
    /// 現在の段階に応じたスポーン情報を取得する
    /// </summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    public SpawnObjectsParameter GetSpawnObjectsParameterByPhase(int phase)
    {
        //閾値リスト内の時
        if (phase < thresholds.Count)
        {
            return thresholds[phase].spawnParameter;
        }
        else
        {
            return step.spawnParameter;
        }
    }
}
