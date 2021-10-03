/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Emit\EmitComponent.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public abstract class EmitComponent : XObject
    {
        public AttackHit AH { get; set; }
        public abstract bool IsDead();
        public abstract void Update(float fTick);
    }
}
