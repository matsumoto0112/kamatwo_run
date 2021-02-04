using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapController : MonoBehaviour
{
    [SerializeField]
    private StageParameter stageParameter = null;
    [SerializeField]
    private GameObject startPosition = null;
    [SerializeField]
    private GameObject endPosition = null;
    [SerializeField]
    private Image playerIcon = null;

    private int maxWaveCount = 0;
    private float distance = 0.0f;

    public void Initialize()
    {
        //�x�����[�h�̏ꍇ
        if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday)
        {
            gameObject.SetActive(false);
            return;
        }

        //�S�[������̂ɕK�v�ȃE�F�[�u��
        maxWaveCount = stageParameter.stageGoalWaveNum;
        playerIcon.rectTransform.position = startPosition.transform.position;
        distance = endPosition.transform.position.y - startPosition.transform.position.y;
    }

    public void OnUpdate()
    {
        if (GameDataStore.Instance.PlayedMode == PlayMode.Holiday)
        {
            return;
        }

        float coef = GameDataStore.Instance.WaveCount / (maxWaveCount * 1.0f);
        float y = distance * coef;
        y = Mathf.Clamp(y, startPosition.transform.position.y, endPosition.transform.position.y);
        playerIcon.transform.position = startPosition.transform.position + new Vector3(0.0f, y, 0.0f);
    }
}
