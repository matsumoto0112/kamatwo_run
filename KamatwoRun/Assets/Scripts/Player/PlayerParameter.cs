using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのパラメータ管理クラス
/// </summary>
public class PlayerParameter : CharacterComponent
{
    [SerializeField]
    private PlayerParameterDataTable dataTable = null;

    public PlayerParameterData parameter { get; private set; }

    public override void OnCreate()
    {
        //ステータス取得
        parameter = new PlayerParameterData(dataTable.GetParameter());
    }
}
