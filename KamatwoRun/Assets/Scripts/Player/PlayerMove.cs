using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayerMove :CharacterComponent
{
    public LaneLocationType LocationType { get; private set; }

    private LanePositions lanePositions = null;

    private Dictionary<CommandType, CommandBase> commandList;
    private CommandType commandType = CommandType.NONE;


    public override void OnCreate()
    {
        LocationType = LaneLocationType.MIDDLE;
        lanePositions = transform.parent.GetComponentInChildren<LanePositions>();
        lanePositions.Initialize();

        commandType = CommandType.NONE;
        //コマンドリスト登録
        commandList = new Dictionary<CommandType, CommandBase>();
        commandList.Add(CommandType.LEFT_MOVE, new LeftSideMoveCommand(this));
        commandList.Add(CommandType.RIGHT_MOVE, new RightSideMoveCommand(this));
        commandList.Add(CommandType.JUMP, new JumpCommand(this));
        commandList.Add(CommandType.SHOT, new CommandBase(this));
    }

    public override void OnUpdate()
    {
        //コマンド実行
        if (commandType != CommandType.NONE)
        {
            commandList[commandType].Execution();
            //コマンド終了検知
            if (commandList[commandType].IsEnd() == true)
            {
                commandType = CommandType.NONE;
            }
            return;
        }

        if (IsLeftMoveInput() == true)
        {
            commandType = CommandType.LEFT_MOVE;
        }
        else if (IsRightMoveInput() == true)
        {
            commandType = CommandType.RIGHT_MOVE;
        }
        else if (IsJumpInput() == true)
        {
            commandType = CommandType.JUMP;
        }

        //コマンド入力があったら
        if (commandType != CommandType.NONE)
        {
            commandList[commandType].Initialize();
        }
    }

    /// <summary>
    /// 左レーンへの状態を変更する
    /// </summary>
    public void LeftSideMoveTypeChange()
    {
        LocationType = (LaneLocationType)Mathf.Clamp((int)LocationType - 1, 0, Enum.GetValues(typeof(LaneLocationType)).Length - 1);
        Debug.Log($"num = {(int)LocationType} : type = {LocationType}");
    }

    public void RightSideMoveTypeChange()
    {
        LocationType = (LaneLocationType)Mathf.Clamp((int)LocationType + 1, 0, Enum.GetValues(typeof(LaneLocationType)).Length - 1);
        Debug.Log($"num = {(int)LocationType} : type = {LocationType}");
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="t"></param>
    public void Move(Vector3 currentPosition, float t)
    {
        transform.position = Vector3.Lerp(currentPosition, NextMovePosition(), t);
    }

    /// <summary>
    /// ジャンプ時の最高到達点までにかかる時間を計算
    /// </summary>
    /// <param name="a">加速度</param>
    /// <param name="height">高さ</param>
    /// <returns></returns>
    public float CulcMaxArrivalTime(float a,float height)
    {
        //速度(m/s)² - 初速度(m/s)² = 2 * 加速度 * 変位
        float c = 2 * a * height;
        float v0 = Mathf.Sqrt(c * -1);

        float hightTime = v0 / (a * -1);
        //Debug.Log($"最高到達点に行くまでの時間->{hightTime}");
        return hightTime;
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    /// <param name="a">加速度</param>
    /// <param name="height">高さ</param>
    /// <param name="t">時間</param>
    /// <returns></returns>
    public float Jump(float a,float height,float t)
    {
        //速度(m/s)² - 初速度(m/s)² = 2 * 加速度 * 変位(最高到達点height)
        float c = 2 * a * height;
        float v0 = Mathf.Sqrt(c * -1);
        //Debug.Log($"初速->{v0}");

        //v = v0 + at;
        float v = v0 + (a * t);
        v *= Time.deltaTime;
        //Debug.Log($"現在の速度->{v}");
        transform.position += new Vector3(0, v, 0);
        return v;
    }

    /// <summary>
    /// 目的地までの距離
    /// </summary>
    /// <returns></returns>
    public float DistanceToDestination()
    {
        return Vector3.Distance(transform.position, NextMovePosition());
    }

    /// <summary>
    /// 次のレーン移動先の位置を返す
    /// </summary>
    /// <returns></returns>
    public Vector3 NextMovePosition()
    {
        return lanePositions.LanePositionList[(int)LocationType].position;
    }

    #region Input

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

    #endregion
}
