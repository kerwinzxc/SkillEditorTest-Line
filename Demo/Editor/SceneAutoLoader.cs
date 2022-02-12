/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Demo\Editor\SceneAutoLoader.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-3-9      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;
    using UnityEditor;
    using System;

    [InitializeOnLoad]
    public static class SceneAutoLoader
    {
        private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";

        static SceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static string MasterScene
        {
            get { return EditorPrefs.GetString(cEditorPrefMasterScene, "Assets/CLineActionEditor/Demo/Scene/Main.unity"); }
            set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
                {
                    if (!EditorApplication.OpenScene(MasterScene))
                    {
                        Debug.LogErrorFormat("error: scene not found: {0}", MasterScene);
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }

        }

    }
}