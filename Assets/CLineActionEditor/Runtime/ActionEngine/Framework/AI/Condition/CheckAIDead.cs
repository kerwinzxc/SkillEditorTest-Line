/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIDead.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-13      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;

    public sealed class CheckAIDead : AICondition, IProperty
    {
        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIDead; }
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
            return ai.Owner.IsDead;
        }

        public void OnAIStart(AIStatus ai, AISwitch owner)
        {

        }

        public void OnAIEnd(AIStatus ai, AISwitch owner)
        {

        }

        public void OnAIChanging(AIStatus ai, AISwitch owner, AISwitch next)
        {

        }

        public void Update(AIStatus ai, AISwitch owner, float fTick)
        {

        }

        public AICondition Clone()
        {
            CheckAIDead ac = new CheckAIDead();
            return ac;
        }

        public void Deserialize(JsonData jd)
        {

        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return writer;
        }
    }
}