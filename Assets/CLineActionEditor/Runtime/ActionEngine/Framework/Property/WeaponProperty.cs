/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\WeaponProperty.cs
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
    using System;
    using System.IO;
    using System.Text;
    using UnityEngine;
    using System.Collections.Generic;
    using LitJson;

    public sealed class WeaponProperty : IProperty
    {
        public enum EWeaponType
        {
            EWT_None = 0,
            EWT_Sword,

        }

        public enum EDeadType
        {
            EDT_Normal = 1,
            EDT_Explosion = 2,
            EDT_NormalOrExplosion = 3,
        }

        [SerializeField] private string mID;
        [SerializeField] private string mName;
        [SerializeField] private string mDesc;
        [SerializeField] private string mPrefab;
        [SerializeField] private string mSetupAction;
        [SerializeField] private string mManShowAction;
        [SerializeField] private string mWomanShowAction;
        [SerializeField] private string mAttachDummyIdle;
        [SerializeField] private string mAttachDummyAttackL;
        [SerializeField] private string mAttachDummyAttackR;
        [SerializeField] private Vector3 mPositionOffset;
        [SerializeField] private EWeaponType mWeaponType;
        [SerializeField] private bool mIsDribble;
        [SerializeField] private EDeadType mDeadType = EDeadType.EDT_NormalOrExplosion;
        [SerializeField] private string mShowPrefab;
        [SerializeField] private List<string> mActionList = new List<string>();

        private float mScale;
        private int mDamage;
        private int mCriticalDamage;
        private int mCriticalRate;
        private int mTenacity;

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
        public string Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }
        [EditorProperty("预制件", EditorPropertyType.EEPT_GameObject)]
        public string Prefab
        {
            get { return mPrefab; }
            set { mPrefab = value; }
        }
        [EditorProperty("初始Action", EditorPropertyType.EEPT_String)]
        public string SetupAction
        {
            get { return mSetupAction; }
            set { mSetupAction = value; }
        }
        [EditorProperty("男性角色展示Action", EditorPropertyType.EEPT_String, LabelWidth = 120)]
        public string ManShowAction
        {
            get { return mManShowAction; }
            set { mManShowAction = value; }
        }
        [EditorProperty("女性角色展示Action", EditorPropertyType.EEPT_String, LabelWidth = 120)]
        public string WomanShowAction
        {
            get { return mWomanShowAction; }
            set { mWomanShowAction = value; }
        }
        [EditorProperty("Attach Dummy Idle", EditorPropertyType.EEPT_String, LabelWidth = 120)]
        public string AttachDummyIdle
        {
            get { return mAttachDummyIdle; }
            set { mAttachDummyIdle = value; }
        }
        [EditorProperty("Attach Dummy Attack Left", EditorPropertyType.EEPT_String, LabelWidth = 160)]
        public string AttachDummyAttackL
        {
            get { return mAttachDummyAttackL; }
            set { mAttachDummyAttackL = value; }
        }
        [EditorProperty("Attach Dummy Attack Right", EditorPropertyType.EEPT_String, LabelWidth = 160)]
        public string AttachDummyAttackR
        {
            get { return mAttachDummyAttackR; }
            set { mAttachDummyAttackR = value; }
        }
        [EditorProperty("相对位置", EditorPropertyType.EEPT_Vector3)]
        public UnityEngine.Vector3 PositionOffset
        {
            get { return mPositionOffset; }
            set { mPositionOffset = value; }
        }
        [EditorProperty("武器类别", EditorPropertyType.EEPT_Enum)]
        public EWeaponType WeaponType
        {
            get { return mWeaponType; }
            set { mWeaponType = value; }
        }
        [EditorProperty("是否属于连击武器", EditorPropertyType.EEPT_Bool)]
        public bool IsDribble
        {
            get { return mIsDribble; }
            set { mIsDribble = value; }
        }
        [EditorProperty("武器导致死亡效果", EditorPropertyType.EEPT_Enum)]
        public WeaponProperty.EDeadType DeadType
        {
            get { return mDeadType; }
            set { mDeadType = value; }
        }
        [EditorProperty("展示模型", EditorPropertyType.EEPT_GameObject)]
        public string ShowPrefab
        {
            get { return mShowPrefab; }
            set { mShowPrefab = value; }
        }
        [EditorProperty("预加载动作", EditorPropertyType.EEPT_List)]
        public List<string> ActionList
        {
            get { return mActionList; }
            set { mActionList = value; }
        }


        [EditorProperty("绽放比例", EditorPropertyType.EEPT_Float, Deprecated = "预值件实现")]
        public float Scale
        {
            get { return mScale; }
            set { mScale = value; }
        }
        [EditorProperty("物理伤害", EditorPropertyType.EEPT_Int, Deprecated = "移至数值表")]
        public int Damage
        {
            get { return mDamage; }
            set { mDamage = value; }
        }
        [EditorProperty("暴击伤害", EditorPropertyType.EEPT_Int, Deprecated = "移至数值表,改成加Buff实现")]
        public int CriticalDamage
        {
            get { return mCriticalDamage; }
            set { mCriticalDamage = value; }
        }
        [EditorProperty("暴击几率", EditorPropertyType.EEPT_Int, Deprecated = "移至数值表,改成加Buff实现")]
        public int CriticalRate
        {
            get { return mCriticalRate; }
            set { mCriticalRate = value; }
        }
        [EditorProperty("韧性值", EditorPropertyType.EEPT_Int, Deprecated = "移至数值表,改成加Buff实现")]
        public int Tenacity
        {
            get { return mTenacity; }
            set { mTenacity = value; }
        }

        #endregion

        public string DebugName
        {
            get { return mName; }
        }

        public void Deserialize(JsonData jd)
        {
            mID = JsonHelper.ReadString(jd["ID"]);
            mName = JsonHelper.ReadString(jd["Name"]);
            mDesc = JsonHelper.ReadString(jd["Description"]);
            mPrefab = JsonHelper.ReadString(jd["Prefab"]);
            mSetupAction = JsonHelper.ReadString(jd["SetupAction"]);
            mManShowAction = JsonHelper.ReadString(jd["ManShowAction"]);
            mWomanShowAction = JsonHelper.ReadString(jd["WomanShowAction"]);
            mAttachDummyIdle = JsonHelper.ReadString(jd["AttachDummyIdle"]);
            mAttachDummyAttackL = JsonHelper.ReadString(jd["AttachDummyAttackL"]);
            mAttachDummyAttackR = JsonHelper.ReadString(jd["AttachDummyAttackR"]);
            mPositionOffset = JsonHelper.ReadVector3(jd["PositionOffset"]);
            mWeaponType = JsonHelper.ReadEnum<EWeaponType>(jd["WeaponType"]);
            mIsDribble = JsonHelper.ReadBool(jd["IsDribble"]);
            mDeadType = JsonHelper.ReadEnum<EDeadType>(jd["DeadType"]);
            mShowPrefab = JsonHelper.ReadString(jd["ShowPrefab"]);
            mActionList = JsonHelper.ReadListString(jd["ActionList"]);
        }
        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "ID", mID);
            JsonHelper.WriteProperty(ref writer, "Name", mName);
            JsonHelper.WriteProperty(ref writer, "Description", mDesc);
            JsonHelper.WriteProperty(ref writer, "Prefab", mPrefab);
            JsonHelper.WriteProperty(ref writer, "SetupAction", mSetupAction);
            JsonHelper.WriteProperty(ref writer, "ManShowAction", mManShowAction);
            JsonHelper.WriteProperty(ref writer, "WomanShowAction", mWomanShowAction);
            JsonHelper.WriteProperty(ref writer, "AttachDummyIdle", mAttachDummyIdle);
            JsonHelper.WriteProperty(ref writer, "AttachDummyAttackL", mAttachDummyAttackL);
            JsonHelper.WriteProperty(ref writer, "AttachDummyAttackR", mAttachDummyAttackR);
            JsonHelper.WriteProperty(ref writer, "PositionOffset", mPositionOffset);
            JsonHelper.WriteProperty(ref writer, "WeaponType", mWeaponType.ToString());
            JsonHelper.WriteProperty(ref writer, "IsDribble", mIsDribble);
            JsonHelper.WriteProperty(ref writer, "DeadType", mDeadType.ToString());
            JsonHelper.WriteProperty(ref writer, "ShowPrefab", mShowPrefab);
            JsonHelper.WriteProperty(ref writer, "ActionList", mActionList);
            writer.WriteObjectEnd();

            return writer;
        }
    }

}
