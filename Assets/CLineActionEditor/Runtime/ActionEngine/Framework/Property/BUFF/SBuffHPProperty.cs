/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\SBuffHPProperty.cs
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

    public sealed class SBuffHPProperty : BuffProperty
    {
        [SerializeField] private EAttributeType mAttr = EAttributeType.EAT_MaxHp;
        [SerializeField] private int mAddValue = 0;
        [SerializeField] private int mMulValue = 0;
        [SerializeField] private string mStartEffect = string.Empty;
        [SerializeField] private string mBuffDummy = string.Empty;

        #region property
        [EditorProperty("属性号", EditorPropertyType.EEPT_Enum)]
        public EAttributeType Attr
        {
            get { return mAttr; }
            set { mAttr = value; }
        }
        [EditorProperty("增减值(精度0.001)", EditorPropertyType.EEPT_Int, LabelWidth = 150)]
        public int AddValue
        {
            get { return mAddValue; }
            set { mAddValue = value; }
        }
        [EditorProperty("增减比率(精度0.001)", EditorPropertyType.EEPT_Int, LabelWidth = 150)]
        public int MulValue
        {
            get { return mMulValue; }
            set { mMulValue = value; }
        }
        [EditorProperty("初始特效", EditorPropertyType.EEPT_String)]
        public string StartEffect
        {
            get { return mStartEffect; }
            set { mStartEffect = value; }
        }
        [EditorProperty("特效挂载点", EditorPropertyType.EEPT_String)]
        public string BuffDummy
        {
            get { return mBuffDummy; }
            set { mBuffDummy = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mAttr = JsonHelper.ReadEnum<EAttributeType>(jd["Attr"]);
            mAddValue = JsonHelper.ReadInt(jd["AddValue"]);
            mMulValue = JsonHelper.ReadInt(jd["MulValue"]);
            mStartEffect = JsonHelper.ReadString(jd["StartEffect"]);
            mBuffDummy = JsonHelper.ReadString(jd["BuffDummy"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Attr", mAttr.ToString());
            JsonHelper.WriteProperty(ref writer, "AddValue", mAddValue);
            JsonHelper.WriteProperty(ref writer, "MulValue", mMulValue);
            JsonHelper.WriteProperty(ref writer, "StartEffect", mStartEffect);
            JsonHelper.WriteProperty(ref writer, "BuffDummy", mBuffDummy);

            return writer;
        }
    }
}