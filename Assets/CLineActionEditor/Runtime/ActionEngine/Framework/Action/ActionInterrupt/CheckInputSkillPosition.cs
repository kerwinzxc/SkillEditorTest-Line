/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckInputSkillPosition.cs
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
    using UnityEngine;

    public sealed class CheckInputSkillPosition : InterruptCondition, IProperty
    {
        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckInputSkillPosition;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            Vector3 pos = Helper.GetAny<Vector3>(unit.PropertyContext.GetProperty(PropertyName.sInputSkillPosition));
            return !pos.Equals(Vector3.positiveInfinity);
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
            return new CheckInputSkillPosition();
        }
    }
}