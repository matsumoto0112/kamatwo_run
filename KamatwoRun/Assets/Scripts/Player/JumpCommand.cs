using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : CommandBase
{
    private PlayerMove playerMove = null;
    private PlayerParameter playerParameter = null;

    private Vector3 currentPosition = Vector3.zero;
    private bool isEnd = false;
    private Timer timer;
    private Timer flightTimer;
    private bool isFlight = true;

    private const float GRAVITY = 9.8f;
    private const float HEIGHT = 1.8f;

    public JumpCommand(ICharacterComponent character)
        : base(character)
    {
        playerMove = character.CharacterTransform.GetComponent<PlayerMove>();
        playerParameter = character.CharacterTransform.GetComponent<PlayerParameter>();
    }

    public override void Initialize()
    {
        base.Initialize();
        isEnd = false;
        currentPosition = playerMove.transform.position;
        timer = new Timer(playerMove.CulcMaxArrivalTime(-GRAVITY * playerParameter.parameter.coefJumpSpeed, HEIGHT));
        flightTimer = new Timer(playerParameter.parameter.flightTime);
        isFlight = true;
    }

    public override void Execution()
    {
        base.Execution();
        //ïÇóVêßå‰
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isFlight = false;
        }

        if(timer.IsTime() == true && isFlight == true && flightTimer.IsTime() == false)
        {
            flightTimer.UpdateTimer();
            return;
        }
        playerMove.Jump(-GRAVITY * playerParameter.parameter.coefJumpSpeed, HEIGHT, timer.CurrentTime);
        timer.UpdateTimer();
        if (timer.IsTime(2.0f) == true)
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
