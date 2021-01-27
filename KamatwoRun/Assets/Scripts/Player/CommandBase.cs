using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase : ICommand
{
    protected ICharacterComponent Character = null;

    public CommandBase(ICharacterComponent character)
    {
        this.Character = character;
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
