using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventTextDataTable", menuName = "ScriptableObject/EventTextDataTable")]
public class EventTextDataTable : ScriptableObject
{
    [SerializeField]
    private EventTextData textData = null;

    public EventTextData GetEventTextData()
    {
        return textData;
    }
}

[System.Serializable]
public class EventTextData
{
    public string weekDayFirstText = "";
    public string weekDaySecondText = "";

    public string holiDayFirstText = "";
    public string holiDaySecondText = "";
}
