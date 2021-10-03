/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\SBuffResetSkillCD.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-14      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public sealed class SBuffResetSkillCD : Buff
    {
        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            MessageMgr.Instance.SendMessage(new Message("EVT_RESET_SKILLCD"));

            return false;
        }
    }
}