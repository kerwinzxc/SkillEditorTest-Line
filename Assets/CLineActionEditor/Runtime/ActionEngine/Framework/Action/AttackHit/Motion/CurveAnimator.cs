/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\CurveAnimator.cs
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
    using UnityEngine;

    public sealed class CurveAnimator : MotionAnimator
    {
        public CurveAnimator(AttackHit ah, AttackEntity owner, CurveAnimatorProperty property)
            : base(ah, owner, property)
        {
            CatmullRomCurveInterpolator inpl = new CatmullRomCurveInterpolator();

            float curveHeightCoeff = property.CurveHeightCoeff;

            Vector3 endPos = Vector3.zero;
            if (property.UseTargetDist && ah.Owner.Target != null && Vector3.Distance(ah.Owner.Position, ah.Owner.Target.Position) <= Distance)
            {
                endPos = ah.Owner.Target.Position;
            }
            else
            {
                float dis = Distance;
                endPos = ah.Owner.Position + owner.StartDirection * dis;
            }

            inpl.Init(owner.StartPosition, endPos, property.Speed, curveHeightCoeff);
            Inpl = inpl;
        }

        public override void Update(float fTick)
        {
            if (Inpl.IsFinished())
                IsDead = true;
        }

        public override void FixedUpdate(float fTick)
        {
            Inpl.Interpolate(fTick);
        }
    }

}
