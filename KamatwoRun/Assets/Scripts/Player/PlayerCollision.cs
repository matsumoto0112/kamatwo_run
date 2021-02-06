using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�Փ˔���N���X
/// </summary>
public class PlayerCollision : CharacterComponent
{
    [SerializeField]
    private GameObject smallEatParticle = null;
    [SerializeField]
    private GameObject bigEatParticle = null;

    private EventManager eventManager = null;
    private PlayerStatus playerStatus = null;
    private PlayerInput playerInput = null;

    public override void OnCreate()
    {
        base.OnCreate();
        playerStatus = GetComponent<PlayerStatus>();
        playerInput = GetComponent<PlayerInput>();
        eventManager = Parent.GetComponent<Player>().EventManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        //���S���Ă�����
        if (playerStatus.IsDead() == true)
        {
            return;
        }

        //�X�R�A�I�u�W�F�N�g�ɏՓ˂�����
        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            GameObject particle = Instantiate(smallEatParticle, transform.position + Vector3.up, Quaternion.identity);
            Destroy(particle, 1.5f);
            playerStatus.AddScore(scoreObject.ScoreInfo.score);
            scoreObject.DestroySelf();
        }
        //����L�q���擾������
        else if (other.gameObject.GetComponentToNullCheck(out DumplingSkin dumplingSkin) == true)
        {
            //�G�I�u�W�F�N�g�ɏՓ˂��Ă�����ԂȂ�
            if (dumplingSkin.ThrowType == ThrowingItemType.HitWrappableObject)
            {
                GameObject particle = Instantiate(bigEatParticle, transform.position + Vector3.up, Quaternion.identity);
                Destroy(particle, 1.5f);
                playerStatus.AddScore(dumplingSkin.WrappableObjectScore);
            }
            else if(dumplingSkin.ThrowType == ThrowingItemType.None || 
                dumplingSkin.ThrowType == ThrowingItemType.Shot)
            {
                return;
            }

            dumplingSkin.OnEnd();
        }

        //�J�[�u�I�u�W�F�N�g���̓S�[��������
        if (other.GetComponent<CurveCameraEvent>() != null ||
            other.GetComponent<GoalSubStage>() != null)
        {
            playerInput.OnEventInitialize();
            playerStatus.OnEventInitialize();
            eventManager.CurveEvent(other.gameObject);
        }

        //�_���[�W���󂯂Ă���r���Ȃ�
        if (playerStatus.IsHit == true)
        {
            return;
        }

        if (other.GetComponent<Obstacle>() != null || other.GetComponent<WrappableObject>() != null)
        {
            playerStatus.Damage();
        }
    }
}
