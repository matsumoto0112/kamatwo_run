using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �i��ɕ�܂�邱�Ƃ��ł���j�G�N���X
/// </summary>
public class Enemy : WrappableObject
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, 0, -10) * Time.deltaTime;
    }
}
