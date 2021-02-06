using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントのベースクラス
/// </summary>
public class BaseEvent : IEvent
{
    protected GameObject playerModelObject = null;
    public bool IsEnd { get; protected set; } = false;

    public BaseEvent(GameObject playerModelObject)
    {
        this.playerModelObject = playerModelObject;
        IsEnd = false;
    }

    public virtual void OnInitialize()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual bool OnEnd()
    {
        return IsEnd;
    }
}
