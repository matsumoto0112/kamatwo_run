using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー衝突判定クラス
/// </summary>
public class PlayerCollision : CharacterComponent
{
    [SerializeField]
    private GameObject smallEatParticle = null;

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
        //死亡していたら
        if (playerStatus.IsDead() == true)
        {
            return;
        }

        //スコアオブジェクトに衝突したら
        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            GameObject particle = Instantiate(smallEatParticle, transform.position + Vector3.up, Quaternion.identity);
            Destroy(particle, 1.5f);
            playerStatus.AddScore(scoreObject.ScoreInfo.score);
            scoreObject.DestroySelf();
        }
        //カーブオブジェクト又はゴールしたら
        if (other.GetComponent<CurveCameraEvent>() != null)
        {
            playerInput.OnEventInitialize();
            playerStatus.OnEventInitialize();
            eventManager.CurveEvent(other.gameObject);
        }
        else if(other.GetComponent<GoalSubStage>() != null)
        {
            playerInput.OnEventInitialize();
            playerStatus.OnEventInitialize();
            eventManager.GoalEvent(other.gameObject);
        }

        //ダメージを受けている途中なら
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
