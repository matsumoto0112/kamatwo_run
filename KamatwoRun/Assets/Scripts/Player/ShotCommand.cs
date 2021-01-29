using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCommand : CommandBase
{
    private PlayerShot playerShot = null;

    public ShotCommand(ICharacterComponent character)
        : base(character)
    {
        playerShot = Character.CharacterTransform.GetComponent<PlayerShot>();
    }

    public override void Initialize()
    {
        base.Initialize();
        if(playerShot.IsShot() == true)
        {
            return;
        }
        playerShot.SpawnDumplingSkin();
    }
}
