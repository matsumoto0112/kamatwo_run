using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySaveSystem : MonoBehaviour
{
    private static readonly string SaveRootPath = Application.dataPath + "/";

    public static void Save<T>(T saveData, string filename)
    {
        using (FileStream fs = new FileStream(SaveRootPath + filename, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, saveData);
        }

        Debug.Log("[Saved]" + saveData);
    }

    public static T Load<T>(string filename)
    {
        T obj = default(T);
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
