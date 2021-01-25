using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包まれた時の得点等情報を返す
/// </summary>
public struct WrappedPoint
{
    //獲得スコア
    int score;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="score"></param>
    public WrappedPoint(int score)
    {
        this.score = score;
    }
}

/// <summary>
/// 皮に包まれることが可能なオブジェクト
/// </summary>
public class WrappableObject : StageObject
{
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 包まれるときの処理
    /// </summary>
    /// <returns>包まれた後の情報を返す</returns>
    public virtual WrappedPoint Wrap()
    {
        return new WrappedPoint(0);
    }
}
