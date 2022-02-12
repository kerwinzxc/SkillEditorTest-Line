/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EEventType.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;

    [Serializable]
    public enum EEventDataType
    {
        EET_None = 0,
        EET_PlayAnim,
        EET_PlayEffect,
        EET_PlaySound,
        EET_AttackDef,
        EET_Interrupt,
        EET_CameraShake,
        EET_CameraEffect,
        EET_SendMessage,
        EET_AddUnit,
        EET_DelUnit,
        EET_PlayAnimatorAnim,
        EET_ShowTrail,
        EET_HideTrail,
        EET_Move,
        EET_AddBuff,
        EET_WeaponAttack,
        EET_WeaponIdle,
        EET_StopEffect,
        EET_SetCustomPropertyBool,
        EET_SetCustomPropertyInt32,
        EET_SetCustomPropertyFloat,
        EET_SetCustomPropertyString,
        EET_SetCustomPropertyInt32Rand,
        EET_EventHoldBoolTag,
    }

}
