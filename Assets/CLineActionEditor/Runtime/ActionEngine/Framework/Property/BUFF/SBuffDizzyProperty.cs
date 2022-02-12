/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\SBuffDizzyProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-14      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class SBuffDizzyProperty : BuffProperty
    {
        [SerializeField] private int mDizzyTime = 1000;

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