using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //パーティクル
    [SerializeField]
    private GameObject[] particle;
    private static GameManager particles;

    private const string SOUND_OBJECT_NAME = "GameManager";
    private static GameManager singletonInstance = null;

    public static GameManager instance = null;
    private void Awake()
    {
        //ゲーム開始時にGameManagerをinstanceに指定
        if (instance == null)
        {
            instance = this;
            //このオブジェクト以外にGameManagerが存在する時
        }
        else if (instance != this)
        {
            //このオブジェクトを破壊する
            Destroy(gameObject);
        }
        //シーン遷移時にこのオブジェクトを受け継ぐ
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
