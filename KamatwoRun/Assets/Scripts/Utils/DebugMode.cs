using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 上書きするゲームモード
/// </summary>
enum OverrideGameMode
{
    NONE,
    Weekday,
    Holiday,
}

public class DebugMode : MonoBehaviour
{
    [SerializeField, Header("ゲームモードを上書きするときに使用")]
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
