using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonMenu : MonoBehaviour
{
    //�{�^����
    [SerializeField] private Button[] button = null;
    [SerializeField] private bool isStartButton = false;

    private int count;


 �@GameObject goalButton;
    GameObject endButton;
    GameObject  ranking;
    GameObject exitButton;


    public Text target;
�@�@public Text text2;


    // Start is called before the first frame update
    private void Start()
    {
        button[0].Select();
        count = 0;
     
        //�{�^��
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

        //�e�L�X�g�������B
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
        //�G���h���X�B
        if (endButton != EventSystem.current.currentSelectedGameObject)
        {
          
            text2.text = "���܃g�D�����c���L�q�����ׂ܂���G���h���X���[�h�B";
         
           
        }

        //�����L���O���[�h�B

        if (goalButton != EventSystem.current.currentSelectedGameObject)
        {
            
            text2.text = "���ꂼ��̃��[�h�̂Ƃ��Ă񃉃��L���O�B";
         

        }

        //�Q�[���I��
        if (ranking != EventSystem.current.currentSelectedGameObject)
        {
         
            target.text = "�Q�[�����I���ɂ���B";

            


        }
        //����
        if (exitButton != EventSystem.current.currentSelectedGameObject)
        {
            target.text = "���܃g�D���M���[�U��H�ׂH�w�@��ڎw�����[�h�B";
            //�Q�[���I��
          
        }
        

    }

    private void SelectButton()
    {
        //�㉺�ړ�
        var moveY = Input.GetAxis("Vertical");

        if (moveY > 0.5f && count > 0)
        {
            //����
            count--;
            button[count].Select();
          
        
        

        }
        else if (moveY < -0.5f && button.Length - 1 > count)
        {
            //��
            count++;
            button[count].Select();
         
        }

      
   

    }





}
