/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\SBuffDizzyProperty.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-14      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class SBuffDizzyProperty : BuffProperty
    {
        private int mDizzyTime = 1000;

        #region property
        [EditorProperty("眩晕时间(精度ms)", EditorPropertyType.EEPT_Int)]
        public int DizzyTime
        {
            get { return mDizzyTime; }
            set { mDizzyTime = value; }
        }
        #endregion property

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mDizzyTime = JsonHelper.ReadInt(jd["DizzyTime"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "DizzyTime", mDizzyTime);

            return writer;
        }
    }
}