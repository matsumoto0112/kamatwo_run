using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static bool GetComponentToNullCheck<T>(this GameObject gameObject, out T component) where T : MonoBehaviour
    {
        component = gameObject.GetComponent<T>();
        if (component == null)
        {
            return false;
        }
        return true;
    }
}
