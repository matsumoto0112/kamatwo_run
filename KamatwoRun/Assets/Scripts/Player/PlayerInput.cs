using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : CharacterComponent
{
    private Dictionary<CommandType, CommandBase> commandList;
    private CommandType commandType = CommandType.NONE;


    public override void OnCreate()
    {
        commandType = CommandType.NONE;
        //コマンドリスト登録
        commandList = new Dictionary<CommandType, CommandBase>();
        commandList.Add(CommandType.LEFT_MOVE, new LeftSideMoveCommand(this));
        commandList.Add(CommandType.RIGHT_MOVE, new RightSideMoveCommand(this));
        commandList.Add(CommandType.JUMP, new JumpCommand(this));
        commandList.Add(CommandType.SHOT, new ShotCommand(this));
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
        else if (IsShotInput() == true)
        {
            commandType = CommandType.SHOT;
        }

        //コマンド入力があったら
        if (commandType != CommandType.NONE)
        {
            commandList[commandType].Initialize();
        }
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

    private bool IsShotInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    #endregion
}
