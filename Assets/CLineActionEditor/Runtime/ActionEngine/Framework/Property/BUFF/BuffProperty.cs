/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\BuffProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-8      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public enum EBuffTarget
    {
        EBT_None = 0,
        EBT_Self,
        EBT_Attacker,
        EBT_Attackee,
        EBT_Summon,
    }

    public enum EBuffAddType
    {
        EAT_AddNone = 0,
        EAT_StoleOldest,
    }

    public class BuffProperty : IProperty
    {
        [SerializeField] private string mID = string.Empty;
        [SerializeField] private string mName = string.Empty;
        [SerializeField] private string mDescription = string.Empty;
        [SerializeField] private string mIcon = string.Empty;
        [SerializeField] private EBuffTarget mBuffTarget = EBuffTarget.EBT_None;
        [SerializeField] private EBuffAddType mAddType = EBuffAddType.EAT_AddNone;
        [SerializeField] private int mAddNum = 1;
        [SerializeField] private int mDuration = -1;

        #region property
        [EditorProperty("ID", EditorPropertyType.EEPT_String)]
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
        [EditorProperty("描述", EditorPropertyType.EEPT_String)]
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }
        [EditorProperty("图标", EditorPropertyType.EEPT_String)]
        public string Icon
        {
            get { return mIcon; }
            set { mIcon = value; }
        }
        [EditorProperty("BUFF目标", EditorPropertyType.EEPT_Enum)]
        public EBuffTarget BuffTarget
        {
            get { return mBuffTarget; }
            set { mBuffTarget = value; }
        }
        [EditorProperty("堆叠方式", EditorPropertyType.EEPT_Enum)]
        public EBuffAddType AddType
        {
            get { return mAddType; }
            set { mAddType = value; }
        }
        [EditorProperty("堆叠层数", EditorPropertyType.EEPT_Int)]
        public int AddNum
        {
            get { return mAddNum; }
            set { mAddNum = value; }
        }
        [EditorProperty("生命周期", EditorPropertyType.EEPT_Int)]
        public int Duration
        {
            get { return mDuration; }
            set { mDuration = value; }
        }
        #endregion

        public string DebugName
        {
            get { return mName; }
        }

        public virtual void Deserialize(JsonData jd)
        {
            mID = JsonHelper.ReadString(jd["ID"]);
            mName = JsonHelper.ReadString(jd["Name"]);
            mDescription = JsonHelper.ReadString(jd["Description"]);
            mIcon = JsonHelper.ReadString(jd["Icon"]);
            mBuffTarget = JsonHelper.ReadEnum<EBuffTarget>(jd["BuffTarget"]);
            mAddType = JsonHelper.ReadEnum<EBuffAddType>(jd["AddType"]);
            mAddNum = JsonHelper.ReadInt(jd["AddNum"]);
            mDuration = JsonHelper.ReadInt(jd["Duration"]);
        }

        public virtual JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "ID", mID);
            JsonHelper.WriteProperty(ref writer, "Name", mName);
            JsonHelper.WriteProperty(ref writer, "Description", mDescription);
            JsonHelper.WriteProperty(ref writer, "Icon", mIcon);
            JsonHelper.WriteProperty(ref writer, "BuffTarget", mBuffTarget.ToString());
            JsonHelper.WriteProperty(ref writer, "AddType", mAddType.ToString());
            JsonHelper.WriteProperty(ref writer, "AddNum", mAddNum);
            JsonHelper.WriteProperty(ref writer, "Duration", mDuration);

            return writer;
        }
    }
}