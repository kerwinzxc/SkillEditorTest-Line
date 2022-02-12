/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindow.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using UnityEditor.SceneManagement;

    internal sealed partial class ActionWindow : EditorWindow
    {
        private static ActionWindow sInstance = null;
        public static ActionWindow instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = GetWindow<ActionWindow>();
                }
                return sInstance;
            }
        }

        [MenuItem("Tools/CLine Action Editor 3.0")]
        private static void Main()
        {
            InitEnvironment();

            instance.titleContent = new GUIContent("CLine Action Editor 3.0");
            //instance.minSize = new Vector2(1500, 750);
            instance.Show();
            instance.Focus();
        }

        private static void InitEnvironment()
        {
            string root = Helper.GetRootDirectory();
            root = Path.Combine(root, "Editor/Editor Resources");

            string path = Path.Combine(root, "Scene/ActionEditor.unity");
            EditorSceneManager.OpenScene(path);

            path = Path.Combine(root, "Layout/ActionEditor.wlt");
            EditorUtility.LoadWindowLayout(path);
        }

        private ActionWindowAsset _editorResources = null;
        public ActionWindowAsset editorResources
        {
            get
            {
                if (_editorResources == null)
                {
                    string path = Path.Combine(editorResourcePath, "ActionWindowSetting.asset");
                    _editorResources = AssetDatabase.LoadAssetAtPath<ActionWindowAsset>(path);
                    _editorResources.Init();
                }
                return _editorResources;
            }
        }

        private string _editorResourcePath = null;
        public string editorResourcePath
        {
            get
            {
                if (string.IsNullOrEmpty(_editorResourcePath))
                {
                    string root = Helper.GetRootDirectory();
                    _editorResourcePath = Path.Combine(root, "Editor/Editor Resources");
                }
                return _editorResourcePath;
            }
        }
    }
}