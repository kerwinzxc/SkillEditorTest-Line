/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckCustomPropertyFloat.cs
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

    public sealed class CheckCustomPropertyFloat : InterruptCondition, IProperty
    {
        [SerializeField] private string mProperty = string.Empty;
        [SerializeField] private float mMinVal = 0f;
        [SerializeField] private float mMaxVal = 1f;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("最小随机比较值", EditorPropertyType.EEPT_Float)]
        public float MinVal
        {
            get { return mMinVal; }
            set { mMinVal = value; }
        }
        [EditorProperty("最大随机比较值", EditorPropertyType.EEPT_Float)]
        public float MaxVal
        {
            get { return mMaxVal; }
            set { mMaxVal = value; }
        }
        #endregion property

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckCustomPropertyFloat;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            float rd = Helper.GetAny<float>(unit.PropertyContext.GetProperty(mProperty));
            return MinVal <= rd && rd < MaxVal;
        }

        public void Deserialize(JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mMinVal = JsonHelper.ReadFloat(jd["MinVal"]);
            mMaxVal = JsonHelper.ReadFloat(jd["MaxVal"]);
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
            CheckCustomPropertyFloat obj = new CheckCustomPropertyFloat();
            obj.Property = Property;
            obj.MinVal = this.MinVal;
            obj.MaxVal = this.MaxVal;

            return obj;
        }

    }
}