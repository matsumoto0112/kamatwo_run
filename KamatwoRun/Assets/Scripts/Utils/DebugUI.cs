using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    private Canvas debugUICanvas;
    [SerializeField]
    private Text frameRateText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            debugUICanvas.enabled = !debugUICanvas.enabled;
        }

        frameRateText.text = $"FrameRate: {1.0 / Time.deltaTime:0.00}";
    }
}
