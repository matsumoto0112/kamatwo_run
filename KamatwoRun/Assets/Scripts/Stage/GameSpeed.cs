using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    [SerializeField]
    private StageParameter stagePamameter;

    /// <summary>
    /// ゲームのスピード
    /// </summary>
    public float Speed { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Speed = stagePamameter.defaultStageMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
