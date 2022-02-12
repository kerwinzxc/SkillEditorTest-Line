/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\PingPongAnimator.cs
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
    using System.Collections;

    public class PingPongAnimator : MotionAnimator
    {
        public PingPongAnimator(AttackHit ah, AttackEntity owner, PingPongAnimatorProperty property)
            : base(ah, owner, property)
        {
            PingPongInterpolator inpl = new PingPongInterpolator();

            inpl.Init(property.PingPongTimes, Distance, owner.StartDirection, property.Speed, property.Acc);

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
