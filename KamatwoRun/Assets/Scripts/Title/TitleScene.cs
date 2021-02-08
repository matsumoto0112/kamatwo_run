using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private Canvas title;

    [SerializeField]
    private Canvas ranking;

    private void Start()
    {
        title.gameObject.SetActive(true);
        ranking.gameObject.SetActive(false);
    }

    public void SwapMode()
    {
        title.gameObject.SetActive(!title.gameObject.activeSelf);
        ranking.gameObject.SetActive(!ranking.gameObject.activeSelf);
    }
}
