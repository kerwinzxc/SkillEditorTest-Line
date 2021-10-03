/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\CheckAICD.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-23      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class CheckAICD : AICondition
    {
        private bool mIsInCD = false;
        private bool mEnable = false;
        private float mCurTime = 0f;

        private float mCD = 0f;
        
        [EditorProperty("CD时间(精度s)", EditorPropertyType.EEPT_Float)]
        public float CD
        {
            get { return mCD; }
            set { mCD = value; }
        }

        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAICD; }
        }

        public bool CheckAI(Unit unit)
        {
            return !mIsInCD;
        }

        public void OnAIStart(Unit unit, AISwitch owner)
        {
            mIsInCD = true;
        }

        public void OnAIEnd(Unit unit, AISwitch owner)
        {
            mEnable = true;
        }

        public void Update(Unit unit, AISwitch owner, float fTick)
        {
            if (mEnable)
            {
                mCurTime += fTick;
                if (mCurTime >= mCD)
                {
                    mEnable = false;
                    mIsInCD = false;
                    mCurTime = 0f;
                }
            }
        }

        public AICondition Clone()
        {
            CheckAICD ac = new CheckAICD();
            ac.CD = CD;

            return ac;
        }

        public void Deserialize(JsonData jd)
        {
            mCD = JsonHelper.ReadFloat(jd["CD"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "CD", mCD);

            return writer;
        }
    }
}