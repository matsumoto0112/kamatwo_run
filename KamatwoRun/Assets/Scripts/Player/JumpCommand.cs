using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : CommandBase
{
    private PlayerMove playerMove = null;
    private PlayerStatus playerStatus = null;

    private Vector3 currentPosition = Vector3.zero;
    private bool isEnd = false;
    private float maxTime = 0.0f;
    private float t;

    private const float GRAVITY = 9.8f;
    private const float HEIGHT = 1.8f;

    public JumpCommand(ICharacterComponent character)
        : base(character)
    {
        playerMove = character.Parent.GetComponentInChildren<PlayerMove>();
        playerStatus = character.Parent.GetComponentInChildren<PlayerStatus>();
    }

    public override void Initialize()
    {
        base.Initialize();
        isEnd = false;
        currentPosition = playerMove.transform.position;
        t = 0.0f;
        maxTime = playerMove.CulcMaxArrivalTime(-GRAVITY * playerStatus.status.coefJumpSpeed, HEIGHT);
    }

    public override void Execution()
    {
        base.Execution();

        playerMove.Jump(-GRAVITY * playerStatus.status.coefJumpSpeed, HEIGHT, t);
        t += Time.deltaTime;
        if (t >= (maxTime * 2.0f))
        {
            playerMove.transform.position = currentPosition;
            isEnd = true;
        }
    }

    public override bool IsEnd()
    {
        return isEnd;
    }
}
