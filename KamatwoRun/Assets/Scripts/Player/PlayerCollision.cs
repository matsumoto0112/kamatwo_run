using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー衝突判定クラス
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
        //死亡していたら
        if(playerStatus.IsDead() == true)
        {
            return;
        }

        //スコアオブジェクトに衝突したら
        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            playerStatus.AddScore(scoreObject.ScoreInfo.score);
            scoreObject.DestroySelf();
        }
        else if(other.gameObject.GetComponentToNullCheck(out DumplingSkin dumplingSkin) == true)
        {
            playerStatus.AddScore(dumplingSkin.score);
        }

        //ダメージを受けている途中なら
        if(playerStatus.IsHit == true)
        {
            return;
        }

        if (other.GetComponent<Obstacle>() != null || other.GetComponent<WrappableObject>() != null)
        {
            playerStatus.Damage();
        }

        //カーブオブジェクトに接触したら
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
