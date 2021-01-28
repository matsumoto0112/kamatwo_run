using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterComponent
{
    /// <summary>
    /// 初期化処理
    /// </summary>
    void OnCreate();

    /// <summary>
    /// 更新処理
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// 終了処理
    /// </summary>
    void OnEnd();

    /// <summary>
    /// 親オブジェクト
    /// </summary>
    Transform Parent { get; }

    /// <summary>
    /// モデルのオブジェクト
    /// </summary>
    Transform CharacterTransform { get; }
}
