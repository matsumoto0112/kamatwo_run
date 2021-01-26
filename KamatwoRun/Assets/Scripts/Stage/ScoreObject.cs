using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 得点オブジェクト
/// </summary>
public class ScoreObject : StageObject
{
    [SerializeField, Tooltip("スコア情報")]
    private ScoreObjectParameter scoreInfo;

    public ScoreObjectParameter ScoreInfo { get { return scoreInfo; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
