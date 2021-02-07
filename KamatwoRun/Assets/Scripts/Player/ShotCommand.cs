using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCommand : CommandBase
{
    private PlayerShot playerShot = null;
    private PlayerInput playerInput = null;

    public ShotCommand(ICharacterComponent character)
        : base(character)
    {
        playerShot = Character.CharacterTransform.GetComponent<PlayerShot>();
        playerInput = Character.CharacterTransform.GetComponent<PlayerInput>();
    }

    public override void Initialize()
    {
        base.Initialize();
        if(playerShot.IsShot() == true)
        {
            return;
        }
        playerShot.SpawnDumplingSkin();
        playerInput.PlayShotSE();
    }
}
