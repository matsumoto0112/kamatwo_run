using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump pushed!");
        }

        Debug.Log($"{Input.GetAxis("RightMove")}");
        if (Input.GetButtonDown("Shot"))
        {
            Debug.Log("Shot pushed!");
        }
    }
}
