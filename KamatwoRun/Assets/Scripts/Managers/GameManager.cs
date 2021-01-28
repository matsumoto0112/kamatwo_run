using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�p�[�e�B�N��
    [SerializeField]
    private GameObject[] particle;
    private static GameManager particles;

    private const string SOUND_OBJECT_NAME = "GameManager";
    private static GameManager singletonInstance = null;

    public static GameManager instance = null;
    private void Awake()
    {
        //�Q�[���J�n����GameManager��instance�Ɏw��
        if (instance == null)
        {
            instance = this;
            //���̃I�u�W�F�N�g�ȊO��GameManager�����݂��鎞
        }
        else if (instance != this)
        {
            //���̃I�u�W�F�N�g��j�󂷂�
            Destroy(gameObject);
        }
        //�V�[���J�ڎ��ɂ��̃I�u�W�F�N�g���󂯌p��
        DontDestroyOnLoad(gameObject);
      
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static GameManager Effecta
    {

        get
        {
            if (!particles)
            {
                GameObject obj = new GameObject(SOUND_OBJECT_NAME);
                singletonInstance = obj.AddComponent<GameManager>();
                DontDestroyOnLoad(obj);
            }
            return singletonInstance;
        }

     
    }
}
