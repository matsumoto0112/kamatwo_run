using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumplingSkin : MonoBehaviour
{
    #region •`‰æ
    private Renderer dumplingSkinRenderer = null;
    private MeshRenderer meshRenderer = null;
    private BoxCollider boxCollider = null;
    #endregion

    private Vector3 initialScale = Vector3.zero;
    private IEnumerator shotCoroutine = null;
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

    /// <summary>
    /// ”­Ëˆ—
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotCoroutine()
    {
        float time = 0.0f;
        Vector3 parentPosition = transform.parent.GetChild(0).position;
        //³–ÊˆÚ“®
        while (true)
        {
            time += Time.deltaTime;
            Vector3 distination = parentPosition + (transform.parent.forward * 20);
            Vector3 vel = Vector3.Lerp(parentPosition, distination, time);
            transform.position = new Vector3(vel.x, transform.position.y, vel.z);

            if (time >= 2.0f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        time = 0.0f;
        Vector3 pos = transform.position;
        parentPosition = transform.parent.GetChild(0).position;
        while (true)
        {
            time += Time.deltaTime;
            Vector3 vel = Vector3.Lerp(pos, parentPosition, time);
            transform.position = new Vector3(vel.x, transform.position.y, vel.z);

            if (time >= 1.0f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        OnEnd();
        yield return null;

    }

    /// <summary>
    /// “Š‚°‚½‚Ìˆ—
    /// </summary>
    public void OnCreate()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        transform.localScale = initialScale;
        IsHit = false;
        shotCoroutine = ShotCoroutine();
        StartCoroutine(shotCoroutine);
    }

    /// <summary>
    /// Õ“Ë‚Ì•Ï‰»
    /// </summary>
    public void OnHit()
    {
        dumplingSkinRenderer.material.SetColor("_Color", Color.red);
        transform.localScale = Vector3.one;
        IsHit = true;
    }

    /// <summary>
    /// I—¹ˆ—
    /// </summary>
    public void OnEnd()
    {
        dumplingSkinRenderer.material.SetColor("_Color", Color.yellow);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        transform.localScale = initialScale;
        IsHit = false;
        shotCoroutine = null;
    }
}
