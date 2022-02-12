/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\EAIConditionType.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-23      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    public enum EAIConditionType
    {
        EAT_None    = 0,
        EAT_CheckAICD,
        EAT_CheckAIHP,
        EAT_CheckAIBornPos,
        EAT_CheckAITargetIsNull,
        EAT_CheckAITargetNotNull,
        EAT_CheckAIDead,
        EAT_CheckAIDistCustomProperty,
        EAT_CheckAIDistMinMax,
        EAT_CheckAIChase,
        EAT_CheckAIPatrol,

        EAT_MAX,
    }
}