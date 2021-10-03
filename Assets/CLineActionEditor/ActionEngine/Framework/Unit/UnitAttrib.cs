/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\UnitAttrib.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-18      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public sealed class UnitAttrib
    {
        public double BaseHP
        {
            get; set;
        }

        public double CurHP
        {
            get; set;
        }

        public float BaseSpeed
        {
            get; set;
        }

        public float CurSpeed
        {
            get; set;
        }

        public UnitAttrib()
        {
            CurHP = 0;
            CurSpeed = 0;
        }
    }
}
