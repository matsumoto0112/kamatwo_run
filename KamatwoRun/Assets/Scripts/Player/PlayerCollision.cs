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
        //���S���Ă�����
        if(playerStatus.IsDead() == true)
        {
            return;
        }

        //�X�R�A�I�u�W�F�N�g�ɏՓ˂�����
        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            playerStatus.AddScore(scoreObject.ScoreInfo.score);
            scoreObject.DestroySelf();
        }
        else if(other.gameObject.GetComponentToNullCheck(out DumplingSkin dumplingSkin) == true)
        {
            playerStatus.AddScore(dumplingSkin.score);
        }

        //�_���[�W���󂯂Ă���r���Ȃ�
        if(playerStatus.IsHit == true)
        {
            return;
        }

        if (other.GetComponent<Obstacle>() != null || other.GetComponent<WrappableObject>() != null)
        {
            playerStatus.Damage();
        }

        //�J�[�u�I�u�W�F�N�g�ɐڐG������
        if(other.GetComponent<CurveCameraEvent>() != null)
        {
            EventManager.Instance.CurveEvent(other.gameObject);
        }
        else if(other.GetComponent<GoalSubStage>() != null)
        {
            EventManager.Instance.GoalEvent(gameObject,other.gameObject);
        }
    }
}
