﻿/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\EHitFeedbackType.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-16      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public enum EHitFeedbackType
    {
        EHT_None        = 0,
        EHT_HitDamage,

        EHT_HitAction,

        EHT_HitSound,
        EHT_HitSoundRandom,

        EHT_HitEffect,
        EHT_HitEffectRandom,

        EHT_HitModifyMaterial,
        EHT_HitAddBuff,
        EHT_HitAttackerSpeed,

        EHT_MAX,
    }
}