using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投擲状態
/// </summary>
public enum ThrowingItemType
{
    None = 0,
    Shot,
    HitObstacle,
    HitWrappableObject,
    NoHit,
}


/// <summary>
/// 投擲アイテムクラス
/// </summary>
public class DumplingSkin : MonoBehaviour
{
    #region 描画
    private Renderer dumplingSkinRenderer = null;
    private MeshRenderer meshRenderer = null;
    private BoxCollider boxCollider = null;
    #endregion

    [SerializeField]
    private Transform modelTransform = null;
    [SerializeField]
    private GameObject bigEatParticle = null;
    [SerializeField]
    private GameObject smokeParticle = null;
    [SerializeField]
    private Texture dumplingTexture = null;
    [SerializeField]
    private Texture wrappTexture = null;
    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private float shotPower = 10.0f;
    [SerializeField,AudioSelect(SoundType.SE)]
    private string eatSEName = "";
    [SerializeField,AudioSelect(SoundType.SE)]
    private string wrapSEName = "";

    private SoundManager soundManager = null;
    private Timer shotTime;
    private Timer stopShotObjectTime;
    private PlayerStatus playerStatus = null;

    private float initialPosY = 0.0f;
    //投擲オブジェクトの状態
    public ThrowingItemType ThrowType { get; private set; } = ThrowingItemType.None;

    public int WrappableObjectScore { get; private set; }

    public bool IsShot => ThrowType != ThrowingItemType.None;

    // Start is called before the first frame update
    void Start()
    {
        dumplingSkinRenderer = GetComponent<Renderer>();
        dumplingSkinRenderer.material.SetTexture("_MainTex", dumplingTexture);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        ThrowType = ThrowingItemType.None;

        initialPosY = transform.position.y;

        shotTime = new Timer();
        stopShotObjectTime = new Timer();
        playerStatus = modelTransform.GetComponent<PlayerStatus>();
        soundManager = GetComponentInParent<Player>().SoundManager;
    }

    private void Update()
    {
        switch (ThrowType)
        {
            case ThrowingItemType.HitWrappableObject:
                HitWrappableEvent();
                break;
            case ThrowingItemType.HitObstacle:
            case ThrowingItemType.NoHit:
                transform.eulerAngles += new Vector3(0, 5, 0);
                ObjectBackEvent();
                break;
            case ThrowingItemType.Shot:
                transform.eulerAngles += new Vector3(0, 5, 0);
                ShotEvent();
                break;
            case ThrowingItemType.None:
                break;
        }
    }

    private void HitWrappableEvent()
    {
        //一定時間停止
        stopShotObjectTime.UpdateTimer();
        if (stopShotObjectTime.IsTime() == true)
        {
            Vector3 vel = Vector3.zero;
            //方向取得
            vel = (modelTransform.position + new Vector3(0.0f, 0.5f, 0.0f) - transform.position).normalized;
            rb.velocity = vel * shotPower;
            if (DistanceCheck() == true)
            {
                soundManager.PlaySE(eatSEName);
                Destroy(Instantiate(bigEatParticle, modelTransform.position + Vector3.up, Quaternion.identity), 2.0f);
                playerStatus.AddScore(WrappableObjectScore);
                OnEnd();
            }
        }
    }

    /// <summary>
    /// 発射時のイベント
    /// </summary>
    private void ShotEvent()
    {
        //停止時間を過ぎたら
        if (stopShotObjectTime.IsTime() == true)
        {
            rb.velocity = Vector3.zero;
            ThrowType = ThrowingItemType.NoHit;
        }

        //投擲時間が終わったら
        if (shotTime.IsTime() == true)
        {
            stopShotObjectTime.UpdateTimer();
            rb.velocity = Vector3.zero;
        }
        //投擲中
        else
        {
            rb.velocity = modelTransform.forward.normalized * shotPower;
            shotTime.UpdateTimer();
        }
    }

    /// <summary>
    /// 何も当たらなかったら
    /// </summary>
    private void ObjectBackEvent()
    {
        Vector3 vel = Vector3.zero;
        //方向取得
        vel = (modelTransform.position + new Vector3(0.0f, 0.5f, 0.0f) - transform.position).normalized;
        rb.velocity = vel * shotPower;

        //距離判定
        if (DistanceCheck() == true)
        {
            OnEnd();
        }
    }

    /// <summary>
    /// プレイヤーモデルと投擲物との距離を調べる
    /// </summary>
    /// <returns></returns>
    private bool DistanceCheck()
    {
        //プレイヤーモデルと投擲物との距離
        float distance = Vector3.Distance(modelTransform.position, transform.position);
        if (distance <= 2.0f)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 投げた時の処理
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        WrappableObjectScore = 0;
        Vector3 position = modelTransform.position;
        position.y = initialPosY;
        transform.position = position;
        //モデルを横に傾ける
        transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        shotTime.Initialize();
        stopShotObjectTime.Initialize();
        ThrowType = ThrowingItemType.Shot;
    }

    /// <summary>
    /// 敵オブジェクト衝突字の処理
    /// </summary>
    private void OnHitToWrappableObject()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", wrappTexture);
        transform.eulerAngles = modelTransform.eulerAngles;
        stopShotObjectTime.Initialize();
        rb.velocity = Vector3.zero;
        ThrowType = ThrowingItemType.HitWrappableObject;
        soundManager.PlaySE(wrapSEName);
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void OnEnd()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", dumplingTexture);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        WrappableObjectScore = 0;
        rb.velocity = Vector3.zero;
        ThrowType = ThrowingItemType.None;
    }

    /// <summary>
    /// 敵を包んだ後のスコア計算処理
    /// </summary>
    private void CulcScore(int wrappableObjectScore)
    {
        //モデルの位置と自信の位置との距離を求める
        float distance = Vector3.Distance(modelTransform.position, transform.position);
        float coef = 0.0f;
        if (distance <= shotPower / 3.0f)
        {
            coef = 1.5f;
        }
        else if (distance <= shotPower / 2.0f)
        {
            coef = 1.25f;
        }
        else
        {
            coef = 1.0f;
        }
        WrappableObjectScore = (int)(wrappableObjectScore * coef);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ThrowType == ThrowingItemType.HitObstacle ||
            ThrowType == ThrowingItemType.HitWrappableObject ||
            ThrowType == ThrowingItemType.NoHit)
        {
            return;
        }

        //敵に衝突したら
        if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            OnHitToWrappableObject();
            //スコア計算
            CulcScore(wrappableObject.Wrap().score);
            wrappableObject.DestroySelf();
            //エフェクト作成
            GameObject particle = Instantiate(smokeParticle, transform.position, Quaternion.identity);
            particle.transform.parent = transform;
            Destroy(particle, 1.5f);
        }
        //障害物に衝突したら
        else if (other.gameObject.GetComponentToNullCheck(out Obstacle obstacle) == true)
        {
            ThrowType = ThrowingItemType.HitObstacle;
        }
    }
}
