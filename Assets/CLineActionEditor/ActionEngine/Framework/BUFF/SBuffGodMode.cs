/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\SBuffGodMode.cs
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
    public sealed class SBuffGodMode : Buff
    {
        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            SBuffGodModeProperty p = mProperty as SBuffGodModeProperty;
            mgr.Owner.ChangeGodMode(true, p.GodTime * mPrecision);

            return false;
        }
    }
}