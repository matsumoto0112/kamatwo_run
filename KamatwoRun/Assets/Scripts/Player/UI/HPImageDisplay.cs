using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPImageDisplay : MonoBehaviour
{
    [SerializeField]
    private RectTransform hpImageObject = null;
    private PlayerStatus playerStatus = null;
    private int prevHP = 0;
    private bool isCreate = false;

    public List<Image> hpImageList { get; private set; }

    private void Start()
    {
        isCreate = false;
        StartCoroutine(Initialize());
    }

    private void Update()
    {
        if (isCreate == false || prevHP == playerStatus.HP)
        {
            return;
        }

        Destroy(hpImageList[playerStatus.HP].gameObject);
        hpImageList.RemoveAt(playerStatus.HP);
        prevHP = playerStatus.HP;
    }

    private IEnumerator Initialize()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStatus>();
        hpImageList = new List<Image>();
        yield return new WaitWhile(() => playerStatus.IsCreate == true);
        float x = hpImageObject.GetComponent<Image>().rectTransform.sizeDelta.x;
        for (int i = 1; i < playerStatus.HP; i++)
        {
            Image image = Instantiate(hpImageObject).GetComponent<Image>();
            image.rectTransform.parent = transform;
            image.rectTransform.localPosition = new Vector3(hpImageObject.localPosition.x + (x * i), hpImageObject.localPosition.y, 0.0f);
            image.rectTransform.localScale = Vector3.one;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            hpImageList.Add(transform.GetChild(i).GetComponent<Image>());
        }

        prevHP = playerStatus.HP;
        isCreate = true;
    }
}
