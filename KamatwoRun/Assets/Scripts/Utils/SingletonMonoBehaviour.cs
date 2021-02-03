using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExLib
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private bool DontDestroyFlag = true;
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //型情報の取得
                    Type t = typeof(T);
                    //Hierarcyにある型を検索
                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                        Debug.LogWarning(t + "をアタッチしているオブジェクトはありません");
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            //シーン上に同じものが存在すかどうか
            if (this != Instance)
            {
                Destroy(this);
                Debug.LogWarning(
                    typeof(T) +
                    "はすでに他のGameObjectにアタッチされているため、Componentを破棄しました。" +
                    "アタッチされているGameObjectは" + Instance.gameObject.name + "です。");
                return;
            }
            if (DontDestroyFlag)
                DontDestroyOnLoad(this.gameObject);
        }
    }
}
