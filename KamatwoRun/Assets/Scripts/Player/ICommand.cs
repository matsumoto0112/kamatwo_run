using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Initialize();

    /// <summary>
    /// 実行
    /// </summary>
    void Execution();

    /// <summary>
    /// 終了検知
    /// </summary>
    /// <returns></returns>
    bool IsEnd();
}
