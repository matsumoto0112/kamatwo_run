using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterComponent
{
    [SerializeField]
    private GameObject playerMeshObject = null;
    private PlayerParameter playerParameter = null;
    private Timer blinkingTimer;    //点滅時間計測
    private Timer invincibleTimer;  //無敵時間計測

    private Animator animator = null;

    public bool IsHit { get; private set; }
    public bool IsCreate { get; private set; } = false;
    public int Score { get; private set; }
    public int HP { get; private set; }

    public override void OnCreate()
    {
        base.OnCreate();
        playerParameter = GetComponent<PlayerParameter>();
        blinkingTimer = new Timer(0.1f);
        invincibleTimer = new Timer(playerParameter.parameter.invincibleTime);

        Score = 0;
        HP = playerParameter.parameter.hp;
        IsHit = false;
        IsCreate = true;
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //衝突中もしくは死亡していたら
        if(IsHit == false || IsDead() == true)
        {
            return;
        }

        invincibleTimer.UpdateTimer();
        if(invincibleTimer.IsTime() == true)
        {
            IsHit = false;
            playerMeshObject.SetActive(true);
            invincibleTimer.Initialize();
            blinkingTimer.Initialize();
            return;
        }

        blinkingTimer.UpdateTimer();
        if(blinkingTimer.IsTime() == true)
        {
            playerMeshObject.SetActive(!playerMeshObject.activeSelf);
            blinkingTimer.Initialize();
        }
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.Score += score;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage = 1)
    {
        if(IsDead() == true)
        {
            return;
        }
        HP -= damage;
        IsHit = true;
        //死亡時にデータ保存とアニメーション再生
        if(IsDead() == true)
        {
            GameDataStore.Instance.Score = Score;
            animator.applyRootMotion = false;
            animator.SetTrigger("Dead");
        }
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="recovery"></param>
    public void Recovery(int recovery = 1)
    {
        HP += recovery;
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return HP <= 0;
    }
}
