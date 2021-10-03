/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Script\ActionWindowBUFF.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-15      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public partial class ActionWindow : EditorWindow
    {
        private EBuffConditionType mCurBuffConditionType = EBuffConditionType.ECT_None;
        private BuffCondition mCurBuffCondition = null;

        private void DrawInspectorBuff()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("New"))
            {
                CreateBuff();
            }
            if (GUILayout.Button("Delete"))
            {
                DeleteBuff();
            }
            if (GUILayout.Button("Save"))
            {
                SaveBuff();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            mScrollViewPosition = GUILayout.BeginScrollView(mScrollViewPosition, false, true);

            DrawPropertyList(mBuffPropertyList);

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void CreateBuff()
        {
            BuffFactoryProperty p = new BuffFactoryProperty();
            mBuffPropertyList.Add(p);
        }

        private void DeleteBuff()
        {
            if (mCurProperty != null && mCurProperty is BuffFactoryProperty)
            {
                mBuffPropertyList.Remove(mCurProperty);
                mCurProperty = null;
            }
        }

        private void SaveBuff()
        {
            SerializeProperty<BuffFactoryProperty>(FilePath + "Buff/Buff.json", mBuffPropertyList);
        }

        private void DrawBuff(BuffFactoryProperty buff)
        {
            GUILayout.Space(5);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("BUFF数据");
            GUI.color = Color.white;
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

        private void DrawBuffCondition(CBuffProperty buff)
        {
            GUILayout.Space(5);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("BUFF条件编辑");
            GUI.color = Color.white;
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            mCurBuffConditionType = (EBuffConditionType)EditorGUILayout.EnumPopup(mCurBuffConditionType);
            if (GUILayout.Button("New Cond"))
            {
                NewBuffCondition(buff);
            }
            if (GUILayout.Button("Del Cond"))
            {
                DelBuffCondition(buff);
            }
            if (GUILayout.Button("Del ALL"))
            {
                DelAllBuffCondition(buff);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            using (List<BuffCondition>.Enumerator itr = buff.ConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(itr.Current.GetType().ToString().Replace("CAE.Core.", "")))
                    {
                        mCurBuffCondition = itr.Current;
                    }
                    if (mCurBuffCondition == itr.Current)
                    {
                        GUILayout.Label(GetCachedTex(Color.red, 16));
                    }
                    GUILayout.EndHorizontal();

                    if (mCurBuffCondition == itr.Current)
                    {
                        GUILayout.Space(3);
                        DrawProperty(itr.Current);
                        GUILayout.Space(3);
                    }
                }
            }
        }

        private void NewBuffCondition(CBuffProperty buff)
        {
            if (mCurBuffConditionType == EBuffConditionType.ECT_None || mCurBuffConditionType == EBuffConditionType.ECT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select BUFF condition type.", "OK");
            }
            else
            {
                mCurBuffCondition = buff.Add(mCurBuffConditionType);
            }
        }

        private void DelBuffCondition(CBuffProperty buff)
        {
            if (mCurBuffCondition != null)
            {
                buff.Del(mCurBuffCondition);
                mCurBuffCondition = null;
            }
        }

        private void DelAllBuffCondition(CBuffProperty buff)
        {
            mCurBuffCondition = null;
            buff.DelAll();
        }
    }
}