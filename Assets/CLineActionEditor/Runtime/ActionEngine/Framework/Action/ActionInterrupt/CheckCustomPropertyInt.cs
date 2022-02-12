/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckCustomPropertyInt.cs
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

    public sealed class CheckCustomPropertyInt : InterruptCondition, IProperty
    {
        [SerializeField] private string mProperty = string.Empty;
        [SerializeField] private int mMinVal = 0;
        [SerializeField] private int mMaxVal = 100;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("最小比较值", EditorPropertyType.EEPT_Int)]
        public int MinVal
        {
            get { return mMinVal; }
            set { mMinVal = value; }
        }
        [EditorProperty("最大比较值", EditorPropertyType.EEPT_Int)]
        public int MaxVal
        {
            get { return mMaxVal; }
            set { mMaxVal = value; }
        }
        #endregion property

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckCustomPropertyInt;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            int rd = Helper.GetAny<int>(unit.PropertyContext.GetProperty(mProperty));
            return MinVal <= rd && rd < MaxVal;
        }

        public void Deserialize(JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mMinVal = JsonHelper.ReadInt(jd["MinVal"]);
            mMaxVal = JsonHelper.ReadInt(jd["MaxVal"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", mProperty);
            JsonHelper.WriteProperty(ref writer, "MinVal", mMinVal);
            JsonHelper.WriteProperty(ref writer, "MaxVal", mMaxVal);

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckCustomPropertyInt obj = new CheckCustomPropertyInt();
            obj.Property = Property;
            obj.MinVal = this.MinVal;
            obj.MaxVal = this.MaxVal;

            return obj;
        }

    }
}