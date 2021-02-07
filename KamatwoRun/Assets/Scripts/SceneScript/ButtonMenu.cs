using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonMenu : MonoBehaviour
{
    //ボタン数
    [SerializeField] private Button[] button = null;
    [SerializeField] private bool isStartButton = false;

    private int count;


 　GameObject goalButton;
    GameObject endButton;
    GameObject  ranking;
    GameObject exitButton;


    public Text target;
　　public Text text2;


    // Start is called before the first frame update
    private void Start()
    {
        button[0].Select();
        count = 0;
     
        //ボタン
        endButton = GameObject.Find("TitleCanvas/Panel/GoalGameButton");
        goalButton = GameObject.Find("TitleCanvas/Panel/EndlessGameButton");
        ranking = GameObject.Find("TitleCanvas/Panel/Rannking");
        exitButton = GameObject.Find("TitleCanvas/Panel/ExitButton");


        endButton = EventSystem.current.firstSelectedGameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isStartButton)
        {

          
              
                SelectButton();
            
        }
        else
        {
            button[count].Select();
           

        }

        //テキストを消す。
        if (goalButton != EventSystem.current.currentSelectedGameObject && ranking != EventSystem.current.currentSelectedGameObject)
        {
            target.gameObject.SetActive(true);
            text2.gameObject.SetActive(false);
        }
        else if (exitButton != EventSystem.current.currentSelectedGameObject && exitButton != EventSystem.current.currentSelectedGameObject)
        {
            target.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
        }
        //エンドレス。
        if (endButton != EventSystem.current.currentSelectedGameObject)
        {
          
            text2.text = "かまトゥが蒲田で餃子をたべまくるエンドレスモード。";
         
           
        }

        //ランキングモード。

        if (goalButton != EventSystem.current.currentSelectedGameObject)
        {
            
            text2.text = "それぞれのモードのとくてんランキング。";
         

        }

        //ゲーム終了
        if (ranking != EventSystem.current.currentSelectedGameObject)
        {
         
            target.text = "ゲームを終わりにする。";

            


        }
        //平日
        if (exitButton != EventSystem.current.currentSelectedGameObject)
        {
            target.text = "かまトゥがギョーザを食べつつ工学院を目指すモード。";
            //ゲーム終了
          
        }
        

    }

    private void SelectButton()
    {
        //上下移動
        var moveY = Input.GetAxis("Vertical");

        if (moveY > 0.5f && count > 0)
        {
            //下へ
            count--;
            button[count].Select();
          
        
        

        }
        else if (moveY < -0.5f && button.Length - 1 > count)
        {
            //上
            count++;
            button[count].Select();
         
        }

      
   

    }





}
