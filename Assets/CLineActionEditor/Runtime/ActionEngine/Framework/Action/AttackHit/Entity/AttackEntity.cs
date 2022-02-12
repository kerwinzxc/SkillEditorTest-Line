/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\AttackEntity.cs
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
    using System.Collections.Generic;

    public abstract class AttackEntity : XObject
    {
        private float mDelayTime = 0f;
        private bool mBuildFinished = false;

        public AttackEntity(AttackHit ah, AttackEntityProperty property, Vector3 pos, Vector3 dir)
        {
            AH = ah;
            Property = property;
            StartPosition = pos;
            StartDirection = dir;

            IsDead = false;
            EnableAnimator = false;
            mDelayTime = Property.Delay;

            Animator = null;
            switch (AH.Data.MotionAnimatorType)
            {
                case EMotionAnimatorType.EMAT_Line:
                    Animator = new LineAnimator(AH, this, AH.Data.MotionAnimatorProperty as LineAnimatorProperty);
                    break;
                case EMotionAnimatorType.EMAT_Curve:
                    Animator = new CurveAnimator(AH, this, AH.Data.MotionAnimatorProperty as CurveAnimatorProperty);
                    break;
                case EMotionAnimatorType.EMAT_PingPong:
                    Animator = new PingPongAnimator(ah, this, AH.Data.MotionAnimatorProperty as PingPongAnimatorProperty);
                    break;
            }
        }

        protected override void OnDispose()
        {
            if (Animator != null)
            {
                Animator.Dispose();
                Animator = null;
            }

            StartDirection = Vector3.zero;
            StartPosition = Vector3.zero;

            Property = null;
            AH = null;
        }

        public AttackHit AH { get; set; }
        public AttackEntityProperty Property { get; set; }
        public MotionAnimator Animator { get; set; }
        public Vector3 StartPosition { get; set; }
        public Vector3 StartDirection { get; set; }
        public bool IsDead { get; set; }
        public bool EnableAnimator { get; set; }

        public virtual void BuildEntity()
        {
            if (!string.IsNullOrEmpty(Property.Effect))
            {
                Quaternion qat = Helper.LookRotation(StartDirection);
                EffectMgr.Instance.PlayEffect(Property.Effect, StartPosition, qat);
            }

            if (!string.IsNullOrEmpty(Property.Sound))
            {
                AudioMgr.Instance.PlaySound(Property.Sound, Property.SoundCount);
            }            
        }

        public virtual bool Update(float fTick)
        {
            if (IsDead) return false;

            if (mDelayTime > fTick)
            {
                mDelayTime -= fTick;
                return false;
            }
            else
            {
                if (!mBuildFinished)
                {
                    mBuildFinished = true;
                    BuildEntity();
                }

                EnableAnimator = true;

                return true;
            }
        }

        public virtual bool FixedUpdate(float fTick)
        {
            return !IsDead;
        }

        // network 
        public virtual void ProcessEntityNet(params object[] param)
        {

        }
    }

}
