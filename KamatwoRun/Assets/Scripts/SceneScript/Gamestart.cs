using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gamestart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //ゲームシーン
    public void ScenenGame()
    {
        SceneManager.LoadScene("Main");
    }
    //ゴールゲームシーン
    public void GoalSceneGame()
    {
        SceneManager.LoadScene("StageObjectTest");
    }
}
