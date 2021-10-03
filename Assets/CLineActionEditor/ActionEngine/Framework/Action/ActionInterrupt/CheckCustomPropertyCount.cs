/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckCustomPropertyCount.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-15      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckCustomPropertyCount : InterruptCondition
    {
        private string mProperty = string.Empty;
        private int mCount = 0;
        private ECompareType mCompareType = ECompareType.ECT_Equal;

        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomPropertyToString)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("连击比较值", EditorPropertyType.EEPT_Int)]
        public int Count
        {
            get { return mCount; }
            set { mCount = value; }
        }
        [EditorProperty("连击比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareType
        {
            get { return mCompareType; }
            set { mCompareType = value; }
        }

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckCustomPropertyCount;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            Debug.Assert(unit.CustomPropertyHash.ContainsKey(mProperty), string.Format("'{0}' is not defined.", mProperty));
            return CustomCompare<int>.Compare(CompareType, (int)unit.CustomPropertyHash[mProperty].Value, mCount);
        }

        public void Deserialize(JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mCount = JsonHelper.ReadInt(jd["Count"]);
            mCompareType = JsonHelper.ReadEnum<ECompareType>(jd["CompareType"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", mProperty);
            JsonHelper.WriteProperty(ref writer, "Count", mCount);
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareType.ToString());

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckCustomPropertyCount obj = new CheckCustomPropertyCount();
            obj.Property = this.Property;
            obj.Count = this.Count;
            obj.CompareType = this.CompareType;

            return obj;
        }
    }

}