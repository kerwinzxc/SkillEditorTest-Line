/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckDead.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-18      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;

    public sealed class CheckDead : InterruptCondition, IProperty
    {
        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckDead;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
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