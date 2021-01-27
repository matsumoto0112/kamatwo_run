using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<ICharacterComponent> componentList;

    // Start is called before the first frame update
    void Start()
    {
        componentList = new List<ICharacterComponent>();
        ICharacterComponent[] array = GetComponentsInChildren<ICharacterComponent>();
        foreach(var c in array)
        {
            componentList.Add(c);
        }

        foreach(var c in componentList)
        {
            c.OnCreate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var c in componentList)
        {
            c.OnUpdate();
        }
    }
}
