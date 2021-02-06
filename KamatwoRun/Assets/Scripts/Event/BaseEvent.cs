using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントのベースクラス
/// </summary>
public class BaseEvent : IEvent
{
    protected EventManager eventManager = null;
    protected GameObject playerModelObject = null;
    public bool IsEnd { get; protected set; } = false;

    public BaseEvent(GameObject playerModelObject,EventManager eventManager)
    {
        this.playerModelObject = playerModelObject;
        this.eventManager = eventManager;
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
