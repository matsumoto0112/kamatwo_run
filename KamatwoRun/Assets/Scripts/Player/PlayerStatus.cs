using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterComponent
{
    [SerializeField]
    private GameObject playerMeshObject = null;
    [SerializeField,AudioSelect(SoundType.SE)]
    private string scoreSEName = "";

    private SoundManager soundManager = null;
    private PlayerParameter playerParameter = null;
    private Timer blinkingTimer;    //�_�Ŏ��Ԍv��
    private Timer invincibleTimer;  //���G���Ԍv��

    private Animator animator = null;

    public bool IsHit { get; private set; }
    public bool IsCreate { get; private set; } = false;
    public int Score { get; private set; }
    public int HP { get; private set; }

    public override void OnCreate()
    {
        base.OnCreate();
        soundManager = Parent.GetComponent<Player>().SoundManager;
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
        //�Փ˒��������͎��S���Ă�����
        if(IsHit == false || IsDead() == true)
        {
            return;
        }

        HitObstacle();
    }

    /// <summary>
    /// �Փˎ��̏���
    /// </summary>
    private void HitObstacle()
    {
        //���G����
        invincibleTimer.UpdateTimer();
        if (invincibleTimer.IsTime() == true)
        {
            IsHit = false;
            playerMeshObject.SetActive(true);
            invincibleTimer.Initialize();
            blinkingTimer.Initialize();
            return;
        }

        //�_��
        blinkingTimer.UpdateTimer();
        if (blinkingTimer.IsTime() == true)
        {
            playerMeshObject.SetActive(!playerMeshObject.activeSelf);
            blinkingTimer.Initialize();
        }
    }

    /// <summary>
    /// �X�R�A���Z
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        soundManager.PlaySE(scoreSEName);
        this.Score += score;
    }

    /// <summary>
    /// �_���[�W���󂯂�
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
        //���S���Ƀf�[�^�ۑ��ƃA�j���[�V�����Đ�
        if(IsDead() == true)
        {
            animator.applyRootMotion = false;
            animator.SetTrigger("Dead");
            Parent.GetComponent<Player>().EventManager.GameOverEvent();
        }
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <param name="recovery"></param>
    public void Recovery(int recovery = 1)
    {
        HP += recovery;
    }

    /// <summary>
    /// ���S����
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return HP <= 0;
    }

    /// <summary>
    /// �C�x���g���̏�����
    /// </summary>
    public void OnEventInitialize()
    {
        playerMeshObject.SetActive(true);
    }
}
