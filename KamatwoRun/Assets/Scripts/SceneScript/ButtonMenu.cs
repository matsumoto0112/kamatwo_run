using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonMenu : MonoBehaviour
{
    //ボタン数
    [SerializeField] private Button[] button = null;
    [SerializeField] private bool isStartButton = false;


    private int count;
    private float timer;

    // Start is called before the first frame update
   private void Start()
    {
        button[0].Select();
        count = 0;
        timer = 0;
    }

    // Update is called once per frame
  private  void Update()
    {
        if (isStartButton)
        {
          
              
                SelectButton();
        
        }
        else
        {
            button[count].Select();
        }


    }
    private void SelectButton()
    {
        //上下移動
        var moveY = Input.GetAxis("Vertical");
       
        if (moveY > 0.4f && count > 0)
        {
            //下へ
            count--;
            button[count].Select();
        }
        else if (moveY < -0.4f && button.Length - 1 > count)
        {
            //上
            count++;
            button[count].Select();
        }
    }



}
