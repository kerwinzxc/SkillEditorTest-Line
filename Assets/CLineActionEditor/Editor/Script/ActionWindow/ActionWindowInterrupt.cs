/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowInterrupt.cs
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
        [System.NonSerialized] private EInterruptType interruptConditionType = EInterruptType.EIT_None;
        [SerializeReference] private Dictionary<Action, ActionInterrupt> actionInterruptDict = new Dictionary<Action, ActionInterrupt>();

        public void DrawActionInterrupt(ActionInterrupt interrupt)
        {
            DrawProperty(interrupt);

            using (new GUIColorScope(editorResources.colorInspectorLabel))
            {
                GUILayout.Space(5);
                EditorGUILayout.LabelField("打断条件编辑");
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            {
                interruptConditionType = (EInterruptType)EditorGUILayout.EnumPopup(interruptConditionType);
                if (GUILayout.Button("New Cond"))
                {
                    NewInterruptCondition(interrupt);
                    BuildConditionTree(interrupt.ConditionList);
                }
                if (GUILayout.Button("Del Cond"))
                {
                    DelInterruptCondition(interrupt);
                    BuildConditionTree(interrupt.ConditionList);
                }
                if (GUILayout.Button("Del ALL"))
                {
                    DelAllInterruptCondition(interrupt);
                    BuildConditionTree(interrupt.ConditionList);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            DrawCondition();
        }

        private void NewInterruptCondition(ActionInterrupt interrupt)
        {
            if (interruptConditionType == EInterruptType.EIT_None || interruptConditionType == EInterruptType.EIT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select interrupt condition type.", "OK");
            }
            else
            {
                interrupt.Add(interruptConditionType);
            }
        }

        private void DelInterruptCondition(ActionInterrupt interrupt)
        {
            var selectable = GetActorCondition();
            if (selectable != null)
            {
                interrupt.Del(selectable.property as InterruptCondition);
                DeselectAllCondition();
            }
        }

        private void DelAllInterruptCondition(ActionInterrupt interrupt)
        {
            interrupt.DelAll();
        }

        private void SaveActionInterrupt()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.WriteObjectStart();
            writer.WritePropertyName("ActionInterrupts");
            writer.WriteArrayStart();
            using (var itr = actionInterruptDict.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer.WriteObjectStart();
                    writer = itr.Current.Value.Serialize(writer);
                    writer.WriteObjectEnd();
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();

            string path = string.Format("{0}ActionInterrupt/{1}.json", FilePath, UnitWrapper.Instance.ActionGroupName);
            BackupsResource(path);

            FileInfo fi = new FileInfo(path);
            using (StreamWriter sw = fi.CreateText())
            {
                sw.WriteLine(sb.ToString());
                sw.Close();
            }

            AssetDatabase.Refresh();
        }

        private void DeserializeInterrupt()
        {
            actionInterruptDict.Clear();

            string groupName = UnitWrapper.Instance.ActionGroupName;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}ActionInterrupt/{1}.json", FilePath, groupName);

            if (File.Exists(sb.ToString()))
            {
                TextAsset t = ResourceMgr.Instance.LoadObject("/GameData/ActionInterrupt/" + groupName + ".json", typeof(TextAsset)) as TextAsset;
                JsonData jd = JsonMapper.ToObject(t.ToString().Trim());
                JsonData interrupts = jd["ActionInterrupts"];
                for (int i = 0; i < interrupts.Count; ++i)
                {
                    ActionInterrupt aci = new ActionInterrupt();
                    aci.Deserialize(interrupts[i]);

                    using (var itr = actorActionTree.children.GetEnumerator())
                    {
                        while (itr.MoveNext())
                        {
                            var ap = itr.Current as ActorProperty;
                            var ac = ap.property as Action;
                            if (ac.ID.Equals(aci.ActionID))
                            {
                                actionInterruptDict.Add(ac, aci);
                                break;
                            }
                        }
                    }
                }
            }

        }

        public ActionInterrupt GetInterrupt(Action action)
        {
            return actionInterruptDict[action];
        }

        public string GetActionInterruptDisplayName(string actionID)
        {
            return string.Format("To{0}", actionID);
        }

        public Dictionary<string, ActionInterrupt> InterruptList(ActionInterrupt interrupt, ref int index)
        {
            int i = 0;
            var dict = new Dictionary<string, ActionInterrupt>();
            using (var itr = actionInterruptDict.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    string idName = GetActionInterruptDisplayName(itr.Current.Key.ID);
                    dict.Add(idName, itr.Current.Value);

                    if (interrupt.ActionID == itr.Current.Key.ID)
                    {
                        index = i;
                    }

                    i++;
                }
            }

            return dict;
        }
    }

}