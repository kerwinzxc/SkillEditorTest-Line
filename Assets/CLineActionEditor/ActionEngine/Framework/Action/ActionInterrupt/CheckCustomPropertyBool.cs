/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckCustomPropertyBool.cs
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

    public sealed class CheckCustomPropertyBool : InterruptCondition
    {
        private string mProperty = string.Empty;
        private bool mCompareVal = false;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomPropertyToString)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("比较值", EditorPropertyType.EEPT_Bool)]
        public bool CompareVal
        {
            get { return mCompareVal; }
            set { mCompareVal = value; }
        }
        #endregion property

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckCustomPropertyBool;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            Debug.Assert(unit.CustomPropertyHash.ContainsKey(mProperty), string.Format("'{0}' is not defined.", mProperty));
            return (bool)unit.CustomPropertyHash[mProperty].Value == mCompareVal;
        }

        public void Deserialize(JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mCompareVal = JsonHelper.ReadBool(jd["CompareVal"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", mProperty);
            JsonHelper.WriteProperty(ref writer, "CompareVal", mCompareVal);

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckCustomPropertyBool obj = new CheckCustomPropertyBool();
            obj.Property = this.Property;
            obj.CompareVal = this.CompareVal;

            return obj;
        }
    }
}