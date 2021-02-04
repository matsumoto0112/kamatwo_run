using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib.Attribute;

public class SceneChangeRelay : MonoBehaviour
{
    [SerializeField, SceneName]
    private string sceneName = "";
    [SerializeField]
    private Fade fade = null;

    public void Next()
    {
        fade.fadeOutStart(Color.black, sceneName);
    }
}
