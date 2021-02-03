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
    public float Speed = 1.0f;
    public float DefaultStageMoveSpeed
    {
        get
        {
            return stagePamameter.defaultStageMoveSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Speed = stagePamameter.defaultStageMoveSpeed;
    }
}
