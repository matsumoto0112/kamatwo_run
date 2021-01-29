using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�Փ˔���N���X
/// </summary>
public class PlayerCollision : CharacterComponent
{
    private PlayerStatus playerStatus = null;

    public override void OnCreate()
    {
        base.OnCreate();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(playerStatus.IsDead() == true)
        {
            return;
        }

        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            playerStatus.AddScore(scoreObject.ScoreInfo.score);
            scoreObject.DestroySelf();
        }
        else if(other.gameObject.GetComponentToNullCheck(out DumplingSkin dumplingSkin) == true)
        {
            playerStatus.AddScore(dumplingSkin.score);
        }

        if(playerStatus.IsHit == true)
        {
            return;
        }

        if (other.gameObject.GetComponentToNullCheck(out Obstacle obstacle) == true)
        {
            playerStatus.Damage();
        }
        else if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            playerStatus.Damage();
        }
    }
}
