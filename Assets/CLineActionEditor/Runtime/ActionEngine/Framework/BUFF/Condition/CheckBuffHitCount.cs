﻿/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\Condition\CheckBuffHitCount.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-11      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckBuffHitCount : BuffCondition, IProperty
    {
        [SerializeField] private ECompareType mCompareType = ECompareType.ECT_Equal;
        [SerializeField] private int mCompareVal = 3;

        private CBuff mOwner = null;
        private int mCurHitCount = 0;

        #region property
        [EditorProperty("比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareType
        {
            get { return mCompareType; }
            set { mCompareType = value; }
        }
        [EditorProperty("N次受击后", EditorPropertyType.EEPT_Int)]
        public int CompareVal
        {
            get { return mCompareVal; }
            set { mCompareVal = value; }
        }
        #endregion property

        public EBuffConditionType BuffCondType
        {
            get { return EBuffConditionType.ECT_CheckBuffHitCount; }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void Init(CBuff owner)
        {
            mOwner = owner;
            mCurHitCount = 0;

            MessageMgr.Instance.Register("EVT_HIT", OnEvent);
        }

        public void Destroy()
        {
            MessageMgr.Instance.Unregister("EVT_HIT", OnEvent);

            mOwner = null;
        }

        public bool CheckBuff(Unit unit)
        {
            return CustomCompare<int>.Compare(mCompareType, mCurHitCount, mCompareVal);
        }

        public void Reset()
        {
            mCurHitCount = 0;
        }

        private void OnEvent(Message msg)
        {
            Unit obj = (Unit)msg.GetArg("Unit");
            if (obj == mOwner.Mgr.Owner)
            {
                mCurHitCount++;

                mOwner.OnEvent();
            }
        }

        public void Deserialize(JsonData jd)
        {
            mCompareType = JsonHelper.ReadEnum<ECompareType>(jd["CompareType"]);
            mCompareVal = JsonHelper.ReadInt(jd["CompareVal"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareType.ToString());
            JsonHelper.WriteProperty(ref writer, "CompareVal", mCompareVal);

            return writer;
        }

        public BuffCondition Clone()
        {
            CheckBuffHitCount cb = new CheckBuffHitCount();
            cb.CompareType = CompareType;
            cb.CompareVal = CompareVal;

            return cb;
        }
    }
}