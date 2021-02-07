using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : CharacterComponent
{
    [SerializeField, AudioSelect(SoundType.SE)]
    private string jumpSEName = "";
    [SerializeField, AudioSelect(SoundType.SE)]
    private string shotSEName = "";

    private SoundManager soundManager = null;
    private Dictionary<CommandType, CommandBase> commandList;
    public CommandType CommandType { get; private set; } = CommandType.NONE;

    public override void OnCreate()
    {
        soundManager = Parent.GetComponent<Player>().SoundManager;
        CommandType = CommandType.NONE;
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
        if (CommandType != CommandType.NONE)
        {
            commandList[CommandType].Execution();
            //コマンド終了検知
            if (commandList[CommandType].IsEnd() == true)
            {
                CommandType = CommandType.NONE;
            }
            return;
        }

        if (IsLeftMoveInput() == true)
        {
            CommandType = CommandType.LEFT_MOVE;
        }
        else if (IsRightMoveInput() == true)
        {
            CommandType = CommandType.RIGHT_MOVE;
        }
        else if (IsJumpInput() == true)
        {
            CommandType = CommandType.JUMP;
        }
        else if (IsShotInput() == true)
        {
            CommandType = CommandType.SHOT;
        }

        //コマンド入力があったら
        if (CommandType != CommandType.NONE)
        {
            commandList[CommandType].Initialize();
        }
    }

    public void OnEventInitialize()
    {
        //コマンドが実行中だったら
        if(CommandType != CommandType.NONE)
        {
            commandList[CommandType].EventInitialize();
            CommandType = CommandType.NONE;
        }
    }

    public void PlayJumpSE()
    {
        soundManager.PlaySE(jumpSEName);
    }

    public void PlayShotSE()
    {
        soundManager.PlaySE(shotSEName);
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
