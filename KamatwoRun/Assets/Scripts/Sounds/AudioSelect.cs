using UnityEngine;
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#endif
public enum SoundType
{
    BGM,
    SE
}
class AudioSelectAttribute : PropertyAttribute
{
    public SoundType type;

    public AudioSelectAttribute(SoundType type)
    {
        this.type = type;
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AudioSelectAttribute))]
public class AudioSelectEditor : PropertyDrawer
{
    //Scene名のList
    List<string> AllSceneName
    {
        get
        {
            List<string> sceneNames = new List<string>();
            //BuildSettingsからSceneのPathを読み込む
            List<string> AllPaths = (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToList();
            //PathからScene名を切り出す
            foreach (string x in AllPaths)
            {
                int slash = x.LastIndexOf("/");
                int dot = x.LastIndexOf(".");
                sceneNames.Add(x.Substring(slash + 1, dot - slash - 1));
            }
            return sceneNames;
        }
    }

    private List<string> AllBGMList
    {
        get{
            var resourceList = Resources.LoadAll("Audio/BGM/").ToList();
            List<string> bgmList = new List<string>();
            foreach (var b in resourceList)
                bgmList.Add(b.name);

            return bgmList;
        }
    }

    private List<string> AllSEList
    {
        get
        {
            var resourceList = Resources.LoadAll("Audio/SE/").ToList();
            List<string> bgmList = new List<string>();
            foreach (var b in resourceList)
                bgmList.Add(b.name);

            return bgmList;
        }
    }

    //ドロップダウンメニューの作成
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AudioSelectAttribute audioType = attribute as AudioSelectAttribute;
        List<string> list = new List<string>();
        if (audioType.type == SoundType.BGM)
            list = AllBGMList;
        else if (audioType.type == SoundType.SE)
            list = AllSEList;
        var selectedIndex = list.FindIndex(item => item.Equals(property.stringValue));
        if (selectedIndex == -1)
        {
            selectedIndex = list.FindIndex(item => item.Equals(list[0]));
        }

        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, list.ToArray());

        property.stringValue = list[selectedIndex];
    }
}
#endif

