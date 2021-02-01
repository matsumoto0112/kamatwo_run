using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    //平日モード
    Weekday,
    //休日モード
    Holiday,
}

public static class PlayModeExtend
{
    /// <summary>
    /// プレイモードをテキストに変換する
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string PlayModeText(this PlayMode mode)
    {
        var kNames = new Dictionary<PlayMode, string>() {
            {PlayMode.Weekday,"平日モード" },
            {PlayMode.Holiday,"休日モード" },
        };

        return kNames[mode];
    }
}