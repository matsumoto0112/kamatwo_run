using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 得点情報
/// </summary>
[CreateAssetMenu(menuName = "Parameters/ScoreObject")]
public class ScoreObjectParameter : ScriptableObject
{
    //得点
    public int score;
}