using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音量ボリュームクラス
public class SoundVolume 
{
    public float BGM = 0.7f;
    public float Voice = 1.0f;
    public float SE = 0.5f;
    public bool Mute = false;

    public void Init()
    {
        BGM = 0.5f;
        Voice = 1.0f;
        SE = 0.2f;
        Mute = false;
    }
 
}
