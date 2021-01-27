using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public struct SpawnObjectInfo
{
    public SpawnObjectType type;
    public GameObject prefab;
}

public class StageObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<SpawnObjectInfo> prefabs;
    [SerializeField]
    private StageParameter stageParameter;

    private List<GameObject> spawnedObjects;

    private void Start()
    {
        spawnedObjects = new List<GameObject>();
    }

    /// <summary>
    /// �X�|�[������ɐ���������X�|�[�������鏈��
    /// </summary>
    /// <param name="type"></param>
    public GameObject SpawnIfSucceed(SpawnObjectType type, Vector3 basePoint, int laneNum)
    {
        GameObject prefab = GetPrefabFromType(type);
        PlacementType placementType = prefab.GetComponent<StageObject>().GetPlacementType();

        //�����L���I�u�W�F�N�g�̏ꍇ�A���[����1�ԁi�^�񒆁j�o�Ȃ��Ɛ����ł��Ȃ�
        if (placementType == PlacementType.Wide && laneNum != 1) { return null; }

        Vector3 offset = new Vector3(0, GetYOffset(placementType), 0);
        Vector3 spawnPosition = basePoint + offset;

        //�X�|�[���e�X�g������
        //�܂��A�����Ȓl�����ׂč폜����
        spawnedObjects.RemoveAll(obj => obj == null);

        bool succeeded = true;
        //�������W�ɐ�������͖̂����ɂ���
        foreach (var obj in spawnedObjects)
        {
            PlacementType obj_pracementType = obj.GetComponent<StageObject>().GetPlacementType();
            if (Vector3.Distance(obj.transform.position, spawnPosition) < Mathf.Epsilon) succeeded = false;
            //���̍L���I�u�W�F�N�g�̏ꍇ�A�����Ŕ��肷��
            if (obj_pracementType == PlacementType.Wide)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < stageParameter.wideObjectJudgeRadius) { succeeded = false; }
            }
            //�����̂���I�u�W�F�N�g�̏ꍇ�Axz���W�Ŕ��肷��
            if (obj_pracementType == PlacementType.High)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < Mathf.Epsilon) { succeeded = false; }
            }
        }

        //���̍L���I�u�W�F�N�g��z�u����Ƃ��́A���E�̃��[�������ׂ�
        if (placementType == PlacementType.Wide)
        {
            foreach (var obj in spawnedObjects)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < stageParameter.wideObjectJudgeRadius) { succeeded = false; }
                //���̍L���I�u�W�F�N�g���m�͂����Ƌ������J����
                if (obj.GetComponent<StageObject>().GetPlacementType() == PlacementType.Wide && Vector3.Distance(a, b) < stageParameter.wideObjectJudgeRadius * 3.0f) { succeeded = false; }
            }
        }

        //�����̂���I�u�W�F�N�g��z�u����Ƃ��́Axz���W�Ŕ��肷��
        if (placementType == PlacementType.High)
        {
            foreach (var obj in spawnedObjects)
            {
                Vector3 a = new Vector3(spawnPosition.x, 0.0f, spawnPosition.z);
                Vector3 b = new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z);
                if (Vector3.Distance(a, b) < Mathf.Epsilon) { succeeded = false; }
                //�����̂���I�u�W�F�N�g���m�͗אڂ��Ȃ��悤�ɂ���
                if (obj.GetComponent<StageObject>().GetPlacementType() == PlacementType.High && Vector3.Distance(a, b) < stageParameter.highObjectJudgeDistance) { succeeded = false; }
            }
        }

        if (!succeeded) return null;

        GameObject res = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(res);
        return res;
    }

    /// <summary>
    /// �X�|�[���I�u�W�F�N�g�̎�ނ���v���n�u���擾����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetPrefabFromType(SpawnObjectType type)
    {
        foreach (var p in prefabs)
        {
            if (p.type == type) return p.prefab;
        }

        Assert.IsTrue(false, "���݂��Ȃ�type���I������܂���");
        return null;
    }

    /// <summary>
    /// �z�u�^�C�v����z�u����Ƃ���Y���W�̃I�t�Z�b�g���擾����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private float GetYOffset(PlacementType type)
    {
        switch (type)
        {
            case PlacementType.OnlyGround:
            case PlacementType.Wide:
                return stageParameter.groundPosition_Y;
            case PlacementType.OnlySky:
                return stageParameter.skyPosition_Y;
            case PlacementType.GroundOrSky:
                {
                    if (Random.Range(0.0f, 1.0f) < 0.5f)
                    {
                        return stageParameter.groundPosition_Y;
                    }
                    else
                    {
                        return stageParameter.skyPosition_Y;
                    }
                }
            case PlacementType.High:
                return stageParameter.groundPosition_Y + 1.0f;
            default:
                return 0.0f;
        }
    }
}
