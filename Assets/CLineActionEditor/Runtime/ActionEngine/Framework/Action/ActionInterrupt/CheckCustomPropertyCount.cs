/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckCustomPropertyCount.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckCustomPropertyCount : InterruptCondition, IProperty
    {
        [SerializeField] private string mProperty = string.Empty;
        [SerializeField] private int mCount = 0;
        [SerializeField] private ECompareType mCompareType = ECompareType.ECT_Equal;

        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
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

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            int v = Helper.GetAny<int>(unit.PropertyContext.GetProperty(mProperty));
            return CustomCompare<int>.Compare(CompareType, v, mCount);
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