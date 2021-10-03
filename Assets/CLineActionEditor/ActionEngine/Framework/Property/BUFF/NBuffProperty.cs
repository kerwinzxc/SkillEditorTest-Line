/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\NBuffProperty.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-8      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class NBuffProperty : BuffProperty
    {
        private Unit.EAttributeType mAttr = Unit.EAttributeType.EAT_Max;
        private int mAddValue = 0;
        private int mMulValue = 0;
        private string mStartEffect = string.Empty;
        private string mKeepEffect = string.Empty;
        private string mBuffDummy = string.Empty;

        #region property
        [EditorProperty("属性号", EditorPropertyType.EEPT_Enum)]
        public Unit.EAttributeType Attr
        {
            get { return mAttr; }
            set { mAttr = value; }
        }
        [EditorProperty("增减值(精度0.001)", EditorPropertyType.EEPT_Int)]
        public int AddValue
        {
            get { return mAddValue; }
            set { mAddValue = value; }
        }
        [EditorProperty("增减比率N%(N的精度0.001)", EditorPropertyType.EEPT_Int)]
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
        [EditorProperty("持续特效", EditorPropertyType.EEPT_String)]
        public string KeepEffect
        {
            get { return mKeepEffect; }
            set { mKeepEffect = value; }
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

            mAttr = JsonHelper.ReadEnum<Unit.EAttributeType>(jd["Attr"]);
            mAddValue = JsonHelper.ReadInt(jd["AddValue"]);
            mMulValue = JsonHelper.ReadInt(jd["MulValue"]);
            mStartEffect = JsonHelper.ReadString(jd["StartEffect"]);
            mKeepEffect = JsonHelper.ReadString(jd["KeepEffect"]);
            mBuffDummy = JsonHelper.ReadString(jd["BuffDummy"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Attr", mAttr.ToString());
            JsonHelper.WriteProperty(ref writer, "AddValue", mAddValue);
            JsonHelper.WriteProperty(ref writer, "MulValue", mMulValue);
            JsonHelper.WriteProperty(ref writer, "StartEffect", mStartEffect);
            JsonHelper.WriteProperty(ref writer, "KeepEffect", mKeepEffect);
            JsonHelper.WriteProperty(ref writer, "BuffDummy", mBuffDummy);

            return writer;
        }
    }

}