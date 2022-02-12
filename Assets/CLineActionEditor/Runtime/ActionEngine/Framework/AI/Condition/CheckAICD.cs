/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\CheckAICD.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-23      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckAICD : AICondition, IProperty
    {
        private bool mIsInCD = false;
        private bool mEnable = false;
        private float mCurTime = 0f;

        [SerializeField] private float mCD = 0f;
        
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

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void OnEvent(AIStatus ai, AISwitch owner, Message msg)
        {

        }

        public bool CheckAI(AIStatus ai, AISwitch owner)
        {
            return !mIsInCD;
        }

        public void OnAIStart(AIStatus ai, AISwitch owner)
        {
            mIsInCD = true;
        }

        public void OnAIEnd(AIStatus ai, AISwitch owner)
        {
            mEnable = true;
        }

        public void OnAIChanging(AIStatus ai, AISwitch owner, AISwitch next)
        {

        }

        public void Update(AIStatus ai, AISwitch owner, float fTick)
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