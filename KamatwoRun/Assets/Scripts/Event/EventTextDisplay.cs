using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTextDisplay : MonoBehaviour
{
    [SerializeField]
    private Image frameImage = null;
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private EventTextDataTable eventTextDataTable = null;

    public void Initialize()
    {
        Color color = frameImage.color;
        color.a = 0.0f;
        frameImage.color = color;
        text.text = "";
    }

    public void ChangeAlpha()
    {
        //透明度が1以下なら
        if (frameImage.color.a < 1.0f)
        {
            Color color = frameImage.color;
            color.a = Mathf.Clamp01(color.a + Time.deltaTime);
            frameImage.color = color;
        }
    }

    /// <summary>
    /// アルファ値が位置以上か
    /// </summary>
    /// <returns></returns>
    public bool IsAlpha()
    {
        return frameImage.color.a >= 1.0f;
    }

    public void SetFrameAlpha(float alpha)
    {
        frameImage.color = new Color(frameImage.color.r, frameImage.color.g, frameImage.color.b, alpha);
    }

    public void FirstText()
    {
        if (GameDataStore.Instance.PlayedMode == PlayMode.Weekday)
        {
            text.text = eventTextDataTable.GetEventTextData().weekDayFirstText;
        }
        else if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday)
        {
            text.text = eventTextDataTable.GetEventTextData().holiDayFirstText;
        }
    }

    public void SecondText()
    {
        if (GameDataStore.Instance.PlayedMode == PlayMode.Weekday)
        {
            text.text = eventTextDataTable.GetEventTextData().weekDaySecondText;
        }
        else if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday)
        {
            text.text = eventTextDataTable.GetEventTextData().holiDaySecondText;
        }
    }
}
