/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\AICondition.cs
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
    public interface AICondition
    {
        EAIConditionType AIType { get; }
        AICondition Clone();

        void OnEvent(AIStatus ai, AISwitch owner, Message msg);
        bool CheckAI(AIStatus ai, AISwitch owner);
        void OnAIStart(AIStatus ai, AISwitch owner);
        void OnAIEnd(AIStatus ai, AISwitch owner);
        void OnAIChanging(AIStatus ai, AISwitch owner, AISwitch next);
        void Update(AIStatus ai, AISwitch owner, float fTick);
    }
}