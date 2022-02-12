/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\EInterruptType.cs
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
    public enum EInterruptType
    {
        EIT_None        = 0,

        EIT_CheckCustomPropertyBool,
        EIT_CheckCustomPropertyInt,
        EIT_CheckCustomPropertyFloat,
        EIT_CheckCustomPropertyString,
        EIT_CheckCustomPropertyCount,

        EIT_CheckDead,
        EIT_CheckTargetNotNull,
        EIT_CheckTargetIsNull,
        EIT_CheckOnGround,
        EIT_CheckActionState,
        EIT_CheckWeaponType,
        EIT_CheckVelocityY,
        EIT_CheckInputSkillPosition,

        EIT_MAX,
    }
}