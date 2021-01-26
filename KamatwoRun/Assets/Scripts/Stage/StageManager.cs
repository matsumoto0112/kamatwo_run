using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> subStagePrefabs;

    private GameSpeed gameSpeed;

    //生成されたサブステージ群
    private List<SubStage> subStages;

    private readonly float SubStageUnit = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = GetComponent<GameSpeed>();
        subStages = new List<SubStage>();

        //最初のステージ情報は固定のものを使用する
        {
            GameObject newObject = Instantiate(subStagePrefabs[0], new Vector3(0, 0, SubStageUnit / 2.0f - 5.0f), Quaternion.identity);
            subStages.Add(newObject.GetComponent<SubStage>());
        }

        for (int i = 0; i < 3; i++)
        {
            SpawnNextSubStage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scrollDirection = GetForegroundDirection(Vector3.zero);
        float speed = gameSpeed.Speed * Time.deltaTime;
        foreach (var st in subStages)
        {
            st.Move(scrollDirection, speed);
        }

        SubStage frontStage = subStages.First();
        if (!frontStage.IsInsideCamera)
        {
            //プレイヤーより前にいったので範囲外になったため、削除する
            Destroy(frontStage.gameObject);
            subStages.RemoveAt(0);

            //新しく追加する
            SpawnNextSubStage();
        }
    }

    /// <summary>
    /// 次のサブステージを生成する
    /// </summary>
    private void SpawnNextSubStage()
    {
        //現状ではまっすぐのステージのみなので何も考えずに生成する
        SubStage prevStage = subStages.Last();
        Vector3 spawnPosition = prevStage.transform.position + new Vector3(0, 0, SubStageUnit);
        GameObject newObject = Instantiate(subStagePrefabs[0], spawnPosition, Quaternion.identity);
        SubStage newSubStage = newObject.GetComponent<SubStage>();
        newSubStage.SpawnObjects(5);
        subStages.Add(newSubStage);
    }

    public Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        foreach (var st in subStages)
        {
            if (st.IsInArea(checkPosition))
            {
                return st.GetForegroundDirection(checkPosition);
            }
        }

        Debug.Log("調べたい座標が生成済みのステージ内にありません");
        return Vector3.back;
    }
}
