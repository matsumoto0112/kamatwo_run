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

    public List<Image> hpImageList { get; private set; }

    public void OnUpdate()
    {
        if (prevHP == playerStatus.HP)
        {
            return;
        }

        Destroy(hpImageList[playerStatus.HP].gameObject);
        hpImageList.RemoveAt(playerStatus.HP);
        prevHP = playerStatus.HP;
    }

    public void Initialize()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStatus>();
        hpImageList = new List<Image>();
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
    }
}
