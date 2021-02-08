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

    public float Speed { get; set; }

    private float ChangedSpeed { get; set; }

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
        Speed = DefaultStageMoveSpeed;
        ChangedSpeed = DefaultStageMoveSpeed;
        Initialize();
    }

    public void Initialize()
    {
        Speed = ChangedSpeed;
    }

    public void UpdateGameSpeed(float nextSpeed)
    {
        ChangedSpeed = nextSpeed;
        Speed = ChangedSpeed;
    }
}
