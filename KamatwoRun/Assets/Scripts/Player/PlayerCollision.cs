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
        //包んだ餃子を取得したら
        else if (other.gameObject.GetComponentToNullCheck(out DumplingSkin dumplingSkin) == true)
        {
            //敵オブジェクトに衝突していた状態なら
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

        //カーブオブジェクト又はゴールしたら
        if (other.GetComponent<CurveCameraEvent>() != null ||
            other.GetComponent<GoalSubStage>() != null)
        {
            playerInput.OnEventInitialize();
            playerStatus.OnEventInitialize();
            eventManager.CurveEvent(other.gameObject);
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
