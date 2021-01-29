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
    /// ”­Ë‚·‚é’e‚Ì¶¬
    /// </summary>
    public void SpawnDumplingSkin()
    {
        dumplingSkin.OnCreate();
    }

    /// <summary>
    /// Õ“Ë”»’è
    /// </summary>
    /// <returns></returns>
    public bool IsHit()
    {
        return dumplingSkin.IsHit;
    }

    public bool IsShot()
    {
        return dumplingSkin.IsShot;
    }

    public void ShotEnd()
    {
        dumplingSkin.OnEnd();
    }
}
