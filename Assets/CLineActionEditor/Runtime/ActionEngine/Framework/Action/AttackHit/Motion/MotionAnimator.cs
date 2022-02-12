/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\MotionAnimator.cs
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

    public abstract class MotionAnimator : XObject
    {
        public AttackHit AH { get; set; }
        public AttackEntity Owner { get; set; }
        public MotionAnimatorProperty Property { get; set; }
        public Interpolator Inpl { get; set; }
        public bool IsDead { get; set; }


        public MotionAnimator(AttackHit ah, AttackEntity owner, MotionAnimatorProperty property)
        {
            AH = ah;
            Owner = owner;
            Property = property;
            Inpl = null;
            IsDead = false;
        }

        public virtual void Update(float fTick)
        {

        }

        public virtual void FixedUpdate(float fTick)
        {

        }

        public virtual Vector3 GetInplDisplace()
        {
            return Vector3.zero;
        }

        protected override void OnDispose()
        {
            AH = null;
            Owner = null;
            Property = null;
            Inpl = null;
        }

        public float Distance
        {
            get
            {
                return Property.Distance;
            }
        }
    }

}
