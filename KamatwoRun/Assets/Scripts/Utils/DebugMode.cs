using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �㏑������Q�[�����[�h
/// </summary>
enum OverrideGameMode
{
    NONE,
    Weekday,
    Holiday,
}

public class DebugMode : MonoBehaviour
{
    [SerializeField, Header("�Q�[�����[�h���㏑������Ƃ��Ɏg�p")]
    private OverrideGameMode overrideGame = OverrideGameMode.NONE;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        switch (overrideGame)
        {
            case OverrideGameMode.Weekday:
                GameDataStore.Instance.PlayedMode = PlayMode.Weekday;
                break;
            case OverrideGameMode.Holiday:
                GameDataStore.Instance.PlayedMode = PlayMode.Holiday;
                break;
        }
#endif    
    }
}
