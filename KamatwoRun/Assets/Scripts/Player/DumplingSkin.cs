using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumplingSkin : MonoBehaviour
{
    #region ï`âÊ
    private Renderer dumplingSkinRenderer = null;
    private MeshRenderer meshRenderer = null;
    private BoxCollider boxCollider = null;
    #endregion

    private Vector3 initialScale = Vector3.zero;
    private IEnumerator shotCoroutine = null;
    public int score { get; private set; }

    public bool IsShot
    {
        get
        {
            return shotCoroutine != null;
        }
    }
    public bool IsHit { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        dumplingSkinRenderer = GetComponent<Renderer>();
        dumplingSkinRenderer.material.SetColor("_Color", Color.yellow);

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        initialScale = transform.localScale;
        IsHit = false;
        shotCoroutine = null;
    }

    private void Update()
    {
        if(IsHit == true)
        {
            return;
        }

        transform.eulerAngles += new Vector3(0, 1, 0);
    }

    /// <summary>
    /// î≠éÀèàóù
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotCoroutine()
    {
        float time = 0.0f;
        Vector3 parentPosition = transform.parent.GetChild(0).position;
        //ê≥ñ à⁄ìÆ
        while (true)
        {
            time += Time.deltaTime;
            Vector3 distination = parentPosition + (transform.parent.forward * 20);
            Vector3 vel = Vector3.Lerp(parentPosition, distination, time);
            transform.position = new Vector3(vel.x, transform.position.y, vel.z);

            if (time >= 1.0f || IsHit == true)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(SkinBackCoroutine());
        OnEnd();
    }

    private IEnumerator SkinBackCoroutine()
    {
        float time = 0.0f;
        Vector3 pos = transform.position;
        while (true)
        {
            time += Time.deltaTime;
            Vector3 vel = Vector3.Lerp(pos, transform.parent.GetChild(0).position, time / 0.5f);
            transform.position = new Vector3(vel.x, transform.position.y, vel.z);

            if (time >= 0.5f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// ìäÇ∞ÇΩéûÇÃèàóù
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        transform.localScale = initialScale;
        IsHit = false;
        score = 0;
        shotCoroutine = ShotCoroutine();
        StartCoroutine(shotCoroutine);
    }

    /// <summary>
    /// è’ìÀéûÇÃïœâª
    /// </summary>
    public void OnHit()
    {
        dumplingSkinRenderer.material.SetColor("_Color", Color.red);
        transform.localScale = Vector3.one;
        IsHit = true;
    }

    /// <summary>
    /// èIóπèàóù
    /// </summary>
    public void OnEnd()
    {
        dumplingSkinRenderer.material.SetColor("_Color", Color.yellow);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        transform.localScale = initialScale;
        IsHit = false;
        shotCoroutine = null;
        score = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsHit == true)
        {
            return;
        }

        //ìGÇ…è’ìÀÇµÇΩÇÁ
        if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            OnHit();
            score = wrappableObject.Wrap().score;
            wrappableObject.DestroySelf();
        }
    }
}
