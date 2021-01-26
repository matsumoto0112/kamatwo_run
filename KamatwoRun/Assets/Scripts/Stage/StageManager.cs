using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> subStagePrefabs;

    private GameSpeed gameSpeed;

    //�������ꂽ�T�u�X�e�[�W�Q
    private List<SubStage> subStages;

    private readonly float SubStageUnit = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = GetComponent<GameSpeed>();
        subStages = new List<SubStage>();

        //�ŏ��̃X�e�[�W���͌Œ�̂��̂��g�p����
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
            //�v���C���[���O�ɂ������̂Ŕ͈͊O�ɂȂ������߁A�폜����
            Destroy(frontStage.gameObject);
            subStages.RemoveAt(0);

            //�V�����ǉ�����
            SpawnNextSubStage();
        }
    }

    /// <summary>
    /// ���̃T�u�X�e�[�W�𐶐�����
    /// </summary>
    private void SpawnNextSubStage()
    {
        //����ł͂܂������̃X�e�[�W�݂̂Ȃ̂ŉ����l�����ɐ�������
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

        Debug.Log("���ׂ������W�������ς݂̃X�e�[�W���ɂ���܂���");
        return Vector3.back;
    }
}
