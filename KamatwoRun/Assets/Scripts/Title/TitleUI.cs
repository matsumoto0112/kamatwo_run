using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    private Text descText;
    [SerializeField]
    private string[] descMessage;

    [SerializeField]
    private Image[] beforeSelectImages;

    [SerializeField]
    private Image[] afterSelectImages;

    private int currentSelectIndex = 0;

    private bool isRunningCoroutine = false;
    private Coroutine imageBoldCoroutine;
    private Vector3 defaultSelectableImageScale;

    [SerializeField]
    private SoundManager soundManager;
    [SerializeField, AudioSelect(SoundType.BGM)]
    private string bgmName;
    [SerializeField, AudioSelect(SoundType.SE)]
    private string selectSeName;
    [SerializeField, AudioSelect(SoundType.SE)]
    private string decisionSeName;
    private bool itemSelectable = true;

    private void Start()
    {
        soundManager.FadeOutBGM();
        soundManager.PlayBGM(bgmName);
        defaultSelectableImageScale = afterSelectImages[0].rectTransform.localScale;
        Selection(currentSelectIndex);
    }

    private void Selection(int index)
    {
        Assert.IsTrue(0 <= index && index < descMessage.Length);
        if (isRunningCoroutine)
        {
            StopCoroutine(imageBoldCoroutine);
            isRunningCoroutine = false;
        }

        descText.text = descMessage[index];

        foreach (var item in afterSelectImages)
        {
            item.enabled = false;
        }
        foreach (var item in beforeSelectImages)
        {
            item.enabled = true;
        }
        beforeSelectImages[index].enabled = false;
        afterSelectImages[index].enabled = true;

        imageBoldCoroutine = StartCoroutine(ImageBold(afterSelectImages[index]));
        soundManager.PlaySE(selectSeName);
    }

    private void Update()
    {
        if (!itemSelectable) return;

        int selectableItemCount = descMessage.Length;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSelectIndex += 1;
            currentSelectIndex = (currentSelectIndex + selectableItemCount) % selectableItemCount;
            Selection(currentSelectIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSelectIndex -= 1;
            currentSelectIndex = (currentSelectIndex + selectableItemCount) % selectableItemCount;
            Selection(currentSelectIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            soundManager.PlaySE(decisionSeName);
            itemSelectable = false;
            switch (currentSelectIndex)
            {
                case 0:
                    GameDataStore.Instance.ResetPlayDatas();
                    GameDataStore.Instance.PlayedMode = PlayMode.Weekday;
                    SceneManager.LoadScene("Main");
                    break;
                case 1:
                    GameDataStore.Instance.ResetPlayDatas();
                    GameDataStore.Instance.PlayedMode = PlayMode.Holiday;
                    SceneManager.LoadScene("Main");
                    break;
                default:
                    Application.Quit(0);
                    break;
            }
        }

    }

    private IEnumerator ImageBold(Image target)
    {
        const float kSpeed = 10.0f;
        float time = 0.0f;
        Vector3 defaultScale = defaultSelectableImageScale;
        Vector3 add = Vector3.zero;
        isRunningCoroutine = true;
        while (true)
        {
            time += kSpeed * Time.deltaTime;
            float f = Mathf.Sin(time) * 0.5f + 0.5f;
            add = new Vector3(f, f, f);
            target.rectTransform.localScale = defaultScale + add;
            yield return null;
        }
    }
}
