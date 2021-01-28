using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterComponent
{
    [SerializeField]
    private PlayerStatusDataTable dataTable = null;

    public PlayerStatusData status { get; private set; }

    public override void OnCreate()
    {
        //ステータス取得
        status = new PlayerStatusData(dataTable.GetStatus());
    }
}
