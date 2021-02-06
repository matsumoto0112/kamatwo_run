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
    private GameObject smokeParticle = null;
    [SerializeField]
    private Texture dumplingTexture = null;
    [SerializeField]
    private Texture wrappTexture = null;
    [SerializeField]
    private float shotPower = 10.0f;

    private float initialPosY = 0.0f;
    private IEnumerator shotCoroutine = null;
    //投擲オブジェクトの状態
    public ThrowingItemType ThrowType { get; private set; } = ThrowingItemType.None;

    public int WrappableObjectScore { get; private set; }

    public bool IsShot => shotCoroutine != null;

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

        shotCoroutine = null;
        initialPosY = transform.position.y;
    }

    private void Update()
    {
        //投擲していない状態または
        //敵オブジェクトに衝突していたら
        if (ThrowType == ThrowingItemType.None ||
            ThrowType == ThrowingItemType.HitWrappableObject)
        {
            return;
        }

        transform.eulerAngles += new Vector3(0, 5, 0);
    }

    /// <summary>
    /// 発射処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotCoroutine()
    {
        //モデルを横に傾ける
        transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        Timer shotTime = new Timer();
        Timer stopShotObjectTime = new Timer();
        Vector3 modelPosition = modelTransform.position;
        Vector3 shotDistination = modelTransform.position + (modelTransform.forward * shotPower);
        Vector3 vel = Vector3.zero;

        //正面移動
        while (true)
        {
            if (stopShotObjectTime.IsTime() == true ||
                ThrowType == ThrowingItemType.HitObstacle ||
                ThrowType == ThrowingItemType.HitWrappableObject)
            {
                break;
            }

            if (shotTime.IsTime() == true)
            {
                stopShotObjectTime.UpdateTimer();
            }
            else
            {
                vel.x = Linear(shotTime.CurrentTime, shotTime.LimitTime, modelPosition.x, shotDistination.x);
                vel.y = transform.position.y;
                vel.z = Linear(shotTime.CurrentTime, shotTime.LimitTime, modelPosition.z, shotDistination.z);

                transform.position = new Vector3(vel.x, transform.position.y, vel.z);
                shotTime.UpdateTimer();
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (ThrowType == ThrowingItemType.HitWrappableObject)
        {
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(SkinBackCoroutine());
        }
        else if (ThrowType == ThrowingItemType.HitObstacle)
        {
            yield return StartCoroutine(SkinBackCoroutine());
        }
        else
        {
            ThrowType = ThrowingItemType.NoHit;
            yield return StartCoroutine(SkinBackCoroutine());
        }
    }

    /// <summary>
    /// 投げたものをもとの場所に戻す
    /// </summary>
    /// <returns></returns>
    private IEnumerator SkinBackCoroutine()
    {
        Timer timer = new Timer(0.5f);
        Vector3 pos = transform.position;
        Vector3 vel = Vector3.zero;
        while (true)
        {
            timer.UpdateTimer();
            vel.x = Linear(timer.CurrentTime, timer.LimitTime, pos.x, modelTransform.position.x);
            vel.y = Mathf.Clamp(vel.y, 0.5f + modelTransform.position.y, Mathf.Infinity);
            vel.z = Linear(timer.CurrentTime, timer.LimitTime, pos.z, modelTransform.position.z);
            transform.position = vel;

            if (timer.IsTime() == true)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// 等速移動
    /// </summary>
    /// <param name="ct">現在時間</param>
    /// <param name="et">終了時間</param>
    /// <param name="start">初めの数値</param>
    /// <param name="end">終了数値</param>
    /// <returns></returns>
    public float Linear(float ct, float et, float start, float end)
    {
        if (ct > et)
            return end;
        end -= start;
        return end * ct / et + start;
    }

    /// <summary>
    /// 投げた時の処理
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        ThrowType = ThrowingItemType.Shot;
        WrappableObjectScore = 0;
        Vector3 position = transform.position;
        position.y = initialPosY;
        transform.position = position;
        shotCoroutine = ShotCoroutine();
        StartCoroutine(shotCoroutine);
    }

    /// <summary>
    /// 敵オブジェクト衝突字の処理
    /// </summary>
    private void OnHitToWrappableObject()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", wrappTexture);
        transform.eulerAngles = modelTransform.eulerAngles;
        ThrowType = ThrowingItemType.HitWrappableObject;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void OnEnd()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", dumplingTexture);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        ThrowType = ThrowingItemType.None;
        shotCoroutine = null;
        WrappableObjectScore = 0;
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
