using System.Collections;
using UnityEditor;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
[InitializeOnLoad]
#endif

public class AutoSaveEDITOR
{
#if UNITY_EDITOR

    static AutoSaveEDITOR()
    {

        EditorApplication.playModeStateChanged += SaveOnPlay;
    }
    private static void SaveOnPlay(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto-saving...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
#endif

}
