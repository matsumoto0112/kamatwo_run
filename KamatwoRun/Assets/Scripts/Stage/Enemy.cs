using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// （皮に包まれることができる）敵クラス
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
