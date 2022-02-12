/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\EBuffConditionType.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-24      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    public enum EBuffConditionType
    {
        ECT_None        = 0,
        ECT_CheckBuffBeforeAttackCount, //N次攻击时
        ECT_CheckBuffAfterAttackCount,  //N次攻击后
        ECT_CheckBuffBeforeUseSkill,    //技能释放时
        ECT_CheckBuffAfterUseSkill,     //技能释放后
        ECT_CheckBuffHitCount,          //N次受击后
        ECT_CheckBuffCombos,            //N次连击后
        ECT_CheckBuffCriticalCount,     //N次暴击后
        ECT_CheckBuffKillMonster,       //消灭N个敌人后
        ECT_CheckBuffHP,                //HP低于H后
        ECT_CheckBuffEvade,             //闪避后
        ECT_CheckBuffSummon,            //召唤时

        ECT_MAX,
    }
}