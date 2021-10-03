/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EEventType.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public enum EEventType
    {
        EET_None = 0,
        EET_PlayAnim,
        EET_PlayEffect,
        EET_PlaySound,
        EET_CameraShake,
        EET_CameraEffect,
        EET_ActionBreak,
        EET_SendMessage,
        EET_AddUnit,
        EET_DelUnit,
        EET_PlayAnimatorAnim,
        EET_ShowTrail,
        EET_HideTrail,
        EET_Move,
        EET_AddBuff,
        EET_UpdateMaterial,
        EET_InvokeFunction,
        EET_AttackDef,
        EET_WeaponAttack,
        EET_WeaponIdle,
        EET_SetCustomProperty,
        EET_StopEffect,
    }

}
