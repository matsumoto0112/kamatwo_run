using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サブステージの進行方向を調べるコンポーネント
/// </summary>
public abstract class DirectionChecker : MonoBehaviour
{
    protected GatewayType entrance;
    protected GatewayType exit;

    /// <summary>
    /// サブステージの入口、出口を設定する
    /// </summary>
    /// <param name="entrance"></param>
    /// <param name="exit"></param>
    public void Init(GatewayType entrance, GatewayType exit)
    {
        this.entrance = entrance;
        this.exit = exit;
    }

    /// <summary>
    /// 指定座標から進行方向を取得する
    /// </summary>
    /// <param name="checkPosition"></param>
    /// <returns></returns>
    public abstract Vector3 Directon(Vector3 checkPosition);
}
