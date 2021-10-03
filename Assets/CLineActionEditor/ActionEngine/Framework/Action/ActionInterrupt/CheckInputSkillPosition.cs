/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckInputSkillPosition.cs
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
    using UnityEngine;

    public sealed class CheckInputSkillPosition : InterruptCondition
    {
        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckInputSkillPosition;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            Debug.Assert(unit.CustomPropertyHash[CustomProperty.sInputSkillPosition] != null, "'sInputSkillPosition' is not defined.");
            Vector3 pos = (Vector3)unit.CustomPropertyHash[CustomProperty.sInputSkillPosition].Value;
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