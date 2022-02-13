/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowInspector.cs
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
    using System.Collections.Generic;
    using System.Reflection;
    using System;
    using LitJson;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal sealed partial class ActionWindow
    {
        [System.NonSerialized] public Vector2 leftScrollPos;
        [System.NonSerialized] public Vector2 rightScrollPos;
        [System.NonSerialized] private Texture2D cacheTexture = null;

        private static Dictionary<string, GameObject> gameobject2stringDic = new Dictionary<string, GameObject>();

        private int inspectorID = 0;
        private string[] inspectorNames = new string[] { "Role", "Weapon", "Action", "AI", "Buff" };

        private void DrawInspector()
        {
            rectInspector = new Rect(position.width - inspectorWidth, toobarHeight, inspectorWidth, position.height - toobarHeight);
            rectInspectorLeft = new Rect(rectInspector.x, rectInspector.y + timeRulerHeight, rectInspector.width / 2, rectInspector.height - timeRulerHeight);
            rectInspectorRight = new Rect(rectInspector.x + rectInspectorLeft.width, rectInspectorLeft.y, rectInspectorLeft.width, rectInspectorLeft.height);

            int id = inspectorID;
            var toolbarRectLeft = new Rect(rectInspector.x, rectInspector.y, rectInspector.width / 2, timeRulerHeight);
            inspectorID = GUI.Toolbar(toolbarRectLeft, inspectorID, inspectorNames, editorResources.appToolbar);
            if (id != inspectorID)
            {
                Repaint();
            }

            GUILayout.BeginArea(rectInspectorLeft);
            switch (inspectorID)
            {
                case 0:
                    DrawInspectorRole();
                    break;
                case 1:
                    DrawInspectorWeapon();
                    break;
                case 2:
                    DrawInspectorAction();
                    break;
                case 3:
                    DrawInspectorAI();
                    break;
                case 4:
                    DrawInspectorBuff();
                    break;
            }
            GUILayout.EndArea();

            var toolbarRectRight = new Rect(toolbarRectLeft.x + toolbarRectLeft.width, toolbarRectLeft.y, toolbarRectLeft.width, toolbarRectLeft.height);
            GUI.Box(toolbarRectRight, "Inspector", editorResources.appToolbar);
            GUILayout.BeginArea(rectInspectorRight, editorResources.propertyBackground);
            {
                var selectEvent = GetActorEvent();
                if (selectEvent != null)
                {
                    selectEvent.DrawInspector();
                }
                else
                {
                    var selectProperty = GetActorProperty();
                    selectProperty?.DrawInspector();
                }
            }
            GUILayout.EndArea();
        }

        public Vector2 MousePos2InspectorPos(Vector2 mousePosition)
        {
            Vector2 pos = mousePosition;
            //pos.y += inspectorScrollPos.y;

            return pos;
        }

        public void DrawInspectorTitle(string title)
        {
            using (new GUIColorScope(editorResources.colorInspectorLabel))
            {
                GUILayout.Label(title);
            }
        }

        public void DrawSelectable(Color color, int size = 16)
        {
            if (cacheTexture == null)
            {
                cacheTexture = new Texture2D(size, size);
                for (int i = 0; i < size; ++i)
                    for (int j = 0; j < size; ++j)
                        cacheTexture.SetPixel(i, j, color);
                cacheTexture.Apply();
                cacheTexture.hideFlags = HideFlags.HideAndDontSave;
            }

            GUILayout.Label(cacheTexture);
        }

        private ActorProperty CreateProperty(ActorTreeProperty parent, System.Type type)
        {
            ActorProperty ap = ScriptableObject.CreateInstance<ActorProperty>();
            ap.property = System.Activator.CreateInstance(type) as IProperty;

            string operation = string.Format("{0} {1}", type.ToString(), parent.children.Count);
            Helper.RegisterCreatedObjectUndo(ap, operation);
            Helper.PushUndo(new UnityEngine.Object[] { ap, parent }, operation);

            ap.Init(parent);

            return ap;
        }

        private void DeleteProperty()
        {
            var selectable = GetActorProperty();
            if (selectable != null)
            {
                string title = string.Format("Delete {0}", selectable.GetPropertyType());
                if (EditorUtility.DisplayDialog(title, "Are you sure?", "YES", "NO!"))
                {
                    var parent = selectable.parent;

                    Helper.PushUndo(new UnityEngine.Object[] { parent, selectable }, editorResources.contextDelProperty.text);
                    Helper.PushDestroyUndo(parent, selectable);

                    parent.RemoveChild(selectable);
                    DeselectAllProperty();
                }
            }
        }

        private ActorProperty DeserializeProperty(ActorTreeProperty parent, System.Type type, JsonData jd)
        {
            ActorProperty ap = ScriptableObject.CreateInstance<ActorProperty>();
            ap.property = System.Activator.CreateInstance(type) as IProperty;
            ap.Init(parent);
            ap.Deserialize(jd);

            return ap;
        }

        private void BackupsResource(string path)
        {
            if (File.Exists(path))
            {
                string libraryPath = Application.dataPath.Replace("Assets", "Library");
                string filePath = path.Replace(FilePath, "");
                libraryPath = Path.Combine(libraryPath, filePath);

                string bakName = string.Format(".{0}.json", System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                libraryPath = libraryPath.Replace(".json", bakName);

                Debug.Log("Backups--->" + libraryPath);
                //File.Delete(path);
                Helper.MoveFile(path, libraryPath);
            }
        }

        private void SerializeProperty<T>(string path, ActorTreeProperty actor) where T : IProperty, new()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = prettyPrint;
            writer.WriteObjectStart();
            writer.WritePropertyName("Property");
            writer.WriteArrayStart();
            using (var itr = actor.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var actorProperty = itr.Current as ActorProperty;
                    writer = actorProperty.Serialize(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();

            BackupsResource(path);

            FileInfo fi = new FileInfo(path);
            using (StreamWriter sw = fi.CreateText())
            {
                sw.WriteLine(sb.ToString());
                sw.Close();
            }

            AssetDatabase.Refresh();
        }

        private void DeserializeProperty<T>(string filename, ActorTreeProperty actor) where T : IProperty, new()
        {
            if (File.Exists(FilePath + filename.Replace("/GameData/", "")))
            {
                TextAsset text = ResourceMgr.Instance.LoadObject(filename, typeof(TextAsset)) as TextAsset;
                if (text == null)
                {
                    LogMgr.Instance.Logf(ELogType.ELT_WARNING, "ActionEditor", "the \"{0}\" of Json is not exist!!!", filename);
                }
                else
                {
                    JsonData jd = JsonMapper.ToObject(text.ToString().Trim());
                    JsonData datas = jd["Property"];
                    for (int i = 0; i < datas.Count; ++i)
                    {
                        DeserializeProperty(actor, typeof(T), datas[i]);
                    }
                }
            }
        }

        private void Deserialize()
        {
            InitInspectorRole();
            InitInspectorWeapon();
            InitInspectorAction();
            InitInspectorAI();
            InitInspectorBuff();
            InitInspectorCondition();

            DeserializeProperty<PlayerProperty>("/GameData/Unit/Player.json", actorPlayerTree);
            DeserializeProperty<MonsterProperty>("/GameData/Unit/Monster.json", actorMonsterTree);
            DeserializeProperty<WeaponProperty>("/GameData/Unit/Weapon.json", actorWeaponTree);
            DeserializeProperty<AISwitch>("/GameData/AI/AISwitch.json", actorAITree);
            DeserializeProperty<BuffFactoryProperty>("/GameData/Buff/Buff.json", actorBuffTree);
        }

        public bool IsInteger(string str)
        {
            return Regex.Match(str, "^[0-9]*$").Success;
        }

        public bool IsFloat(string str)
        {
            return Regex.Match(str, "^[0-9]*[.][0-9]*$").Success;
        }

        public void DrawProperty(object obj)
        {
            PropertyInfo[] pis = obj.GetType().GetProperties().OrderBy(p => p.MetadataToken).ToArray();
            for (int i = 0; i < pis.Length; ++i)
            {
                object[] attrs = pis[i].GetCustomAttributes(typeof(EditorPropertyAttribute), false);
                if (attrs.Length == 1)
                {
                    EditorPropertyAttribute epa = (EditorPropertyAttribute)attrs[0];
                    if (!string.IsNullOrEmpty(epa.Deprecated)) continue;

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(epa.PropertyName, GUILayout.Width(epa.LabelWidth));

                    object val = Helper.GetProperty(obj, pis[i].Name);
                    if (epa.Edit)
                    {
                        switch (epa.PropertyType)
                        {
                            case EditorPropertyType.EEPT_Bool:
                                Helper.SetProperty(obj, pis[i].Name, GUILayout.Toggle((bool)val, ""));
                                break;
                            case EditorPropertyType.EEPT_Int:
                                {
                                    string sv = val.ToString();
                                    sv = GUILayout.TextField(sv);
                                    if (IsInteger(sv))
                                    {
                                        Helper.SetProperty(obj, pis[i].Name, int.Parse(sv));
                                    }
                                }
                                break;
                            case EditorPropertyType.EEPT_Float:
                                Helper.SetProperty(obj, pis[i].Name, EditorGUILayout.FloatField((float)val));
                                break;
                            case EditorPropertyType.EEPT_String:
                                Helper.SetProperty(obj, pis[i].Name, GUILayout.TextField((string)val));
                                break;
                            case EditorPropertyType.EEPT_Vector2:
                                Helper.SetProperty(obj, pis[i].Name, EditorGUILayout.Vector2Field("", (Vector2)val));
                                break;
                            case EditorPropertyType.EEPT_Vector3:
                                Helper.SetProperty(obj, pis[i].Name, EditorGUILayout.Vector3Field("", (Vector3)val));
                                break;
                            case EditorPropertyType.EEPT_Vector4:
                                Helper.SetProperty(obj, pis[i].Name, EditorGUILayout.Vector4Field("", (Vector4)val));
                                break;
                            case EditorPropertyType.EEPT_Color:
                                Helper.SetProperty(obj, pis[i].Name, EditorGUILayout.ColorField((Color)val));
                                break;
                            case EditorPropertyType.EEPT_Quaternion:
                                {
                                    Quaternion q = (Quaternion)val;
                                    q.eulerAngles = EditorGUILayout.Vector3Field("", q.eulerAngles);
                                    Helper.SetProperty(obj, pis[i].Name, q);
                                }
                                break;
                            case EditorPropertyType.EEPT_GameObject:
                                {
                                    GameObjectField(obj, pis[i].Name, val);
                                }
                                break;
                            case EditorPropertyType.EEPT_Enum:
                                Helper.SetProperty(obj, pis[i].Name, EditorGUILayout.EnumPopup((Enum)val));
                                break;
                            case EditorPropertyType.EEPT_AnimatorTypeName:
                                {
                                    PopupList(obj, pis[i].Name, val, AnimatorTypeNameList);
                                }
                                break;
                            case EditorPropertyType.EEPT_AnimatorState:
                                {
                                    PopupList(obj, pis[i].Name, val, UnitWrapper.Instance.StateNameList);
                                }
                                break;
                            case EditorPropertyType.EEPT_AnimatorParam:
                                {
                                    PopupList(obj, pis[i].Name, val, UnitWrapper.Instance.ParameterList);
                                }
                                break;
                            case EditorPropertyType.EEPT_CustomProperty:
                                {
                                    PopupList(obj, pis[i].Name, val, ActionEngine.PropertyName.CustomPropertyList());
                                }
                                break;
                            case EditorPropertyType.EEPT_Action:
                                {
                                    PopupList(obj, pis[i].Name, val, ActionList());
                                }
                                break;
                            case EditorPropertyType.EEPT_Object:
                                {
                                    DrawProperty(val);
                                }
                                break;
                            case EditorPropertyType.EEPT_List:
                                {
                                    ListField(obj, pis[i].Name, val);
                                }
                                break;
                            case EditorPropertyType.EEPT_GameObjectList:
                                {
                                    GameObjectListField(obj, pis[i].Name, val);
                                }
                                break;
                            
                        }
                    }
                    else
                    {
                        string sz = (val == null ? string.Empty : val.ToString());
                        GUILayout.Label(sz, GUILayout.Width(epa.LabelWidth));
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        private void PopupList(object obj, string propertyName, object val, List<string> list)
        {
            int idx = Helper.GetStringIndex(list, (string)val);
            idx = EditorGUILayout.Popup(idx, list.ToArray());
            if (idx >= 0 && idx < list.Count)
                Helper.SetProperty(obj, propertyName, list[idx]);
        }

        private void ListField(object obj, string propertyName, object val)
        {
            var itemType = typeof(bool);
            var list = val as System.Collections.IList;
            foreach (var interfaceType in val.GetType().GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    itemType = val.GetType().GetGenericArguments()[0];
                    break;
                }
            }

            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    using (new GUIColorScope(editorResources.colorInspectorLabel))
                    {
                        GUILayout.Label("Size");
                    }
                    int count = EditorGUILayout.IntField(list.Count);
                    if (count < list.Count)
                    {
                        int idx = list.Count;
                        while (idx > count)
                        {
                            list.RemoveAt(idx - 1);
                            idx--;
                        }
                    }
                    else if (count > list.Count)
                    {
                        int num = count - list.Count;
                        for (int j = 0; j < num; ++j)
                        {
                            TypeCode typeCode = Type.GetTypeCode(itemType);
                            if (typeCode == TypeCode.String)
                            {
                                list.Add(string.Empty);
                            }
                            else
                            {
                                list.Add(System.Activator.CreateInstance(itemType));
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();

                for (int j = 0; j < list.Count; ++j)
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label(j.ToString());
                        var typeCode = Type.GetTypeCode(itemType);
                        switch (typeCode)
                        {
                            case TypeCode.Int32:
                                {
                                    var sv = list[j].ToString();
                                    sv = GUILayout.TextField(sv);
                                    if (IsInteger(sv))
                                    {
                                        list[j] = int.Parse(sv);
                                    }
                                }
                                break;
                            case TypeCode.Single:
                                list[j] = EditorGUILayout.FloatField((float)list[j]);
                                break;
                            case TypeCode.String:
                                list[j] = GUILayout.TextField((string)list[j]);
                                break;
                            case TypeCode.Object:
                                DrawProperty(list[j]);
                                break;
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndVertical();

            Helper.SetProperty(obj, propertyName, list);
        }

        private void GameObjectField(object obj, string propertyName, object val)
        {
            GameObject go = null;
            var szPath = (string)val;

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
            var name = ResourceMgr.Instance.FormatResourceName(AssetDatabase.GetAssetPath(go));

            Helper.SetProperty(obj, propertyName, (go != null ? name : string.Empty));
        }

        private void GameObjectListField(object obj, string propertyName, object val)
        {
            var list = val as List<string>;

            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    using (new GUIColorScope(editorResources.colorInspectorLabel))
                    {
                        GUILayout.Label("Size");
                    }
                    int count = EditorGUILayout.IntField(list.Count);
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
                }
                GUILayout.EndHorizontal();

                for (int j = 0; j < list.Count; ++j)
                {
                    GameObject go = null;
                    var szPath = list[j];
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

                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label(j.ToString());
                        go = EditorGUILayout.ObjectField(go, typeof(GameObject), true) as GameObject;
                    }
                    GUILayout.EndVertical();

                    string name = ResourceMgr.Instance.FormatResourceName(AssetDatabase.GetAssetPath(go));
                    list[j] = (go != null ? name : string.Empty);
                }
            }
            GUILayout.EndVertical();

            Helper.SetProperty(obj, propertyName, list);
        }

    }
}