/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowAction.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-26      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using LitJson;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    internal sealed partial class ActionWindow
    {
        [SerializeField] private ActorTreeProperty actorActionTree = null;

        private void InitInspectorAction()
        {
            actorActionTree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorActionTree.Init(null);
            actorActionTree.AddManipulator(new ActorTreePropertyManipulator(actorActionTree));
        }

        private void DrawInspectorAction()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(2);
                GUILayout.BeginHorizontal(editorResources.btnToolBoxStyle, GUILayout.Width(rectInspectorLeft.width), GUILayout.Height(16));
                {
                    if (GUILayout.Button("New"))
                    {
                        if (!UnitWrapper.Instance.IsReady)
                        {
                            EditorUtility.DisplayDialog("INFO", "Please attach player or monster firstly!", "OK");
                            return;
                        }

                        //TO CLine: support undo at later.
                        var ap = CreateProperty(actorActionTree, typeof(Action));
                        var ac = ap.property as Action;
                        ac.TotalTime = 1666;

                        ActionInterrupt interrupt = new ActionInterrupt();
                        actionInterruptDict.Add(ap.property as Action, interrupt);
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        var selectable = GetActorProperty();
                        if (selectable != null)
                        {
                            var ap = selectable as ActorProperty;
                            var ac = ap.property as Action;
                            actionInterruptDict.Remove(ac);
                        }
                        DeleteProperty();
                        ClearTreeView();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        SaveAction();
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

                leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, false, true);
                {
                    actorActionTree.HandleManipulatorsEvents(this, Event.current);
                    actorActionTree.Draw();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void SaveAction()
        {
            if (!UnitWrapper.Instance.IsReady)
            {
                EditorUtility.DisplayDialog("INFO", "There is no action to save!", "OK");
                return;
            }

            SaveActionInterrupt();

            string path = string.Format("{0}Action/{1}.json", FilePath, UnitWrapper.Instance.ActionGroupName);
            SerializeProperty<Action>(path, actorActionTree);

            EditorUtility.DisplayDialog("INFO", "Succeed to save!", "OK");
        }

        public void DeserializeAction()
        {
            actorActionTree.RemoveAll();

            string groupName = UnitWrapper.Instance.ActionGroupName;
            string fileName = "/GameData/Action/" + groupName + ".json";

            DeserializeProperty<Action>(fileName, actorActionTree);
        }

        public List<string> ActionList()
        {
            List<string> list = new List<string>();
            using (var itr = actorActionTree.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var ap = itr.Current as ActorProperty;
                    var ac = ap.property as Action;
                    list.Add(ac.ID);
                }
            }

            return list;
        }
    }

}