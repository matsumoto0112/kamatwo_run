using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadEvents
{
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        Application.targetFrameRate = 60;
    }
}
