using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class BuildWindow : EditorWindow
{
    [MenuItem("Utility/Build Project")]
    public static void ShowWindow()
    {
        // Show window if there is not already a window
        GetWindow<BuildWindow>("Build");
    }

    private string exportLocation = "C:/Users/frres/source/repos/WorldsOnline/Builds/Current";
    private string sceneLocation = "Assets/BaseGameAssets/Scenes/GameScene.unity";

    private void OnGUI()
    {
        // Get a build location folder
        //GUILayout.Label("Please input the scenes to be exported...", EditorStyles.boldLabel);
        //
        //GUILayout.BeginHorizontal();
        //sceneLocation = EditorGUILayout.TextField("Scene location...", sceneLocation);
        //
        //if (GUILayout.Button("Set location"))
        //{
        //    sceneLocation = EditorUtility.OpenFilePanel("Destination", "", "");
        //}
        //GUILayout.EndHorizontal();

        GUILayout.Label("Please select an export location...", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        exportLocation = EditorGUILayout.TextField("Export location...", exportLocation);

        if (GUILayout.Button("Set location"))
        {
            exportLocation = EditorUtility.OpenFolderPanel("Destination", "", "");
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Build"))
        {
            BuildScript.PerformBuild(new string[] { sceneLocation }, exportLocation);
        }

        if (GUILayout.Button("Build asset bundle"))
        {
            BuildScript.PerformAssetBundleBuild(exportLocation);
        }
    }
}

#endif