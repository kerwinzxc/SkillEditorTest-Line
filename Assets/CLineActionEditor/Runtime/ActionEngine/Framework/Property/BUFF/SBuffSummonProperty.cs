/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\SBuffSummonProperty.cs
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

    public sealed class SBuffSummonProperty : BuffProperty
    {
        [SerializeField] private bool mUseSummonTime = false;
        [SerializeField] private int mUnitNum = 1;
        [SerializeField] private int mSummonTime = -1;
        [SerializeField] private string mUnitID = string.Empty;
        
        #region property
        [EditorProperty("召唤ID", EditorPropertyType.EEPT_String)]
        public string UnitID
        {
            get { return mUnitID; }
            set { mUnitID = value; }
        }
        [EditorProperty("召唤数量", EditorPropertyType.EEPT_Int)]
        public int UnitNum
        {
            get { return mUnitNum; }
            set { mUnitNum = value; }
        }
        [EditorProperty("是否使用召唤时间", EditorPropertyType.EEPT_Bool)]
        public bool UseSummonTime
        {
            get { return mUseSummonTime; }
            set { mUseSummonTime = value; }
        }
        [EditorProperty("召唤时间(精度ms)", EditorPropertyType.EEPT_Int)]
        public int SummonTime
        {
            get { return mSummonTime; }
            set { mSummonTime = value; }
        }
        #endregion property

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mUnitID = JsonHelper.ReadString(jd["UnitID"]);
            mUnitNum = JsonHelper.ReadInt(jd["UnitNum"]);
            mUseSummonTime = JsonHelper.ReadBool(jd["UseSummonTime"]);
            mSummonTime = JsonHelper.ReadInt(jd["SummonTime"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "UnitID", mUnitID);
            JsonHelper.WriteProperty(ref writer, "UnitNum", mUnitNum);
            JsonHelper.WriteProperty(ref writer, "UseSummonTime", mUseSummonTime);
            JsonHelper.WriteProperty(ref writer, "SummonTime", mSummonTime);

            return writer;
        }
    }
}