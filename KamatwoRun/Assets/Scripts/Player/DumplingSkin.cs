using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
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
/// �����A�C�e���N���X
/// </summary>
public class DumplingSkin : MonoBehaviour
{
    #region �`��
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
    private Rigidbody rb = null;
    [SerializeField]
    private float shotPower = 10.0f;

    private float initialPosY = 0.0f;
    private IEnumerator shotCoroutine = null;
    private IEnumerator backCoroutine = null;
    //�����I�u�W�F�N�g�̏��
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

        shotCoroutine = null;
        backCoroutine = null;
        initialPosY = transform.position.y;
    }

    private void Update()
    {
        //�������Ă��Ȃ���Ԃ܂���
        //�G�I�u�W�F�N�g�ɏՓ˂��Ă�����
        if (ThrowType == ThrowingItemType.None ||
            ThrowType == ThrowingItemType.HitWrappableObject)
        {
            return;
        }

        transform.eulerAngles += new Vector3(0, 5, 0);
    }

    /// <summary>
    /// ���ˏ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotCoroutine()
    {
        //���f�������ɌX����
        transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
        Timer shotTime = new Timer();
        Timer stopShotObjectTime = new Timer();

        //���ʈړ�
        while (true)
        {
            if (stopShotObjectTime.IsTime() == true ||
                ThrowType == ThrowingItemType.HitObstacle ||
                ThrowType == ThrowingItemType.HitWrappableObject)
            {
                rb.velocity = Vector3.zero;
                break;
            }

            if (shotTime.IsTime() == true)
            {
                stopShotObjectTime.UpdateTimer();
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.velocity = modelTransform.forward.normalized * shotPower;
                shotTime.UpdateTimer();
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (ThrowType == ThrowingItemType.HitWrappableObject)
        {
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            ThrowType = ThrowingItemType.NoHit;
        }
        backCoroutine = SkinBackCoroutine();
        yield return StartCoroutine(backCoroutine);
    }

    /// <summary>
    /// ���������̂����Ƃ̏ꏊ�ɖ߂�
    /// </summary>
    /// <returns></returns>
    private IEnumerator SkinBackCoroutine()
    {
        Vector3 vel = Vector3.zero;
        while (true)
        {          
            //�����擾
            vel = (modelTransform.position + new Vector3(0.0f, 0.5f, 0.0f) - transform.position).normalized;
            rb.velocity = vel * shotPower;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// �����ړ�
    /// </summary>
    /// <param name="ct">���ݎ���</param>
    /// <param name="et">�I������</param>
    /// <param name="start">���߂̐��l</param>
    /// <param name="end">�I�����l</param>
    /// <returns></returns>
    public float Linear(float ct, float et, float start, float end)
    {
        if (ct > et)
            return end;
        end -= start;
        return end * ct / et + start;
    }

    /// <summary>
    /// ���������̏���
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        ThrowType = ThrowingItemType.Shot;
        WrappableObjectScore = 0;
        Vector3 position = modelTransform.position;
        position.y = initialPosY;
        transform.position = position;
        shotCoroutine = ShotCoroutine();
        StartCoroutine(shotCoroutine);
    }

    /// <summary>
    /// �G�I�u�W�F�N�g�Փˎ��̏���
    /// </summary>
    private void OnHitToWrappableObject()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", wrappTexture);
        transform.eulerAngles = modelTransform.eulerAngles;
        ThrowType = ThrowingItemType.HitWrappableObject;
    }

    /// <summary>
    /// �I������
    /// </summary>
    public void OnEnd()
    {
        dumplingSkinRenderer.material.SetTexture("_MainTex", dumplingTexture);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        ThrowType = ThrowingItemType.None;
        StopCoroutine(shotCoroutine);
        shotCoroutine = null;
        StopCoroutine(backCoroutine);
        backCoroutine = null;
        WrappableObjectScore = 0;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// �G���񂾌�̃X�R�A�v�Z����
    /// </summary>
    private void CulcScore(int wrappableObjectScore)
    {
        //���f���̈ʒu�Ǝ��M�̈ʒu�Ƃ̋��������߂�
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

        //�G�ɏՓ˂�����
        if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            OnHitToWrappableObject();
            //�X�R�A�v�Z
            CulcScore(wrappableObject.Wrap().score);
            wrappableObject.DestroySelf();
            //�G�t�F�N�g�쐬
            GameObject particle = Instantiate(smokeParticle, transform.position, Quaternion.identity);
            particle.transform.parent = transform;
            Destroy(particle, 1.5f);
        }
        //��Q���ɏՓ˂�����
        else if (other.gameObject.GetComponentToNullCheck(out Obstacle obstacle) == true)
        {
            ThrowType = ThrowingItemType.HitObstacle;
        }
    }
}
