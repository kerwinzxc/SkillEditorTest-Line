/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowRole.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-23      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEditor;
    using UnityEngine;

    internal sealed partial class ActionWindow
    {
        [SerializeField] private ActorTreeProperty actorPlayerTree = null;
        [SerializeField] private ActorTreeProperty actorMonsterTree = null;

        private void InitInspectorRole()
        {
            actorPlayerTree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorPlayerTree.Init(null);
            actorPlayerTree.AddManipulator(new ActorTreePropertyManipulator(actorPlayerTree));

            actorMonsterTree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorMonsterTree.Init(null);
            actorMonsterTree.AddManipulator(new ActorTreePropertyManipulator(actorMonsterTree));

        }

        private void DrawInspectorRole()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(2);
                GUILayout.BeginHorizontal(editorResources.btnToolBoxStyle, GUILayout.Width(rectInspectorLeft.width), GUILayout.Height(16));
                { 
                    if (GUILayout.Button("Player"))
                    {
                        CreateProperty(actorPlayerTree, typeof(PlayerProperty));
                    }
                    if (GUILayout.Button("Monster"))
                    {
                        CreateProperty(actorMonsterTree, typeof(MonsterProperty));
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        DeleteProperty();
                    }
                    if (GUILayout.Button("Attach"))
                    {
                        AttachRole();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        SaveRole();
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

                leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, false, true);
                {
                    DrawInspectorTitle("player");
                    actorPlayerTree.HandleManipulatorsEvents(this, Event.current);
                    actorPlayerTree.Draw();

                    GUILayout.Space(5);

                    DrawInspectorTitle("monster");
                    actorMonsterTree.HandleManipulatorsEvents(this, Event.current);
                    actorMonsterTree.Draw();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void SaveRole()
        {
            SerializeProperty<PlayerProperty>(FilePath + "Unit/Player.json", actorPlayerTree);
            SerializeProperty<MonsterProperty>(FilePath + "Unit/Monster.json", actorMonsterTree);

            EditorUtility.DisplayDialog("INFO", "Succeed to save!", "OK");
        }

        private void AttachRole()
        {
            var selectProperty = GetActorProperty() as ActorProperty;
            if (selectProperty == null)
            {
                EditorUtility.DisplayDialog("INFO", "please select role!", "YES");
                return;
            }

            if (!UnitWrapper.Instance.BuildUnit(selectProperty.property))
            {
                return;
            }

            ClearTreeView();
            DeserializeAction();
            DeserializeInterrupt();
        }
    }

}