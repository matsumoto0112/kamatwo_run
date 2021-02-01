using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySaveSystem : MonoBehaviour
{
    //�ۑ����郋�[�g�t�H���_�p�X
    private static readonly string SaveRootPath = Application.dataPath + "/";

    /// <summary>
    /// �o�C�i���f�[�^�Ƃ��ĕۑ�����
    /// </summary>
    /// <typeparam name="T">�ۑ�����^</typeparam>
    /// <param name="saveData">�ۑ�����f�[�^</param>
    /// <param name="filename">�t�@�C����</param>
    public static void Save<T>(T saveData, string filename)
    {
        using (FileStream fs = new FileStream(SaveRootPath + filename, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, saveData);
        }

        Debug.Log("[Saved]" + saveData);
    }

    /// <summary>
    /// �o�C�i���f�[�^�Ƃ��ēǂݍ���
    /// </summary>
    /// <typeparam name="T">�ǂݍ��ތ^</typeparam>
    /// <param name="filename">�t�@�C����</param>
    /// <returns>�ǂݍ��񂾃f�[�^��Ԃ�
    /// ���݂��Ȃ���΃f�t�H���g�̒l��Ԃ�</returns>
    public static T Load<T>(string filename) where T : new()
    {
        T obj = new T();
        using (FileStream fs = new FileStream(SaveRootPath + filename, FileMode.OpenOrCreate, FileAccess.Read))
        {
            if (fs.Length == 0)
            {
                return obj;
            }
            BinaryFormatter bf = new BinaryFormatter();
            obj = (T)bf.Deserialize(fs);
        }

        Debug.Log("[Loaded]" + obj);
        return obj;
    }
}
