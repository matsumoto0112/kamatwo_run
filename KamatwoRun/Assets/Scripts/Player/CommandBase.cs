using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase : ICommand
{
    protected ICharacterComponent Character = null;
    protected bool isEnd = false;

    public CommandBase(ICharacterComponent character)
    {
        this.Character = character;
        isEnd = false;
    }

    public virtual void EventInitialize()
    {
        
    }

    public virtual void Execution()
    {

    }

    public virtual void Initialize()
    {

    }

    public virtual bool IsEnd()
    {
        return true;
    }
}
