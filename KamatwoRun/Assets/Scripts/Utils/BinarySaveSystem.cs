using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySaveSystem : MonoBehaviour
{
    //保存するルートフォルダパス
    private static readonly string SaveRootPath = Application.dataPath + "/";

    /// <summary>
    /// バイナリデータとして保存する
    /// </summary>
    /// <typeparam name="T">保存する型</typeparam>
    /// <param name="saveData">保存するデータ</param>
    /// <param name="filename">ファイル名</param>
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
    /// バイナリデータとして読み込む
    /// </summary>
    /// <typeparam name="T">読み込む型</typeparam>
    /// <param name="filename">ファイル名</param>
    /// <returns>読み込んだデータを返す
    /// 存在しなければデフォルトの値を返す</returns>
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
