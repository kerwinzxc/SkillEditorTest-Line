﻿/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\DBuffProperty.cs
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

    public sealed class DBuffProperty : BuffProperty
    {
        private int mInterval = 0;
        private string mAddBuffID = string.Empty;

        #region property
        [EditorProperty("间隔时间", EditorPropertyType.EEPT_Int)]
        public int Interval
        {
            get { return mInterval; }
            set { mInterval = value; }
        }
        [EditorProperty("触发BUFF", EditorPropertyType.EEPT_String)]
        public string AddBuffID
        {
            get { return mAddBuffID; }
            set { mAddBuffID = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mInterval = JsonHelper.ReadInt(jd["Interval"]);
            mAddBuffID = JsonHelper.ReadString(jd["AddBuffID"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Interval", mInterval);
            JsonHelper.WriteProperty(ref writer, "AddBuffID", mAddBuffID);

            return writer;
        }
    }
}