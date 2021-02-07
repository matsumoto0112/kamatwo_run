using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapController : MonoBehaviour
{
    [SerializeField]
    private EventManager eventManager = null;
    [SerializeField]
    private StageParameter stageParameter = null;
    [SerializeField]
    private RectTransform startPosition = null;
    [SerializeField]
    private RectTransform endPosition = null;
    [SerializeField]
    private Image playerIcon = null;

    private int maxWaveCount = 0;
    private float distance = 0.0f;

    public void Initialize()
    {
        //休日モードの場合
        if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday)
        {
            gameObject.SetActive(false);
            return;
        }

        //ゴールするのに必要なウェーブ数
        maxWaveCount = stageParameter.stageGoalWaveNum;
        playerIcon.rectTransform.localPosition = startPosition.localPosition;
        distance = Mathf.Abs(endPosition.localPosition.y - startPosition.localPosition.y);
    }

    public void OnUpdate()
    {
        if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday ||
            eventManager.StartEventFlag == false)
        {
            return;
        }

        float coef = GameDataStore.Instance.WaveCount / (maxWaveCount * 1.0f);
        float y = (distance * coef) - (distance / 2.0f);
        y = Mathf.Clamp(y, startPosition.localPosition.y, endPosition.localPosition.y);
        Debug.Log(y);
        playerIcon.transform.localPosition = new Vector3(playerIcon.transform.localPosition.x, y, playerIcon.transform.localPosition.z);
    }
}
