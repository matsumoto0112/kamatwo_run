using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanePositions : MonoBehaviour
{
    public List<Transform> LanePositionList { get; private set; }

    public void Initialize()
    {
        LanePositionList = new List<Transform>();
        for(int i = 0;i < transform.childCount; i++)
        {
            LanePositionList.Add(transform.GetChild(i));
        }
    }
}
