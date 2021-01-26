using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum LaneLocationType
{
    LEFT_SIDE = 0,
    MIDDLE,
    RIGHT_SIDE,
}

public class PlayerMove : MonoBehaviour, ICharacterComponent
{
    private LaneLocationType locationType = LaneLocationType.MIDDLE;
    private LaneLocationType prevLocationType = LaneLocationType.MIDDLE;

    private LanePositions lanePositions = null;
    private Vector3 currentPosition = Vector3.zero;

    private float t;

    public void OnCreate()
    {
        locationType = LaneLocationType.MIDDLE;
        prevLocationType = locationType;
        lanePositions = transform.parent.GetComponentInChildren<LanePositions>();
        lanePositions.Initialize();
        currentPosition = transform.position;
        t = 0;
    }

    public void OnUpdate()
    {
        if (prevLocationType != locationType)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPosition, NextMovePosition(), t / 1.0f);
            if(Vector3.Distance(transform.position,NextMovePosition()) <= 0.1f)
            {
                transform.position = NextMovePosition();
                currentPosition = transform.position;
                prevLocationType = locationType;
                t = 0;
            }
            return;
        }

        Move();
    }

    public void OnEnd()
    {
    }

    /// <summary>
    /// 移動入力処理
    /// </summary>
    private void Move()
    {
        if (IsLeftMoveInput() == true)
        {
            prevLocationType = locationType;
            locationType = (LaneLocationType)Mathf.Clamp((int)locationType - 1, 0, Enum.GetValues(typeof(LaneLocationType)).Length - 1);
            Debug.Log($"num = {(int)locationType} : type = {locationType}");
        }
        else if (IsRightMoveInput() == true)
        {
            prevLocationType = locationType;
            locationType = (LaneLocationType)Mathf.Clamp((int)locationType + 1, 0, Enum.GetValues(typeof(LaneLocationType)).Length - 1);
            Debug.Log($"num = {(int)locationType} : type = {locationType}");
        }
    }

    /// <summary>
    /// 次のレーン移動先の位置を返す
    /// </summary>
    /// <returns></returns>
    private Vector3 NextMovePosition()
    {
        return lanePositions.LanePositionList[(int)locationType].position;
    }

    /// <summary>
    /// 左側移動入力処理判定
    /// </summary>
    /// <returns></returns>
    private bool IsLeftMoveInput()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
    }

    /// <summary>
    /// 左側移動入力処理判定
    /// </summary>
    /// <returns></returns>
    private bool IsRightMoveInput()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
    }

    /// <summary>
    /// ジャンプ入力処理判定
    /// </summary>
    /// <returns></returns>
    private bool IsJumpInput()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    }
}
