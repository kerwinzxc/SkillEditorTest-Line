/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAITargetIsNull.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-13      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class CheckAITargetIsNull : AICondition
    {
        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAITargetIsNull; }
        }

        public bool CheckAI(Unit unit)
        {
            return unit.Target == null;
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
            CheckAITargetIsNull ac = new CheckAITargetIsNull();
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