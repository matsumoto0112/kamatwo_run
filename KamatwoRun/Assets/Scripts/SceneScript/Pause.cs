using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    //　ポーズした時に表示するUI
    [SerializeField]
  //  private GameObject pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //リザルトシーンに移行
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("ResultScene");
        }
        ////ポーズ起動
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    //　ポーズUIのアクティブ、非アクティブを切り替え
        //    pauseUI.SetActive(!pauseUI.activeSelf);

        //    //　ポーズUIが表示されてる時は停止
        //    if (pauseUI.activeSelf)
        //    {
        //        Time.timeScale = 0f;
        //        //　ポーズUIが表示されてなければ通常通り進行
               
        //    }
        //    else
        //    {
        //        Time.timeScale = 1f;
        //        pauseUI.SetActive(pauseUI.activeSelf);
            
        //    }
        
        //}
    }
}
