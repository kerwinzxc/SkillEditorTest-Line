/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\MonsterProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class MonsterProperty : IProperty
    {
        [SerializeField] private string mID;
        [SerializeField] private string mName;
        [SerializeField] private string mDescription;
        [SerializeField] private string mPrefab;
        [SerializeField] private string mStartupAI;
        [SerializeField] private string mStartupWeapon;
        [SerializeField] private string mActionGroup;
        [SerializeReference] private List<AISwitch> mAISwitch = new List<AISwitch>();


        #region property
        [EditorProperty("ResID(资源ID)", EditorPropertyType.EEPT_String)]
        public string ID
        {
            get { return mID; }
            set { mID = value; }
        }
        [EditorProperty("名称", EditorPropertyType.EEPT_String)]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        [EditorProperty("描述", EditorPropertyType.EEPT_String, Deprecated = "移至数值表")]
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }
        [EditorProperty("预制件", EditorPropertyType.EEPT_GameObject)]
        public string Prefab
        {
            get { return mPrefab; }
            set { mPrefab = value; }
        }
        [EditorProperty("启动AI", EditorPropertyType.EEPT_String)]
        public string StartupAI
        {
            get { return mStartupAI; }
            set { mStartupAI = value; }
        }
        [EditorProperty("默认武器", EditorPropertyType.EEPT_String)]
        public string StartupWeapon
        {
            get { return mStartupWeapon; }
            set { mStartupWeapon = value; }
        }
        [EditorProperty("Action组", EditorPropertyType.EEPT_String)]
        public string ActionGroup
        {
            get { return mActionGroup; }
            set { mActionGroup = value; }
        }
        #endregion


        public string DebugName
        {
            get { return mName; }
        }

        public List<AISwitch> AISwitch
        {
            get { return mAISwitch; }
            set { mAISwitch = value; }
        }

        public void Deserialize(JsonData jd)
        {
            mID = JsonHelper.ReadString(jd["ID"]);
            mName = JsonHelper.ReadString(jd["Name"]);
            mDescription = JsonHelper.ReadString(jd["Description"]);
            mPrefab = JsonHelper.ReadString(jd["Prefab"]);
            mStartupAI = JsonHelper.ReadString(jd["StartupAI"]);
            mStartupWeapon = JsonHelper.ReadString(jd["StartupWeapon"]);
            mActionGroup = JsonHelper.ReadString(jd["ActionGroup"]);

            JsonData jdAI = jd["AISwitchs"];
            for (int i = 0; i < jdAI.Count; ++i)
            {
                AISwitch ai = new AISwitch();
                ai.Deserialize(jdAI[i]);
                AISwitch.Add(ai);
            }
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "ID", mID);
            JsonHelper.WriteProperty(ref writer, "Name", mName);
            JsonHelper.WriteProperty(ref writer, "Description", mDescription);
            JsonHelper.WriteProperty(ref writer, "Prefab", mPrefab);
            JsonHelper.WriteProperty(ref writer, "StartupAI", mStartupAI);
            JsonHelper.WriteProperty(ref writer, "StartupWeapon", mStartupWeapon);
            JsonHelper.WriteProperty(ref writer, "ActionGroup", mActionGroup);

            writer.WritePropertyName("AISwitchs");
            writer.WriteArrayStart();
            AISwitch.Sort((l, r) =>
            {
                return l.Order < r.Order ? -1 : (l.Order == r.Order ? 0 : 1);
            });
            using (List<AISwitch>.Enumerator itr = AISwitch.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer = itr.Current.Serialize(writer);
                }
            }
            writer.WriteArrayEnd();

            writer.WriteObjectEnd();
            return writer;
        }

    }
}
