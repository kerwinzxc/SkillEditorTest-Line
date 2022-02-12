/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckVelocityY.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-18      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckVelocityY : InterruptCondition, IProperty
    {
        [SerializeField] private float mVelocityY;
        [SerializeField] private ECompareType mCompareVelocityY;

        #region property
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
        #endregion

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckVelocityY;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            float v = Helper.GetAny<float>(unit.PropertyContext.GetProperty(PropertyName.sVelocityY));
            return CustomCompare<float>.Compare(CompareVelocityY, v, VelocityY);
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