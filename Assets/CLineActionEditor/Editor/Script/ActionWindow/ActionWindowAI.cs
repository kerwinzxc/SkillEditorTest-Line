/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowAI.cs
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
        [SerializeField] private ActorTreeProperty actorAITree = null;
        [System.NonSerialized] private EAIConditionType aiConditionType = EAIConditionType.EAT_None;
        [System.NonSerialized] private int aiTemplateIndex = 0;
        [System.NonSerialized] private List<string> aiTemplateList = new List<string>();
        [System.NonSerialized] private AISwitch selectInstanceAI = null;

        private void InitInspectorAI()
        {
            actorAITree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorAITree.Init(null);
            actorAITree.AddManipulator(new ActorTreePropertyManipulator(actorAITree));
        }

        //////////////////////////////////////////////////////////////////////////
        /// AI Template
        private void DrawInspectorAI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(2);
                GUILayout.BeginHorizontal(editorResources.btnToolBoxStyle, GUILayout.Width(rectInspectorLeft.width), GUILayout.Height(16));
                {
                    if (GUILayout.Button("New"))
                    {
                        CreateProperty(actorAITree, typeof(AISwitch));
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        DeleteProperty();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        SaveAI();
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

                leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, false, true);
                {
                    actorAITree.HandleManipulatorsEvents(this, Event.current);
                    actorAITree.Draw();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private  void SaveAI()
        {
            actorAITree.children.Sort((lhs, rhs) =>
            {
                var l = lhs as ActorProperty;
                var r = rhs as ActorProperty;
                var lp = l.property as AISwitch;
                var rp = r.property as AISwitch;
                return lp.Order < rp.Order ? -1 : (lp.Order == rp.Order ? 0 : 1);
            });
            SerializeProperty<AISwitch>(FilePath + "AI/AISwitch.json", actorAITree);

            EditorUtility.DisplayDialog("INFO", "Succeed to save!", "OK");
        }

        //////////////////////////////////////////////////////////////////////////
        /// AI Condition
        public void DrawAICondition(AISwitch ai)
        {
            GUILayout.Space(5);
            using (new GUIColorScope(editorResources.colorInspectorLabel))
            {
                EditorGUILayout.LabelField("AI条件编辑");
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            {
                aiConditionType = (EAIConditionType)EditorGUILayout.EnumPopup(aiConditionType);
                if (GUILayout.Button("New Cond"))
                {
                    NewAICondition(ai);
                    BuildConditionTree(ai.ConditionList);
                }
                if (GUILayout.Button("Del Cond"))
                {
                    DelAICondition(ai);
                    BuildConditionTree(ai.ConditionList);
                }
                if (GUILayout.Button("Del ALL"))
                {
                    DelAllAICondition(ai);
                    BuildConditionTree(ai.ConditionList);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            DrawCondition();
        }

        private void NewAICondition(AISwitch ai)
        {
            if (aiConditionType == EAIConditionType.EAT_None || aiConditionType == EAIConditionType.EAT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select AI condition type.", "OK");
            }
            else
            {
                ai.Add(aiConditionType);
            }
        }

        private void DelAICondition(AISwitch ai)
        {
            var selectable = GetActorCondition();
            if (selectable != null)
            {
                ai.Del(selectable.property as AICondition);
                DeselectAllCondition();
            }
        }

        private void DelAllAICondition(AISwitch ai)
        {
            ai.DelAll();
        }


        //////////////////////////////////////////////////////////////////////////
        /// AI Instance
        public void DrawRoleAI(List<AISwitch> aiSwitch)
        {
            GUILayout.Space(5);
            using (new GUIColorScope(editorResources.colorInspectorLabel))
            {
                EditorGUILayout.LabelField("AI 编辑");
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            {
                BuildAITemplate();
                aiTemplateIndex = EditorGUILayout.Popup(aiTemplateIndex, aiTemplateList.ToArray());
                if (GUILayout.Button("New AI"))
                {
                    NewRoleAI(aiSwitch);
                }
                if (GUILayout.Button("Del AI"))
                {
                    DelRoleAI(aiSwitch);
                }
                if (GUILayout.Button("Del ALL"))
                {
                    DelAllAI(aiSwitch);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            using (List<AISwitch>.Enumerator itr = aiSwitch.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    GUILayout.BeginHorizontal();
                    {
                        string btnName = string.IsNullOrEmpty(itr.Current.DebugName) ? "noname" : itr.Current.DebugName;
                        var color = (selectInstanceAI == itr.Current) ? editorResources.colorPropertySelected : editorResources.colorWhite;
                        using (new GUIColorScope(color))
                        {
                            if (GUILayout.Button(btnName))
                            {
                                selectInstanceAI = itr.Current;
                                BuildConditionTree(selectInstanceAI.ConditionList);
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (selectInstanceAI == itr.Current)
                    {
                        GUILayout.Space(3);
                        DrawProperty(itr.Current);
                        DrawAICondition(itr.Current);
                        GUILayout.Space(3);
                    }
                }
            }
        }

        private void BuildAITemplate()
        {
            aiTemplateList.Clear();
            aiTemplateList.Add("noname");

            using (var itr = actorAITree.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    ActorProperty ap = itr.Current as ActorProperty;
                    AISwitch ai = ap.property as AISwitch;

                    aiTemplateList.Add(ai.Name);
                }
            }
        }

        private void NewRoleAI(List<AISwitch> aiSwitch)
        {
            if (aiTemplateIndex == 0)
            {
                AISwitch ai = new AISwitch();
                ai.ID = "noname" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
                aiSwitch.Add(ai);
            }
            else
            {
                AISwitch tempAI = null;
                using (var itr = actorAITree.children.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        ActorProperty ap = itr.Current as ActorProperty;
                        AISwitch ai = ap.property as AISwitch;
                        if (ai.Name == aiTemplateList[aiTemplateIndex])
                        {
                            tempAI = ai;
                            break;
                        }
                    }
                }

                AISwitch newAI = tempAI.Clone();
                aiSwitch.Add(newAI);
            }

        }

        private void DelRoleAI(List<AISwitch> aiSwitch)
        {
            if (selectInstanceAI != null)
            {
                aiSwitch.Remove(selectInstanceAI);
                selectInstanceAI = null;
            }
        }

        private void DelAllAI(List<AISwitch> aiSwitch)
        {
            aiSwitch.Clear();
            selectInstanceAI = null;
        }

    }

}