/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\LineAnimator.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    public sealed class LineAnimator : MotionAnimator
    {
        public LineAnimator(AttackHit ah, AttackEntity owner, LineAnimatorProperty property)
            : base(ah, owner, property)
        {
            LineInterpolator inpl = new LineInterpolator();
            inpl.Init(Distance, owner.StartDirection, property.Speed, property.Acc);

            Inpl = inpl;
        }

        public override void Update(float fTick)
        {
            if (Inpl.IsFinished())
            {
                IsDead = true;
            }
        }

        public override void FixedUpdate(float fTick)
        {
            Inpl.Interpolate(fTick);
        }
    }

}
