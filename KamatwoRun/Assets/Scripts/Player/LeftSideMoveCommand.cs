using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSideMoveCommand : CommandBase
{
    private PlayerMove playerMove = null;
    private PlayerParameter playerParameter = null;
    private Vector3 currentPosition = Vector3.zero;

    private float time = 0.0f;

    public LeftSideMoveCommand(ICharacterComponent character) 
        : base(character)
    {
        playerMove = Character.CharacterTransform.GetComponent<PlayerMove>();
        playerParameter = Character.CharacterTransform.GetComponent<PlayerParameter>();
    }

    public override void Initialize()
    {
        base.Initialize();
        //現在位置の登録
        currentPosition = playerMove.transform.position;
        isEnd = false;
        time = 0.0f;

        LaneLocationType prevLocationType = playerMove.LocationType;
        playerMove.LeftSideMoveTypeChange();

        //動きに変更がなかった場合
        if (prevLocationType == playerMove.LocationType)
        {
            isEnd = true;
        }
    }

    public override void Execution()
    {
        base.Execution();

        if(isEnd == true)
        {
            return;
        }

        time += Time.deltaTime;
        playerMove.Move(currentPosition, time / playerParameter.parameter.timeToMove);

        if(playerMove.DistanceToDestination() <= 0.05f)
        {
            playerMove.transform.position = playerMove.NextMovePosition();
            isEnd = true;
        }
    }

    /// <summary>
    /// イベント発生時に行動していた時の処理
    /// </summary>
    public override void EventInitialize()
    {
        base.EventInitialize();
        //移動先の位置へワープさせる
        playerMove.transform.position = playerMove.NextMovePosition();
    }

    public override bool IsEnd()
    {
        return isEnd;
    }
}
