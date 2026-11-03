using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class ProjectBootstrap
{
    [MenuItem("Tools/Generate Main Scene")]
    public static void Generate()
    {
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        var go=new GameObject("RepairGame"); go.AddComponent<RepairGame>();
        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene,"Assets/Scenes/Main.unity");
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/Main.unity",true)};
        AssetDatabase.Refresh();
    }
    [InitializeOnLoadMethod]
    static void EnsureScene()
    {
        if(!File.Exists("Assets/Scenes/Main.unity")) Generate();
    }
}