/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\SBuffSummon.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-14      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    public sealed class SBuffSummon : Buff
    {
        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            SBuffSummonProperty p = mProperty as SBuffSummonProperty;
            Unit unit = UnitMgr.Instance.CreateUnit(EUnitType.EUT_Summon, p.UnitID, mgr.Owner.Position, 0, mgr.Owner.Camp);
            if (p.UseSummonTime)
            {
                mgr.Owner.AddChild(unit, p.SummonTime * mPrecision);
            }
            else
            {
                mgr.Owner.AddChild(unit, -1);
            }

            return false;
        }
    }
}