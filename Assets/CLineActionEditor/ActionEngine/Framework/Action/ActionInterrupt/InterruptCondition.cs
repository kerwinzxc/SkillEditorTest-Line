/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\InterruptCondition.cs
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

    public interface InterruptCondition
    {
        EInterruptType  InterruptType { get; }
        bool CheckInterrupt(Unit unit);
        void Deserialize(JsonData jd);
        JsonWriter Serialize(JsonWriter writer);
        InterruptCondition Clone();
    }
}