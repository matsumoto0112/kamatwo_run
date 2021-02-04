using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumplingSkin : MonoBehaviour
{
    #region 描画
    private Renderer dumplingSkinRenderer = null;
    private MeshRenderer meshRenderer = null;
    private BoxCollider boxCollider = null;
    #endregion

    [SerializeField]
    private GameObject smokeParticle = null;
    [SerializeField]
    private Texture dumplingTexture = null;
    [SerializeField]
    private Texture wrappTexture = null;
    [SerializeField]
    private float shotPower = 10.0f;

    private IEnumerator shotCoroutine = null;

    public int score { get; private set; }

    public bool IsShot
    {
        get
        {
            return shotCoroutine != null;
        }
    }

    //敵に衝突して包んだかどうか
    public bool IsHit { get; private set; }
    //障害物に当たったかどうか
    public bool IsObstacleHit { get; private set; }
    public bool IsNoHit { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        dumplingSkinRenderer = GetComponent<Renderer>();
        dumplingSkinRenderer.material.SetTexture("_MainTex", dumplingTexture);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        IsHit = false;
        IsObstacleHit = false;
        IsNoHit = false;
        shotCoroutine = null;
    }

    private void Update()
    {
        if (IsHit == true)
        {
            return;
        }

        transform.eulerAngles += new Vector3(0, 1, 0);
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
        Transform modelTransform = transform.parent.GetChild(0);
        Vector3 modelPosition = modelTransform.position;
        Vector3 shotDistination = modelTransform.position + (modelTransform.forward * shotPower);

        //正面移動
        while (true)
        {
            if (stopShotObjectTime.IsTime() == true || IsHit == true || IsObstacleHit == true)
            {
                break;
            }

            if (shotTime.IsTime() == true)
            {
                stopShotObjectTime.UpdateTimer();
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }

            Vector3 vel = Vector3.Lerp(modelPosition, shotDistination, shotTime.CurrentTime);
            transform.position = new Vector3(vel.x, transform.position.y, vel.z);
            shotTime.UpdateTimer();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (IsHit == true)
        {
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(SkinBackCoroutine());
        }
        else if (IsObstacleHit == true)
        {
            yield return StartCoroutine(SkinBackCoroutine());
        }
        else
        {
            IsNoHit = true;
            yield return StartCoroutine(SkinBackCoroutine());
        }
    }

    /// <summary>
    /// 投げたものをもとの場所に戻す
    /// </summary>
    /// <returns></returns>
    private IEnumerator SkinBackCoroutine()
    {
        float time = 0.0f;
        Vector3 pos = transform.position;
        while (true)
        {
            time += Time.deltaTime;
            Vector3 vel = Vector3.Lerp(pos, transform.parent.GetChild(0).position, time / 0.5f);
            vel.y = Mathf.Clamp(vel.y, 1.5f, Mathf.Infinity);
            transform.position = vel;

            if (time >= 0.5f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// 投げた時の処理
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        IsHit = false;
        IsNoHit = false;
        IsObstacleHit = false;
        score = 0;
        shotCoroutine = ShotCoroutine();
        StartCoroutine(shotCoroutine);
    }

    /// <summary>
    /// 衝突時の変化
    /// </summary>
    public void OnHit()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", wrappTexture);
        transform.eulerAngles = transform.parent.GetChild(0).eulerAngles;
        IsHit = true;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void OnEnd()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", dumplingTexture);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        IsHit = false;
        IsNoHit = false;
        IsObstacleHit = false;
        shotCoroutine = null;
        score = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsHit == true || IsObstacleHit == true)
        {
            return;
        }
        //戻るまでに何にも当たらなかったら
        if(IsNoHit == true)
        {
            return;
        }
        //敵に衝突したら
        if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            OnHit();
            //モデルと投擲アイテムとの距離
            float distance = Vector3.Distance(transform.parent.GetChild(0).position, transform.position);
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
            score = (int)(wrappableObject.Wrap().score * coef);
            wrappableObject.DestroySelf();
            GameObject particle = Instantiate(smokeParticle, transform.position, Quaternion.identity);
            particle.transform.parent = transform;
            Destroy(particle, 1.5f);
        }
        //障害物に衝突したら
        else if (other.gameObject.GetComponentToNullCheck(out Obstacle obstacle) == true)
        {
            IsObstacleHit = true;
        }
    }
}
