using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultScore : MonoBehaviour
{
   //public static int score = 0;
    public Text ScroeText;
    public GameObject score;
   
    // Start is called before the first frame update
    void Start()
    {
      //  score =scoreInfo.score;
        ScroeText.text = string.Format("Score:{0}", score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
