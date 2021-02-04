using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneLauncherWindow : EditorWindow
{
    private List<SceneAsset> sceneNames;
    private Vector2 scrollPosition = Vector2.zero;

    //Ctrl + Alt + s
    [MenuItem("Tools/Smy/Scene Launcher %&s")]
    static void Open()
    {
        GetWindow<SceneLauncherWindow>("SceneLauncher");
    }

    private void OnGUI()
    {
        if (sceneNames == null)
            RegistrationSceneName();

        if (sceneNames.Count <= 0)
        {
            EditorGUILayout.LabelField("BuildSettingsにシーンが登録されていません");
            return;
        }

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (var s in sceneNames)
        {
            if (GUILayout.Button(s.name))
            {
                bool sceneChangeFlag = true;
                if (EditorSceneManager.GetSceneAt(0).isDirty)
                {
                    sceneChangeFlag = EditorUtility.DisplayDialog(
                        "",
                        "シーンが保存されていません。保存しますか?",
                        "Save",
                        "Don't Save");

                    if (sceneChangeFlag)
                    {
                        EditorSceneManager.SaveOpenScenes();
                        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(s));
                        Close();
                    }
                }
                else
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(s));
                    Close();
                }
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUI.EndDisabledGroup();
    }

    private void RegistrationSceneName()
    {
        //中身のクリア
        sceneNames = new List<SceneAsset>();
        sceneNames = AssetDatabase.FindAssets("t:SceneAsset")
             .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
             .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)))
             .Where(obj => obj != null)
             .Select(obj => (SceneAsset)obj).ToList();
    }
}
