/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Script\ActionWindowInterrupt.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-19      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public partial class ActionWindow : EditorWindow
    {
        private InterruptCondition mCurInterruptCondition = null;
        private EInterruptType mTemplateInterruptType = EInterruptType.EIT_None;
        private ActorGroupInterrupt mCopyInterruptGroup = null;

        public ActorGroupInterrupt CopyInterruptGroup
        {
            get { return mCopyInterruptGroup; }
            set { mCopyInterruptGroup = value; }
        }

        public void DrawActionInterrupt(ActionInterrupt interrupt)
        {
            DrawProperty(interrupt);

            GUILayout.Space(5);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("打断条件编辑");
            GUI.color = Color.white;
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            mTemplateInterruptType = (EInterruptType)EditorGUILayout.EnumPopup(mTemplateInterruptType);
            if (GUILayout.Button("New Cond"))
            {
                NewInterruptCondition(interrupt);
            }
            if (GUILayout.Button("Del Cond"))
            {
                DelInterruptCondition(interrupt);
            }
            if (GUILayout.Button("Del ALL"))
            {
                DelAllInterruptCondition(interrupt);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            using (List<InterruptCondition>.Enumerator itr = interrupt.ConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(itr.Current.GetType().ToString().Replace("CAE.Core.", "")))
                    {
                        mCurInterruptCondition = itr.Current;
                    }
                    if (mCurInterruptCondition == itr.Current)
                    {
                        GUILayout.Label(GetCachedTex(Color.red, 16));
                    }
                    GUILayout.EndHorizontal();

                    if (mCurInterruptCondition == itr.Current)
                    {
                        GUILayout.Space(3);
                        DrawProperty(itr.Current);
                        GUILayout.Space(3);
                    }
                }
            }
        }

        private void NewInterruptCondition(ActionInterrupt interrupt)
        {
            if (mTemplateInterruptType == EInterruptType.EIT_None || mTemplateInterruptType == EInterruptType.EIT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select interrupt condition type.", "OK");
            }
            else
            {
                mCurInterruptCondition = interrupt.Add(mTemplateInterruptType);
            }
        }

        private void DelInterruptCondition(ActionInterrupt interrupt)
        {
            if (mCurInterruptCondition != null)
            {
                interrupt.Del(mCurInterruptCondition);
                mCurInterruptCondition = null;
            }
        }

        private void DelAllInterruptCondition(ActionInterrupt interrupt)
        {
            mCurInterruptCondition = null;
            interrupt.DelAll();
        }

    }
}