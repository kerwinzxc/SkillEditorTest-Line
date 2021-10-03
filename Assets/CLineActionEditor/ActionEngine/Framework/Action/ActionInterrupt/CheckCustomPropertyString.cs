/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckCustomPropertyString.cs
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

    public sealed class CheckCustomPropertyString : InterruptCondition
    {
        private string mProperty = CustomProperty.sAI;
        private string mCompareVal = string.Empty;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomPropertyToString)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("比较值", EditorPropertyType.EEPT_String)]
        public string CompareVal
        {
            get { return mCompareVal; }
            set { mCompareVal = value; }
        }
        #endregion property

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckCustomPropertyString;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            Debug.Assert(unit.CustomPropertyHash.ContainsKey(mProperty), string.Format("'{0}' is not defined.", mProperty));
            return (string)unit.CustomPropertyHash[mProperty].Value == mCompareVal;
        }

        public void Deserialize(JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mCompareVal = JsonHelper.ReadString(jd["CompareVal"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", mProperty);
            JsonHelper.WriteProperty(ref writer, "CompareVal", mCompareVal);

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckCustomPropertyString obj = new CheckCustomPropertyString();
            obj.Property = this.Property;
            obj.CompareVal = this.CompareVal;

            return obj;
        }
    }
}