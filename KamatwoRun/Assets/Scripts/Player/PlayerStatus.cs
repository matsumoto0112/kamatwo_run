using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterComponent
{
    public int score { get; private set; }

    public override void OnCreate()
    {
        base.OnCreate();
        score = 0;
    }

    /// <summary>
    /// ÉXÉRÉAâ¡éZ
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.score += score;
    }
}
