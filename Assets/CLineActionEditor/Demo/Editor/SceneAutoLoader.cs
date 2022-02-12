/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Demo\Editor\SceneAutoLoader.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-3-9      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.IO;
    using UnityEditor.SceneManagement;

    [InitializeOnLoad]
    public static class SceneAutoLoader
    {
        private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
        private const string cEditorRuntimeLayout = "SceneAutoLoader.RuntimeLayout";

        static SceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static string MasterScene
        {
            get 
            {
                string root = Helper.GetRootDirectory();
                root = Path.Combine(root, "Demo/Scene/Main.unity");
                return EditorPrefs.GetString(cEditorPrefMasterScene, root);
            }
            set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
        }

        private static string RuntimeLayout
        {
            get
            {
                string root = Helper.GetRootDirectory();
                root = Path.Combine(root, "Editor/Editor Resources/Layout/ActionRuntime.wlt");
                return EditorPrefs.GetString(cEditorRuntimeLayout, root);
            }
            set { EditorPrefs.SetString(cEditorRuntimeLayout, value); }
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
                {
                    EditorUtility.LoadWindowLayout(RuntimeLayout);
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