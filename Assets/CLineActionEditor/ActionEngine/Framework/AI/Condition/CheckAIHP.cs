/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIHP.cs
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

    public sealed class CheckAIHP : AICondition
    {
        private ECompareType mCompareType = ECompareType.ECT_LessEqual;
        private float mCompareVal = 0.3f;

        #region property
        [EditorProperty("比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareType
        {
            get { return mCompareType; }
            set { mCompareType = value; }
        }
        [EditorProperty("HP百分比值", EditorPropertyType.EEPT_Float)]
        public float CompareVal
        {
            get { return mCompareVal; }
            set { mCompareVal = value; }
        }
        #endregion property

        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIHP; }
        }

        public bool CheckAI(Unit unit)
        {
            float hpPercent = (float)(unit.Attrib.CurHP / unit.GetAttribute(Unit.EAttributeType.EAT_MaxHp));

            return CustomCompare<float>.Compare(mCompareType, hpPercent, mCompareVal);
        }

        public void OnAIStart(Unit unit, AISwitch owner)
        {

        }

        public void OnAIEnd(Unit unit, AISwitch owner)
        {

        }

        public void Update(Unit unit, AISwitch owner, float fTick)
        {

        }

        public AICondition Clone()
        {
            CheckAIHP ac = new CheckAIHP();
            ac.CompareType = this.CompareType;
            ac.CompareVal = this.CompareVal;

            return ac;
        }

        public void Deserialize(JsonData jd)
        {
            mCompareType = JsonHelper.ReadEnum<ECompareType>(jd["CompareType"]);
            mCompareVal = JsonHelper.ReadFloat(jd["CompareVal"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareType.ToString());
            JsonHelper.WriteProperty(ref writer, "CompareVal", mCompareVal);

            return writer;
        }
    }
}