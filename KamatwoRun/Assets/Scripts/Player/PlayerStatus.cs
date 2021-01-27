using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour,ICharacterComponent
{
    [SerializeField]
    private PlayerStatusDataTable dataTable = null;

    public Transform Parent => transform.parent;
    public PlayerStatusData status { get; private set; }

    public void OnCreate()
    {
        //ステータス取得
        status = new PlayerStatusData(dataTable.GetStatus());
    }

    public void OnEnd()
    {
    }

    public void OnUpdate()
    {
    }
}
