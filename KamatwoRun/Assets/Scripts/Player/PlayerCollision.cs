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
    [SerializeField, AudioSelect(SoundType.SE)]
    private string eatSEName = "";
    [SerializeField, AudioSelect(SoundType.SE)]
    private string hitSEName = "";
    [SerializeField, AudioSelect(SoundType.SE)]
    private string goalSEName = "";

    private SoundManager soundManager = null;
    private EventManager eventManager = null;
    private PlayerStatus playerStatus = null;
    private PlayerInput playerInput = null;

    public override void OnCreate()
    {
        base.OnCreate();
        playerStatus = GetComponent<PlayerStatus>();
        playerInput = GetComponent<PlayerInput>();
        eventManager = Parent.GetComponent<Player>().EventManager;
        soundManager = Parent.GetComponent<Player>().SoundManager;
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
            soundManager.PlaySE(eatSEName);
            Vector3 position = transform.position + Vector3.up + (transform.forward * 1.2f);
            GameObject particle = Instantiate(smallEatParticle, position, Quaternion.identity);
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
            soundManager.PlaySE(goalSEName);
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
            soundManager.PlaySE(hitSEName);
            playerStatus.Damage();
        }
    }
}
