using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// Reference: http://tsubakit1.hateblo.jp/entry/2014/08/16/233509

//シーンビューのカメラの座標・回転をほかのカメラにコピーする機能拡張
public class FollowSceneCamera : EditorWindow
{
    [MenuItem("Window/Camera/FollowSceneCamera")]
    static void Init()
    {
        var window = EditorWindow.FindObjectOfType<FollowSceneCamera>();
        if (window != null)
            window.Close();

        window = EditorWindow.CreateInstance<FollowSceneCamera>();
        window.Show();
    }

    public Camera sceneCamera
    {
        get { return SceneView.lastActiveSceneView.camera; }
    }

    void OnGUI()
    {
        foreach (var camera in GameObject.FindObjectsOfType<Camera>())
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(camera.name))
            {
                Undo.RecordObject(camera.transform, "camera");
                camera.transform.position = sceneCamera.transform.position;
                camera.transform.rotation = sceneCamera.transform.rotation;
            }

            if (GUILayout.Button("F", GUILayout.Width(30)))
            {
                Selection.activeGameObject = camera.gameObject;
            }
            GUILayout.EndHorizontal();
        }
    }
}
