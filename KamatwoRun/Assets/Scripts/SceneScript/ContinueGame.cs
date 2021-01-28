using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour
{

    [SerializeField] private GameObject pauseUI;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickContinue()
    {

        if (Time.timeScale == 0)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);


            Time.timeScale = 1f;

        }
        else
        {
            Time.timeScale = 0f;
        }
        }
}
