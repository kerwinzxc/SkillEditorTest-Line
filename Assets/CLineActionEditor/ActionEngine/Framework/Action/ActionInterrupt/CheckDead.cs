/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckDead.cs
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

    public sealed class CheckDead : InterruptCondition
    {
        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckDead;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            return unit.IsDead;
        }

        public void Deserialize(JsonData jd)
        {
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return writer;
        }

        public InterruptCondition Clone()
        {
            return new CheckDead();
        }
    }
}