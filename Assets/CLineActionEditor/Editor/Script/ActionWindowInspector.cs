/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActionWindowInspector.cs
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
    using System;
    using System.Text;
    using System.Reflection;
    using System.Collections.Generic;
    using System.IO;
    using LitJson;

    public partial class ActionWindow : EditorWindow
    {
        private IProperty mCurProperty;

        private Vector2 mScrollViewPosition;
        private Vector2 mScrollViewPosition1;

        private List<IProperty> mPlayerPropertyList = new List<IProperty>();
        private List<IProperty> mMonsterPropertyList = new List<IProperty>();
        private List<IProperty> mAIPlayerPropertyList = new List<IProperty>();
        private List<IProperty> mPetPropertyList = new List<IProperty>();
        private List<IProperty> mWeaponPropertyList = new List<IProperty>();
        private List<IProperty> mAIPropertyList = new List<IProperty>();
        private List<IProperty> mBuffPropertyList = new List<IProperty>();
        private List<IProperty> mActionList = new List<IProperty>();//group_action

        private Dictionary<IProperty, ActionWrapper> mActionWrapperHash = new Dictionary<IProperty, ActionWrapper>();
        private Dictionary<IProperty, ActionInterrupt> mActionInterruptList = new Dictionary<IProperty, ActionInterrupt>();//group_actioninterrupt
        private static Dictionary<string, GameObject> gameobject2stringDic = new Dictionary<string, GameObject>();

        private static string FilePath
        {
            get { return  Application.dataPath + @"/CLineActionEditor/Demo/Resource/GameData/"; }
        }

        public IProperty Property
        {
            get { return mCurProperty; }
        }

        public Dictionary<IProperty, ActionInterrupt> ActionInterruptList
        {
            get { return mActionInterruptList; }
        }

        private void DrawInspector()
        {
            GUI.color = Color.white;

            int id = mInspectorID;
            mInspectorID = GUI.Toolbar(new Rect(position.width - mInspectorWidth, mToolBarHeight, mInspectorWidth * 0.5f, 20), mInspectorID, mInspectorString);
            if (id != mInspectorID)
                Repaint();

            Rect rc = new Rect(position.width - mInspectorWidth, mToolBarHeight * 2f, mInspectorWidth * 0.5f, position.height - mToolBarHeight * 2f);
            GUILayout.BeginArea(rc);
            switch (mInspectorID)
            {
                case 0:
                    DrawInspectorRole();
                    break;
                case 1:
                    DrawInspectorWeapon();
                    break;
                case 2:
                    DrawInspectorActionList();
                    break;
                case 3:
                    DrawInspectorAI();
                    break;
                case 4:
                    DrawInspectorBuff();
                    break;
                case 5:
                    //DrawInspectorProperty();
                    break;
            }
            GUILayout.EndArea();
            Rect rc1 = new Rect(position.width - mInspectorWidth * 0.5f, mToolBarHeight * 2f, mInspectorWidth * 0.5f, position.height - mToolBarHeight * 2f);
            GUILayout.BeginArea(rc1);

            DrawInspectorProperty();

            GUILayout.EndArea();
        }

        private void DrawInspectorProperty()
        {
            if (mCurActor == null && mCurProperty == null) return;

            if (mCurActor != null)
            {
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                mScrollViewPosition1 = GUILayout.BeginScrollView(mScrollViewPosition1, false, true);

                mCurActor.OnDrawInspector();

                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                mScrollViewPosition1 = GUILayout.BeginScrollView(mScrollViewPosition1, false, true);

                if (mCurProperty is Action)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Label("Action Attribute");
                    GUILayout.Space(2);
                    GUI.color = Color.white;
                }
                DrawProperty(mCurProperty);
                if (mCurProperty is Action)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Space(5);
                    GUILayout.Label("Action Interrupt");
                    GUILayout.Space(2);
                    GUI.color = Color.white;

                    Action action = mCurProperty as Action;
                    ActionInterrupt interrupt = mActionInterruptList[mCurProperty];
                    interrupt.ActionID = action.ID;
                    interrupt.InterruptName = "To" + action.ID;

                    DrawActionInterrupt(interrupt);

                    mTotalTime = (mCurProperty as Action).TotalTime * 0.001f;
                }
                if (mCurProperty is AISwitch)
                {
                    DrawAICondition(mCurProperty as AISwitch);
                }
                if (mCurProperty is MonsterProperty)
                {
                    MonsterProperty m = mCurProperty as MonsterProperty;
                    DrawMonsterOrAiPlayerAI(m.AISwitch);
                }
                if (mCurProperty is PetProperty)
                {
                    PetProperty p = mCurProperty as PetProperty;
                    DrawMonsterOrAiPlayerAI(p.AISwitch);
                }
                if (mCurProperty is BuffFactoryProperty)
                {
                    DrawBuff(mCurProperty as BuffFactoryProperty);
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
        }

        private void DrawPropertyList(List<IProperty> list)
        {
            using (List<IProperty>.Enumerator itr = list.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    string btnName = string.IsNullOrEmpty(itr.Current.DebugName) ? "noname" : itr.Current.DebugName;
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(btnName))
                    {
                        mCurProperty = itr.Current;

                        if (mCurProperty is Action)
                        {
                            mCurActionWrapper = mActionWrapperHash[mCurProperty];
                        }
                    }
                    if (mCurProperty == itr.Current)
                    {
                        GUILayout.Label(GetCachedTex(Color.red, 16));
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        public static void DrawProperty(object obj)
        {
            PropertyInfo[] pis = obj.GetType().GetProperties();

            for (int i = 0; i < pis.Length; ++i)
            {
                object[] attrs = pis[i].GetCustomAttributes(typeof(EditorPropertyAttribute), false);
                if (attrs.Length == 1)
                {
                    EditorPropertyAttribute epa = (EditorPropertyAttribute)attrs[0];
                    if (!string.IsNullOrEmpty(epa.Deprecated)) continue;

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(epa.PropertyName);
                    GUILayout.Space(5);

                    object val = EditorUtil.GetProperty(obj, pis[i].Name);

                    if (epa.Edit)
                    {
                        switch (epa.PropertyType)
                        {
                            case EditorPropertyType.EEPT_Bool:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.Toggle((bool)val));
                                break;
                            case EditorPropertyType.EEPT_Int:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.IntField((int)val));
                                break;
                            case EditorPropertyType.EEPT_Float:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.FloatField((float)val));
                                break;
                            case EditorPropertyType.EEPT_String:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.TextField((string)val));
                                break;
                            case EditorPropertyType.EEPT_Vector2:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.Vector2Field("", (Vector2)val));
                                break;
                            case EditorPropertyType.EEPT_Vector3:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.Vector3Field("", (Vector3)val));
                                break;
                            case EditorPropertyType.EEPT_Vector4:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.Vector4Field("", (Vector4)val));
                                break;
                            case EditorPropertyType.EEPT_Color:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.ColorField((Color)val));
                                break;
                            case EditorPropertyType.EEPT_Quaternion:
                                {
                                    Quaternion q = (Quaternion)val;
                                    q.eulerAngles = EditorGUILayout.Vector3Field("", q.eulerAngles);
                                    EditorUtil.SetProperty(obj, pis[i].Name, q);
                                }
                                break;
                            case EditorPropertyType.EEPT_Transform:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.ObjectField((UnityEngine.Object)val, typeof(Transform), true));
                                break;
                            case EditorPropertyType.EEPT_GameObject:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.ObjectField((UnityEngine.Object)val, typeof(GameObject), true));
                                break;
                            case EditorPropertyType.EEPT_GameObjectToString:
                                {
                                    GameObject go = null;

                                    string szPath = (string)val;
                                    if (!string.IsNullOrEmpty(szPath))
                                    {
                                        if (gameobject2stringDic.ContainsKey(szPath))
                                        {
                                            go = gameobject2stringDic[szPath];
                                        }
                                        else
                                        {
                                            go = ResourceMgr.Instance.LoadObject<GameObject>(szPath);
                                            if (go != null && !gameobject2stringDic.ContainsKey(szPath))
                                            {
                                                gameobject2stringDic.Add(szPath, go);
                                            }
                                        }
                                    }
                                    go = EditorGUILayout.ObjectField(go, typeof(GameObject), true) as GameObject;
                                    string name = ResourceMgr.Instance.FormatResourceName(AssetDatabase.GetAssetPath(go));

                                    if (go != null)
                                        EditorUtil.SetProperty(obj, pis[i].Name, name);
                                    else
                                        EditorUtil.SetProperty(obj, pis[i].Name, string.Empty);

                                }
                                break;
                            case EditorPropertyType.EEPT_Texture:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.ObjectField((UnityEngine.Object)val, typeof(Texture), true));
                                break;
                            case EditorPropertyType.EEPT_Material:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.ObjectField((UnityEngine.Object)val, typeof(Material), true));
                                break;
                            case EditorPropertyType.EEPT_Enum:
                                EditorUtil.SetProperty(obj, pis[i].Name, EditorGUILayout.EnumPopup((Enum)val));
                                break;
                            case EditorPropertyType.EEPT_AnimatorStateToString:
                                {
                                    int idx = EditorUtil.GetStringIndex(UnitWrapper.Instance.StateNameList, (string)val);
                                    idx = EditorGUILayout.Popup(idx, UnitWrapper.Instance.StateNameList.ToArray());
                                    if (idx >= 0 && idx < UnitWrapper.Instance.StateNameList.Count)
                                        EditorUtil.SetProperty(obj, pis[i].Name, UnitWrapper.Instance.StateNameList[idx]);
                                }
                                break;
                            case EditorPropertyType.EEPT_AnimatorParamToString:
                                {
                                    int idx = EditorUtil.GetStringIndex(UnitWrapper.Instance.ParameterList, (string)val);
                                    idx = EditorGUILayout.Popup(idx, UnitWrapper.Instance.ParameterList.ToArray());
                                    if (idx >= 0 && idx < UnitWrapper.Instance.ParameterList.Count)
                                        EditorUtil.SetProperty(obj, pis[i].Name, UnitWrapper.Instance.ParameterList[idx]);
                                }
                                break;
                            case EditorPropertyType.EEPT_CustomPropertyToString:
                                {
                                    List<string> list = CustomProperty.CustomPropertyList();
                                    int idx = EditorUtil.GetStringIndex(list, (string)val);
                                    idx = EditorGUILayout.Popup(idx, list.ToArray());
                                    if (idx >= 0 && idx < list.Count)
                                        EditorUtil.SetProperty(obj, pis[i].Name, list[idx]);
                                }
                                break;
                            case EditorPropertyType.EEPT_ActionToString:
                                {
                                    List<string> list = ActionWindow.Instance.ActionList();
                                    int idx = EditorUtil.GetStringIndex(list, (string)val);
                                    idx = EditorGUILayout.Popup(idx, list.ToArray());
                                    if (idx >= 0 && idx < list.Count)
                                        EditorUtil.SetProperty(obj, pis[i].Name, list[idx]);
                                }
                                break;
                            case EditorPropertyType.EEPT_Object:
                                {
                                    DrawProperty(val);
                                }
                                break;
                            case EditorPropertyType.EEPT_ObjectHitList:
                                {
                                    List<HitRandom> list = (List<HitRandom>)val;

                                    GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                    GUI.color = Color.cyan;
                                    GUILayout.Label("Size");
                                    GUI.color = Color.white;

                                    int count = list.Count;
                                    count = EditorGUILayout.IntField(count);
                                    if (count < list.Count)
                                    {
                                        list.RemoveRange(count, list.Count - count);
                                    }
                                    else if (count > list.Count)
                                    {
                                        int num = count - list.Count;
                                        for (int j = 0; j < num; ++j)
                                        {
                                            list.Add(new HitRandom());
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                    for (int j = 0; j < list.Count; ++j)
                                    {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label(j.ToString());
                                        DrawProperty(list[j]);
                                        GUILayout.EndHorizontal();
                                    }
                                    GUILayout.EndVertical();

                                    EditorUtil.SetProperty(obj, pis[i].Name, list);
                                }
                                break;
                            case EditorPropertyType.EEPT_GameObjectToStringList:
                                {
                                    List<string> list = (List<string>)val;

                                    GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                    GUI.color = Color.cyan;
                                    GUILayout.Label("Size");
                                    GUI.color = Color.white;

                                    int count = list.Count;
                                    count = EditorGUILayout.IntField(count);
                                    if (count < list.Count)
                                    {
                                        list.RemoveRange(count, list.Count - count);
                                    }
                                    else if (count > list.Count)
                                    {
                                        int num = count - list.Count;
                                        for (int j = 0; j < num; ++j)
                                        {
                                            list.Add("");
                                        }
                                    }
                                    GUILayout.EndHorizontal();

                                    for (int j = 0; j < list.Count; ++j)
                                    {
                                        GameObject go = null;

                                        string szPath = (string)list[j];
                                        if (!string.IsNullOrEmpty(szPath))
                                        {
                                            if (gameobject2stringDic.ContainsKey(szPath))
                                            {
                                                go = gameobject2stringDic[szPath];
                                            }
                                            else
                                            {
                                                go = ResourceMgr.Instance.LoadObject<GameObject>(szPath);
                                                if (go != null && !gameobject2stringDic.ContainsKey(szPath))
                                                {
                                                    gameobject2stringDic.Add(szPath, go);
                                                }
                                            }
                                        }

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label(j.ToString());
                                        go = EditorGUILayout.ObjectField(go, typeof(GameObject), true) as GameObject;
                                        GUILayout.EndHorizontal();

                                        string name = ResourceMgr.Instance.FormatResourceName(AssetDatabase.GetAssetPath(go));
                                        list[j] = (go != null ? name : string.Empty);
                                    }
                                    GUILayout.EndVertical();

                                    EditorUtil.SetProperty(obj, pis[i].Name, list);
                                }
                                break;
                            case EditorPropertyType.EEPT_StringList:
                                {
                                    List<string> list = (List<string>)val;

                                    GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                    GUI.color = Color.cyan;
                                    GUILayout.Label("Size");
                                    GUI.color = Color.white;

                                    int count = list.Count;
                                    count = EditorGUILayout.IntField(count);
                                    if (count < list.Count)
                                    {
                                        list.RemoveRange(count, list.Count - count);
                                    }
                                    else if (count > list.Count)
                                    {
                                        int num = count - list.Count;
                                        for (int j = 0; j < num; ++j)
                                        {
                                            list.Add("");
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                    for (int j = 0; j < list.Count; ++j)
                                    {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label(j.ToString());
                                        list[j] = EditorGUILayout.TextField(list[j]);
                                        GUILayout.EndHorizontal();
                                    }
                                    GUILayout.EndVertical();

                                    EditorUtil.SetProperty(obj, pis[i].Name, list);

                                }
                                break;
                        }
                    }
                    else
                    {
                        string sz = (val == null ? string.Empty : val.ToString());
                        GUILayout.Label(sz);
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        private void SerializeProperty<T>(string filename, List<IProperty> mList) where T : IProperty, new()
        {
            if (mList.Count < 1) return;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            FileInfo fi = new FileInfo(filename);
            StreamWriter sw = fi.CreateText();

            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.WriteObjectStart();
            writer.WritePropertyName("Property");
            writer.WriteArrayStart();
            for (int i = 0; i < mList.Count; ++i)
            {
                T t = (T)mList[i];
                writer = t.Serialize(writer);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();

            sw.WriteLine(sb.ToString());
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
        }

        private void DeserializeProperty<T>(string filename, List<IProperty> mList) where T : IProperty, new()
        {
            if (File.Exists(FilePath + filename.Replace("/GameData/", "")))
            {
                TextAsset text = ResourceMgr.Instance.LoadObject(filename, typeof(TextAsset)) as TextAsset;
                if (text == null)
                {
                    LogMgr.Instance.Logf(ELogType.ELT_WARNING, "ActionEditor", "the \"{0}\" of Json is not exist!!!", filename);
                    return;
                }
                else
                {
                    JsonData jd = JsonMapper.ToObject(text.ToString().Trim());
                    JsonData datas = jd["Property"];
                    for (int i = 0; i < datas.Count; ++i)
                    {
                        T t = new T();
                        t.Deserialize(datas[i]);

                        mList.Add(t);
                    }
                }
            }
        }

        public void Deserialize()
        {
            DeserializeProperty<PlayerProperty>("/GameData/Unit/Player.json", mPlayerPropertyList);
            DeserializeProperty<MonsterProperty>("/GameData/Unit/Monster.json", mMonsterPropertyList);
            DeserializeProperty<PetProperty>("/GameData/Unit/Pet.json", mPetPropertyList);
            DeserializeProperty<WeaponProperty>("/GameData/Unit/Weapon.json", mWeaponPropertyList);
            DeserializeProperty<AISwitch>("/GameData/AI/AISwitch.json", mAIPropertyList);
            DeserializeProperty<BuffFactoryProperty>("/GameData/Buff/Buff.json", mBuffPropertyList);

            BuildAITemplate();
        }

        public List<string> ActionList()
        {
            List<string> list = new List<string>();
            using (List<IProperty>.Enumerator itr = mActionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    Action ac = itr.Current as Action;
                    list.Add(ac.ID);
                }
            }

            return list;
        }

    }
}
