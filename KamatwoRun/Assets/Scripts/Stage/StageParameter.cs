using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameters/Stage")]
public class StageParameter : ScriptableObject
{
    //ゲーム内のレーン数
    public int laneNum = 3;

    //ステージ幅
    public float stageWidth = 6.0f;

    //デフォルトでの移動速度
    public float defaultStageMoveSpeed = 15.0f;
}
