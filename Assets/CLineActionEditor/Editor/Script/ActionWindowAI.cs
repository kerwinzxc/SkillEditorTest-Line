/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActionWindowAI.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-22      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public partial class ActionWindow : EditorWindow
    {
        private AISwitch mCurAI;
        private int mTemplateIndex = 0;
        private List<string> mTemplateAIList;

        private AICondition mCurAICondition = null;
        private EAIConditionType mTemplateAIConditionType = EAIConditionType.EAT_None;

        private void DrawInspectorAI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("New"))
            {
                CreateAI();
            }
            if (GUILayout.Button("Delete"))
            {
                DeleteAI();
            }
            if (GUILayout.Button("Save"))
            {
                SaveAI();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            mScrollViewPosition = GUILayout.BeginScrollView(mScrollViewPosition, false, true);

            DrawPropertyList(mAIPropertyList);

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void CreateAI()
        {
            AISwitch ai = new AISwitch();
            mAIPropertyList.Add(ai);

            BuildAITemplate();
        }

        private void DeleteAI()
        {
            if (mCurProperty != null && mCurProperty is AISwitch)
            {
                mAIPropertyList.Remove(mCurProperty);
                mCurProperty = null;

                BuildAITemplate();
            }
        }

        private void SaveAI()
        {
            SerializeProperty<AISwitch>(FilePath + "AI/AISwitch.json", mAIPropertyList);
        }

        public void DrawAICondition(AISwitch ai)
        {
            GUILayout.Space(5);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("AI条件编辑");
            GUI.color = Color.white;
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            mTemplateAIConditionType = (EAIConditionType)EditorGUILayout.EnumPopup(mTemplateAIConditionType);
            if (GUILayout.Button("New Cond"))
            {
                NewAICondition(ai);
            }
            if (GUILayout.Button("Del Cond"))
            {
                DelAICondition(ai);
            }
            if (GUILayout.Button("Del ALL"))
            {
                DelAllAICondition(ai);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            using (List<AICondition>.Enumerator itr = ai.ConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(itr.Current.GetType().ToString().Replace("CAE.Core.", "")))
                    {
                        mCurAICondition = itr.Current;
                    }
                    if (mCurAICondition == itr.Current)
                    {
                        GUILayout.Label(GetCachedTex(Color.red, 16));
                    }
                    GUILayout.EndHorizontal();

                    if (mCurAICondition == itr.Current)
                    {
                        GUILayout.Space(3);
                        DrawProperty(itr.Current);
                        GUILayout.Space(3);
                    }
                }
            }
        }

        private void NewAICondition(AISwitch ai)
        {
            if (mTemplateAIConditionType == EAIConditionType.EAT_None || mTemplateAIConditionType == EAIConditionType.EAT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select AI condition type.", "OK");
            }
            else
            {
                mCurAICondition = ai.Add(mTemplateAIConditionType);
            }
        }

        private void DelAICondition(AISwitch ai)
        {
            if (mCurAICondition != null)
            {
                ai.Del(mCurAICondition);
                mCurAICondition = null;
            }
        }

        private void DelAllAICondition(AISwitch ai)
        {
            mCurAICondition = null;
            ai.DelAll();
        }

        private void DrawMonsterOrAiPlayerAI(List<AISwitch> aiSwitch)
        {
            GUILayout.Space(5);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("AI 编辑");
            GUI.color = Color.white;
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            mTemplateIndex = EditorGUILayout.Popup(mTemplateIndex, mTemplateAIList.ToArray());
            if (GUILayout.Button("New AI"))
            {
                NewMonsterAI(aiSwitch);
            }
            if (GUILayout.Button("Del AI"))
            {
                DelMonsterAI(aiSwitch);
            }
            if (GUILayout.Button("Del ALL"))
            {
                DelAllAI(aiSwitch);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            using (List<AISwitch>.Enumerator itr = aiSwitch.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(itr.Current.Name))
                    {
                        mCurAI = itr.Current;
                    }
                    if (mCurAI == itr.Current)
                    {
                        GUILayout.Label(GetCachedTex(Color.red, 16));
                    }
                    GUILayout.EndHorizontal();

                    if (mCurAI == itr.Current)
                    {
                        GUILayout.Space(3);
                        DrawProperty(itr.Current);
                        DrawAICondition(itr.Current);
                        GUILayout.Space(3);
                    }
                }
            }
        }

        private void NewMonsterAI(List<AISwitch> aiSwitch)
        {
            if (mTemplateIndex == 0)
            {
                AISwitch ai = new AISwitch();
                ai.ID = "noname" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
                aiSwitch.Add(ai);
            }
            else
            {
                AISwitch tempAI = null;
                using (List<IProperty>.Enumerator itr = mAIPropertyList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        AISwitch ai = itr.Current as AISwitch;
                        if (ai.Name == mTemplateAIList[mTemplateIndex])
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

        private void DelMonsterAI(List<AISwitch> aiSwitch)
        {
            if (mCurAI != null)
            {
                aiSwitch.Remove(mCurAI);

                mCurAI = null;
            }
        }

        private void DelAllAI(List<AISwitch> aiSwitch)
        {
            mCurAI = null;
            aiSwitch.Clear();
        }

        private void BuildAITemplate()
        {
            mTemplateAIList = new List<string>();
            mTemplateAIList.Add("noname");

            using (List<IProperty>.Enumerator itr = mAIPropertyList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    AISwitch ai = itr.Current as AISwitch;
                    mTemplateAIList.Add(ai.Name);
                }
            }
        }

    }
}
