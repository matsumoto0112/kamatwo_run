using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReserveNextSubStageShapeTest : MonoBehaviour
{
    [SerializeField]
    private StageManager manager;

    public void OnClick()
    {
        manager.ReserveNextSubstageShapeType(SubStageShapeType.L_Shape);
    }
}
