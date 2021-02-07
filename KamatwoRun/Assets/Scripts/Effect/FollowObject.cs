using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private DumplingSkin followTarget = null;
    private bool isUpdate = false;

    public void Initialize(DumplingSkin followTarget)
    {
        this.followTarget = followTarget;
        isUpdate = true;
    }

    private void Update()
    {
        if (isUpdate == false)
        {
            return;
        }

        if(followTarget.ThrowType == ThrowingItemType.None)
        {
            isUpdate = false;
            GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject,2.0f);
            return;
        }
        transform.position = followTarget.transform.position;
    }
}
