using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : CommandBase
{
    private PlayerMove playerMove = null;
    private PlayerParameter playerParameter = null;
    private ParticleSystem smokeParticle = null;

    private Vector3 currentPosition = Vector3.zero;
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
        smokeParticle = character.CharacterTransform.GetComponentInChildren<ParticleSystem>();
    }

    public override void Initialize()
    {
        base.Initialize();
        isEnd = false;
        currentPosition = playerMove.transform.position;
        timer = new Timer(playerMove.CulcMaxArrivalTime(-GRAVITY * playerParameter.parameter.coefJumpSpeed, HEIGHT));
        flightTimer = new Timer(playerParameter.parameter.flightTime);
        isFlight = true;
        smokeParticle.Stop();
    }

    public override void Execution()
    {
        base.Execution();
        //ïÇóVêßå‰
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isFlight = false;
        }
        //ïÇóV
        if(timer.IsTime() == true && isFlight == true && flightTimer.IsTime() == false)
        {
            flightTimer.UpdateTimer();
            return;
        }
        //ÉWÉÉÉìÉvèàóù
        playerMove.Jump(-GRAVITY * playerParameter.parameter.coefJumpSpeed, HEIGHT, timer.CurrentTime);
        timer.UpdateTimer();

        //èIóπèàóù
        if (timer.IsTime(2.0f) == true)
        {
            playerMove.transform.position = currentPosition;
            smokeParticle.Play();
            isEnd = true;
        }
    }

    public override void EventInitialize()
    {
        base.EventInitialize();
        playerMove.transform.position = currentPosition;
        smokeParticle.Play();
    }

    public override bool IsEnd()
    {
        return isEnd;
    }
}
