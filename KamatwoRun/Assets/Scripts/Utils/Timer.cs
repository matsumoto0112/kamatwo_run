using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    /// <summary>
    /// タイマータイプ
    /// </summary>
    public enum TimerType
    {
        INCREASE,
        DECREASE
    }

    private TimerType type = TimerType.INCREASE;

    /// <summary>
    /// タイマーの停止
    /// </summary>
    private bool isStop = false;


    /// <summary>
    /// 現在時間
    /// </summary>
    public float CurrentTime { get; private set; } = 0.0f;

    /// <summary>
    /// 制限時間
    /// </summary>
    public float LimitTime { get; private set; } = 0.0f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Timer()
    {
        isStop = false;
        type = TimerType.INCREASE;
        LimitTime = 1.0f;
        Initialize();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="limitTime">制限時間</param>
    /// <param name="type">タイマータイプ</param>
    public Timer(float limitTime, TimerType type = TimerType.INCREASE)
    {
        isStop = false;
        this.type = type;
        LimitTime = limitTime;
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        if (type == TimerType.INCREASE)
        {
            CurrentTime = 0.0f;
        }
        else if (type == TimerType.DECREASE)
        {
            CurrentTime = LimitTime;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void UpdateTimer()
    {
        if (isStop == true)
        {
            return;
        }

        if (type == TimerType.INCREASE)
        {
            CurrentTime += Time.deltaTime;
        }
        else if (type == TimerType.DECREASE)
        {
            CurrentTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 時間になったかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsTime()
    {
        if (type == TimerType.INCREASE)
        {
            return CurrentTime >= LimitTime;
        }
        else if (type == TimerType.DECREASE)
        {
            return CurrentTime <= 0.0f;
        }

        return false;
    }

    public bool IsTime(float coef)
    {
        if (type == TimerType.INCREASE)
        {
            return CurrentTime >= LimitTime * coef;
        }
        else if (type == TimerType.DECREASE)
        {
            return CurrentTime <= 0.0f;
        }

        return false;

    }

    /// <summary>
    /// タイマーの更新を始める/再開する
    /// </summary>
    public void Start()
    {
        isStop = false;
    }

    /// <summary>
    /// タイマーの更新を停止する
    /// </summary>
    public void Stop()
    {
        isStop = true;
    }
}
