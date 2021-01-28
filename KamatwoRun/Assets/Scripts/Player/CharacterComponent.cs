using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour,ICharacterComponent
{
    public Transform Parent => transform.parent;

    public Transform CharacterTransform => transform;

    public virtual void OnCreate()
    {

    }

    public virtual void OnEnd()
    {

    }

    public virtual void OnUpdate()
    {

    }
}
