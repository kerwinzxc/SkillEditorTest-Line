/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\BuffCondition.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-24      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public interface BuffCondition
    {
        EBuffConditionType BuffCondType { get; }
        void Deserialize(JsonData jd);
        JsonWriter Serialize(JsonWriter writer);

        BuffCondition Clone();
        void Init(CBuff owner);
        void Destroy();
        void Reset();
        bool CheckBuff(Unit unit);
    }
}