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
    private float shotPower = 10.0f;

    private IEnumerator shotCoroutine = null;
    //�����I�u�W�F�N�g�̏��
    public  ThrowingItemType ThrowType{ get; private set; } = ThrowingItemType.None;

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
    }

    private void Update()
    {
        //�������Ă��Ȃ���Ԃ܂���
        //�G�I�u�W�F�N�g�ɏՓ˂��Ă�����
        if(ThrowType == ThrowingItemType.None || 
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
        Vector3 modelPosition = modelTransform.position;
        Vector3 shotDistination = modelTransform.position + (modelTransform.forward * shotPower);

        //���ʈړ�
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
                Vector3 vel = Vector3.Lerp(modelPosition, shotDistination, shotTime.CurrentTime);
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
    /// ���������̂����Ƃ̏ꏊ�ɖ߂�
    /// </summary>
    /// <returns></returns>
    private IEnumerator SkinBackCoroutine()
    {
        float time = 0.0f;
        Vector3 pos = transform.position;
        while (true)
        {
            time += Time.deltaTime;
            Vector3 vel = Vector3.Lerp(pos, modelTransform.position, time / 0.5f);
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
    /// ���������̏���
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        ThrowType = ThrowingItemType.Shot;
        WrappableObjectScore = 0;
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
        shotCoroutine = null;
        WrappableObjectScore = 0;
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
        if(ThrowType == ThrowingItemType.HitObstacle || 
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
