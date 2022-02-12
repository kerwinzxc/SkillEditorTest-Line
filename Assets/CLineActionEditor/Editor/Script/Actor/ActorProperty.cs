/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorProperty.cs
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
    using LitJson;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    internal class ActorProperty : ActorTreeProperty
    {
        [SerializeReference] public IProperty property = null;

        public override void OnSelected()
        {
            if (property is Action action)
            {
                window.ClearConditionTree();

                var interrupt = window.GetInterrupt(action);
                window.BuildConditionTree(interrupt.ConditionList);

                window.ClearTreeView();
                window.BuildTreeView(action);
            }
            else if (property is AISwitch ai)
            {
                window.ClearConditionTree();
                window.BuildConditionTree(ai.ConditionList);
            }
            else if (property is BuffFactoryProperty buff)
            {
                window.ClearConditionTree();
                if (buff.BuffProperty != null && buff.BuffProperty is CBuffProperty cbuff)
                {
                    window.BuildConditionTree(cbuff.ConditionList);
                }
            }
        }

        public override void Draw()
        {
            GUILayout.BeginHorizontal();
            {
                var selected = window.HasSelectProperty(this);
                var offset = selected ? 50f : 20f;
                var rc = GUILayoutUtility.GetRect(window.rectInspectorLeft.width - offset, window.propertyHeight);
                if (rc.height == window.propertyHeight)
                {
                    rect = rc;
                }

                string btnName = string.IsNullOrEmpty(property.DebugName) ? "noname" : property.DebugName;
                if (GUI.Button(rc, btnName))
                {
                }
                if (selected)
                {
                    window.DrawSelectable(window.editorResources.colorRed);
                }
            }
            GUILayout.EndHorizontal();
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(2);

                window.rightScrollPos = GUILayout.BeginScrollView(window.rightScrollPos, false, true);
                {
                    if (property is Action)
                    {
                        using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                        {
                            GUILayout.Label("Action Attribute");
                            GUILayout.Space(5);
                        }
                    }

                    window.DrawProperty(property);

                    if (property is Action action)
                    {
                        using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                        {
                            GUILayout.Space(5);
                            GUILayout.Label("Action Interrupt");
                        }

                        GUILayout.Space(2);
                        var interrupt = window.GetInterrupt(action);
                        interrupt.ActionID = action.ID;
                        interrupt.InterruptName = window.GetActionInterruptDisplayName(action.ID);
                        window.DrawActionInterrupt(interrupt);
                    }
                    else if (property is AISwitch aiSwitch)
                    {
                        window.DrawAICondition(aiSwitch);
                    }
                    else if (property is BuffFactoryProperty buff)
                    {
                        window.DrawBuff(buff);
                    }
                    else if (property is MonsterProperty mp)
                    {
                        window.DrawRoleAI(mp.AISwitch);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        public override string GetPropertyType()
        {
            return property.GetType().Name;
        }

        public void Deserialize(JsonData jd)
        {
            property.Deserialize(jd);
            if (property is Action ac)
            {
                ac.ForceSort();
            }
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return property.Serialize(writer);
        }
    }
}