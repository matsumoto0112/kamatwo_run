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
        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            Debug.Log($"Hit to ScoreObject!!");
            playerStatus.AddScore(scoreObject.ScoreInfo.score);
            scoreObject.DestroySelf();
        }
        else if (other.gameObject.GetComponentToNullCheck(out Obstacle obstacle) == true)
        {
            Debug.Log($"Hit to Obstacle...");
        }
        else if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            Debug.Log("$Hit to EnemyObject");
        }
        else if(other.gameObject.GetComponentToNullCheck(out DumplingSkin dumplingSkin) == true)
        {
            playerStatus.AddScore(dumplingSkin.score);
        }
    }
}
