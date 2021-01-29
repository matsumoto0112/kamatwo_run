using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包まれた時の得点等情報を返す
/// </summary>
public struct WrappedPoint
{
    //獲得スコア
    public int score { get; private set; }
    //回復量
    public float recover { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="score"></param>
    public WrappedPoint(int score, float recover)
    {
        this.score = score;
        this.recover = recover;
    }
}

/// <summary>
/// 皮に包まれることが可能なオブジェクト
/// </summary>
public abstract class WrappableObject : StageObject
{
    /// <summary>
    /// 包まれるときの処理
    /// </summary>
    /// <returns>包まれた後の情報を返す</returns>
    public virtual WrappedPoint Wrap()
    {
        return new WrappedPoint(0, 0.0f);
    }
}
