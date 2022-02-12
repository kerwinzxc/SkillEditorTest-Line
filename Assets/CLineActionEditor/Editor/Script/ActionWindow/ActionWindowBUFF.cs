/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowBUFF.cs
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
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    internal sealed partial class ActionWindow
    {
        [SerializeField] private ActorTreeProperty actorBuffTree = null;
        [System.NonSerialized] private EBuffConditionType buffConditionType = EBuffConditionType.ECT_None;

        private void InitInspectorBuff()
        {
            actorBuffTree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorBuffTree.Init(null);
            actorBuffTree.AddManipulator(new ActorTreePropertyManipulator(actorBuffTree));
        }

        private void DrawInspectorBuff()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(2);
                GUILayout.BeginHorizontal(editorResources.btnToolBoxStyle, GUILayout.Width(rectInspectorLeft.width), GUILayout.Height(16));
                {
                    if (GUILayout.Button("New"))
                    {
                        CreateProperty(actorBuffTree, typeof(BuffFactoryProperty));
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        DeleteProperty();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        SaveBuff();
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

                leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, false, true);
                {
                    actorBuffTree.HandleManipulatorsEvents(this, Event.current);
                    actorBuffTree.Draw();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void SaveBuff()
        {
            SerializeProperty<BuffFactoryProperty>(FilePath + "Buff/Buff.json", actorBuffTree);

            EditorUtility.DisplayDialog("INFO", "Succeed to save!", "OK");
        }

        public void DrawBuff(BuffFactoryProperty buff)
        {
            GUILayout.Space(5);
            using (new GUIColorScope(editorResources.colorInspectorLabel))
            {
                EditorGUILayout.LabelField("BUFF数据");
            }

            GUILayout.Space(2);
            if (buff.BuffProperty != null)
            {
                DrawProperty(buff.BuffProperty);

                if (buff.BuffProperty is CBuffProperty)
                {
                    DrawBuffCondition(buff.BuffProperty as CBuffProperty);
                }
            }
        }

        // right condition
        private void DrawBuffCondition(CBuffProperty buff)
        {
            GUILayout.Space(5);
            using (new GUIColorScope(editorResources.colorInspectorLabel))
            {
                EditorGUILayout.LabelField("BUFF条件编辑");
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            {
                buffConditionType = (EBuffConditionType)EditorGUILayout.EnumPopup(buffConditionType);
                if (GUILayout.Button("New Cond"))
                {
                    NewBuffCondition(buff);
                    BuildConditionTree(buff.ConditionList);
                }
                if (GUILayout.Button("Del Cond"))
                {
                    DelBuffCondition(buff);
                    BuildConditionTree(buff.ConditionList);
                }
                if (GUILayout.Button("Del ALL"))
                {
                    DelAllBuffCondition(buff);
                    BuildConditionTree(buff.ConditionList);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            DrawCondition();
        }

        private void NewBuffCondition(CBuffProperty buff)
        {
            if (buffConditionType == EBuffConditionType.ECT_None || buffConditionType == EBuffConditionType.ECT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select BUFF condition type.", "OK");
            }
            else
            {
                buff.Add(buffConditionType);
            }
        }

        private void DelBuffCondition(CBuffProperty buff)
        {
            var selectable = GetActorCondition();
            if (selectable != null)
            {
                buff.Del(selectable.property as BuffCondition);
                DeselectAllCondition();
            }
        }

        private void DelAllBuffCondition(CBuffProperty buff)
        {
            buff.DelAll();
        }

    }

}