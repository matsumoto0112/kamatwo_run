using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    //�@�|�[�Y�������ɕ\������UI
    [SerializeField]
  //  private GameObject pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���U���g�V�[���Ɉڍs
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("ResultScene");
        }
        ////�|�[�Y�N��
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    //�@�|�[�YUI�̃A�N�e�B�u�A��A�N�e�B�u��؂�ւ�
        //    pauseUI.SetActive(!pauseUI.activeSelf);

        //    //�@�|�[�YUI���\������Ă鎞�͒�~
        //    if (pauseUI.activeSelf)
        //    {
        //        Time.timeScale = 0f;
        //        //�@�|�[�YUI���\������ĂȂ���Βʏ�ʂ�i�s
               
        //    }
        //    else
        //    {
        //        Time.timeScale = 1f;
        //        pauseUI.SetActive(pauseUI.activeSelf);
            
        //    }
        
        //}
    }
}
