/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\CheckBuffHP.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-24      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using NumericalType = System.Double;

    public sealed class CheckBuffHP : BuffCondition, IProperty
    {
        [SerializeField] private ECompareType mCompareType = ECompareType.ECT_LessEqual;
        [SerializeField] private float mCompareVal = 0.2f;

        private CBuff mOwner = null;
        private float mCurPercent = 1f;

        #region property
        [EditorProperty("HP比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareType
        {
            get { return mCompareType; }
            set { mCompareType = value; }
        }
        [EditorProperty("HP比较值", EditorPropertyType.EEPT_Float)]
        public float CompareVal
        {
            get { return mCompareVal; }
            set { mCompareVal = value; }
        }
        #endregion property

        public EBuffConditionType BuffCondType
        {
            get { return EBuffConditionType.ECT_CheckBuffHP; }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void Init(CBuff owner)
        {
            mOwner = owner;
            mCurPercent = 1f;

            MessageMgr.Instance.Register("EVT_HP_CHANGED", OnEvent);
        }

        public void Destroy()
        {
            MessageMgr.Instance.Unregister("EVT_HP_CHANGED", OnEvent);

            mOwner = null;
        }

        public bool CheckBuff(Unit unit)
        {
            return CustomCompare<float>.Compare(mCompareType, mCurPercent, mCompareVal);
        }

        public void Reset()
        {
            mCurPercent = 1f;
        }

        private void OnEvent(Message msg)
        {
            Unit obj = (Unit)msg.GetArg("Unit");
            if (obj == mOwner.Mgr.Owner)
            {
                NumericalType curHP = (NumericalType)msg.GetArg("CurHP");
                NumericalType maxHP = (NumericalType)msg.GetArg("MaxHP");
                mCurPercent = (float)(curHP / maxHP);

                mOwner.OnEvent();
            }
        }

        public void Deserialize(JsonData jd)
        {
            mCompareType = JsonHelper.ReadEnum<ECompareType>(jd["CompareType"]);
            mCompareVal = JsonHelper.ReadFloat(jd["CompareVal"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareType.ToString());
            JsonHelper.WriteProperty(ref writer, "CompareVal", mCompareVal);

            return writer;
        }

        public BuffCondition Clone()
        {
            CheckBuffHP cb = new CheckBuffHP();
            cb.CompareType = CompareType;
            cb.CompareVal = CompareVal;

            return cb;
        }

    }
}