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
        //�����x��1�ȉ��Ȃ�
        if (frameImage.color.a < 1.0f)
        {
            Color color = frameImage.color;
            color.a = Mathf.Clamp01(color.a + Time.deltaTime);
            frameImage.color = color;
        }
    }

    /// <summary>
    /// �A���t�@�l���ʒu�ȏォ
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
