using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Fade : MonoBehaviour
{
   
    //�t�F�[�h�A�E�g�����̊J�n�A�������Ǘ�����t���O
    private bool isFadeOut = false;
    //�t�F�[�h�C�������̊J�n�A�������Ǘ�����t���O
    private bool isFadeIn = true;
    //�����x���ς��X�s�[�h
    float fadeSpeed = 0.75f;
    //��ʂ��t�F�[�h�����邽�߂̉摜���p�u���b�N�Ŏ擾
    public Image fadeImage;
    Color color;
    //�V�[���J�ڂ̂��߂̌^
    string afterScene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        color = Color.black;
        //�V�[���J�ڂ����������ۂɃt�F�[�h�C�����J�n����悤�ɐݒ�
        SceneManager.sceneLoaded += fadeInStart;
    }
    //�V�[���J�ڂ����������ۂɃt�F�[�h�C�����J�n����悤�ɐݒ�
    void fadeInStart(Scene scene, LoadSceneMode mode)
    {
        isFadeIn = true;
    
    }
    /// <summary>
    /// �t�F�[�h�A�E�g�J�n���̉摜��RGBA�l�Ǝ��̃V�[�������w��
    /// </summary>
    /// <param name="red">�摜�̐Ԑ���</param>
    /// <param name="green">�摜�̗ΐ���</param>
    /// <param name="blue">�摜�̐���</param>
    /// <param name="alfa">�摜�̓����x</param>
    /// <param name="nextScene">�J�ڐ�̃V�[����</param>
    public void fadeOutStart(Color color, string nextScene)
    {
        this.color = color;
        SetColor();
        isFadeOut = true;
        afterScene = nextScene;
    }
    // Update is called once per frame
    void Update()
    {
        if (isFadeIn == true)
        {
            //�s�����x�����X�ɉ�����
            color.a -= fadeSpeed * Time.deltaTime;
            //�ύX���������x���摜�ɔ��f������֐����Ă�
            SetColor();
            if (color.a <= 0)
                isFadeIn = false;
        }
        if (isFadeOut == true)
        {
            //�s�����x�����X�ɏグ��
            color.a += fadeSpeed * Time.deltaTime;
            //�ύX���������x���摜�ɔ��f������֐����Ă�
            SetColor();
            if (color.a >= 1)
            {
                isFadeOut = false;
                SceneManager.LoadScene(afterScene);
            }
        }
    }
    //�摜�ɐF��������֐�
    void SetColor()
    {
        fadeImage.color =color;
    }
  
}
