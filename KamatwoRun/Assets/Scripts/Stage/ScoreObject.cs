using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���_�I�u�W�F�N�g
/// </summary>
public class ScoreObject : StageObject
{
    [SerializeField, Tooltip("�X�R�A���")]
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
