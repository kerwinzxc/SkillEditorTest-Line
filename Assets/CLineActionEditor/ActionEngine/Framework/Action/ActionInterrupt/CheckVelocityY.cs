/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckVelocityY.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-18      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckVelocityY : InterruptCondition
    {
        private float mVelocityY;
        private ECompareType mCompareVelocityY;

        [EditorProperty("Y向速度比较值", EditorPropertyType.EEPT_Float)]
        public float VelocityY
        {
            get { return mVelocityY; }
            set { mVelocityY = value; }
        }

        [EditorProperty("Y向速度比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareVelocityY
        {
            get { return mCompareVelocityY; }
            set { mCompareVelocityY = value; }
        }

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckVelocityY;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            Debug.Assert(unit.CustomPropertyHash[CustomProperty.sVelocityY] != null, "'sVelocityY' is not defined.");
            return CustomCompare<float>.Compare(CompareVelocityY, (float)unit.CustomPropertyHash[CustomProperty.sVelocityY].Value, VelocityY);
        }

        public void Deserialize(JsonData jd)
        {
            mVelocityY = JsonHelper.ReadFloat(jd["VelocityY"]);
            mCompareVelocityY = JsonHelper.ReadEnum<ECompareType>(jd["CompareVelocityY"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "VelocityY", mVelocityY);
            JsonHelper.WriteProperty(ref writer, "CompareVelocityY", mCompareVelocityY.ToString());

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckVelocityY obj = new CheckVelocityY();
            obj.VelocityY = this.VelocityY;
            obj.CompareVelocityY = this.CompareVelocityY;

            return obj;
        }
    }
}