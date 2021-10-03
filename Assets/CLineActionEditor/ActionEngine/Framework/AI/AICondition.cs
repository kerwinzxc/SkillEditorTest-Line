/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\AICondition.cs
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

    public interface AICondition
    {
        EAIConditionType AIType { get; }
        void Deserialize(JsonData jd);
        JsonWriter Serialize(JsonWriter writer);

        AICondition Clone();
        void Update(Unit unit, AISwitch owner, float fTick);
        void OnAIStart(Unit unit, AISwitch owner);
        void OnAIEnd(Unit unit, AISwitch owner);
        bool CheckAI(Unit unit);
    }
}