using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    //�������[�h
    Weekday,
    //�x�����[�h
    Holiday,
}

public static class PlayModeExtend
{
    /// <summary>
    /// �v���C���[�h���e�L�X�g�ɕϊ�����
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string PlayModeText(this PlayMode mode)
    {
        var kNames = new Dictionary<PlayMode, string>() {
            {PlayMode.Weekday,"�������[�h" },
            {PlayMode.Holiday,"�x�����[�h" },
        };

        return kNames[mode];
    }
}