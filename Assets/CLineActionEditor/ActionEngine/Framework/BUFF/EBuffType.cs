﻿/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\EBuffType.cs
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
    public enum EBuffType
    {
        EBT_None            = 0,
        EBT_NumericalBuff,
        EBT_DeltaBuff,
        EBT_ConditionBuff,

        EBT_SpecialBuffHP,
        EBT_SpecialBuffDizzy,
        EBT_SpecialBuffGodMode,
        EBT_SpecialBuffResetSkillCD,
        EBT_SpecialBuffSummon,

        EBT_MAX,
    }
}