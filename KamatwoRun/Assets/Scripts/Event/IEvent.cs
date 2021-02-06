using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    /// <summary>
    /// イベント開始時初期化処理
    /// </summary>
    void OnInitialize();

    /// <summary>
    /// イベント更新処理
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// イベント終了処理
    /// </summary>
    bool OnEnd();
}
