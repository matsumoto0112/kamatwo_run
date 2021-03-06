using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : CharacterComponent
{
    private DumplingSkin dumplingSkin = null;
    public Vector3 DumplingSkinPosition => dumplingSkin.transform.position;

    public override void OnCreate()
    {
        base.OnCreate();
        dumplingSkin = Parent.GetComponentInChildren<DumplingSkin>();
    }

    /// <summary>
    /// 発射する弾の生成
    /// </summary>
    public void SpawnDumplingSkin()
    {
        dumplingSkin.OnCreate();
    }

    public bool IsShot()
    {
        return dumplingSkin.IsShot;
    }
}
