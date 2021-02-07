using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCanvas : MonoBehaviour
{
    [SerializeField]
    private Image frameImage = null;
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private Image gameOverImage = null;
    [SerializeField]
    private Image goalImage = null;

    [SerializeField]
    private EventTextDataTable eventTextDataTable = null;

    private Timer goalEventTimer;
    private Timer gameOverEventTimer;
    private float gameOverImageY = 0.0f;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        goalEventTimer = new Timer(2.0f);
        gameOverEventTimer = new Timer(2.0f);
        GameStartEventInitialize();
        GoalEventInitialize();
        GameOverEventInitialize();
    }

    #region GameStartEvent

    /// <summary>
    /// ゲームスタート時に発生する
    /// イベントで使用するUIの初期化
    /// </summary>
    public void GameStartEventInitialize()
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
    #endregion

    #region GameOverEvent

    public void GameOverEventInitialize()
    {
        gameOverImage.rectTransform.localPosition = new Vector3(0.0f, 290.0f, 0.0f);
        gameOverImageY = gameOverImage.rectTransform.localPosition.y;
    }

    public bool UpdateGameOverImage()
    {
        if(gameOverEventTimer.IsTime() == true)
        {
            return true;
        }
        gameOverEventTimer.UpdateTimer();

        float y = BackOut(gameOverEventTimer.CurrentTime, gameOverEventTimer.LimitTime, gameOverImageY, 0.0f,1.0f);
        gameOverImage.rectTransform.localPosition = new Vector3(0.0f, y, 0.0f);
        return false;
    }

    #endregion

    #region GoalEvent

    public void GoalEventInitialize()
    {
        goalEventTimer.Initialize();
        goalImage.rectTransform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        goalImage.rectTransform.localPosition = Vector3.zero;
    }

    public bool UpdateGoalImage()
    {
        if (goalEventTimer.IsTime() == true)
        {
            return true;
        }
        goalEventTimer.UpdateTimer();
        float x = BackOut(goalEventTimer.CurrentTime, goalEventTimer.LimitTime, 0.0f, 1.0f, 1.0f);
        float y = BackOut(goalEventTimer.CurrentTime, goalEventTimer.LimitTime, 0.0f, 1.0f, 1.0f);

        goalImage.transform.localScale = new Vector3(x, y, 1.0f);

        return false;
    }

    #endregion

    public float BackOut(float ct, float et, float start, float end, float s)
    {
        if (ct > et)
            return end;

        ct = ct / et - 1;
        end -= start;

        return end * (ct * ct * ((s + 1.0f) * ct + s) + 1.0f) + start;
    }

    /// <summary>
    /// 二乗
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private float Quad(float t) => t * t;

    public float BounceIn(float ct, float et, float start, float end)
    {
        if (ct > et)
            return end;

        end -= start;
        return end - BounceOut(et - ct, et, 0.0f, end) + start;
    }

    public float BounceOut(float ct, float et, float start, float end)
    {
        if (ct > et)
            return end;

        end -= start;
        ct /= et;

        const float d1 = 2.75f;
        const float n1 = 7.5625f;

        if (ct < 1.0f / d1)
        {
            return end * (n1 * Quad(ct)) + start;
        }
        else if (ct < 2.0f / d1)
        {
            ct -= 1.5f / d1;
            return end * (n1 * Quad(ct) + 0.75f) + start;
        }
        else if (ct < 2.5f / d1)
        {
            ct -= 2.25f / d1;
            return end * (n1 * Quad(ct) + 0.9375f) + start;
        }
        else
        {
            ct -= 2.625f / d1;
            return end * (n1 * Quad(ct) + 0.984375f) + start;
        }
    }

    public float BounceInOut(float ct, float et, float start, float end)
    {
        if (ct > et)
            return end;

        if (ct < et / 2.0f)
            return BounceIn(ct * 2.0f, et, 0.0f, end - start) * 0.5f + start;
        else
            return BounceOut(ct * 2.0f - et, et, 0.0f, end - start) * 0.5f + start + (end - start) * 0.5f;
    }

}
