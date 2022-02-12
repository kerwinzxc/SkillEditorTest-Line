/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Emit\ArcEmitComponent.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public class ArcEmitComponent : EmitComponent
    {
        private ArcEmitProperty mProperty;
        private int mCurNum = 0;
        private float mEmitTimer = 0f;
        private bool mFirstTime = true;

        private EArcEmitType mEmitType;

        private int mIntervalDegree;
        private float mRadiusOffset;
        private Vector3 mPosOffset;
        private int mStarDegree;

        private static int[] sRandomDegree = { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330 };

        public ArcEmitComponent(AttackHit ah, ArcEmitProperty property)
        {
            AH = ah;
            mProperty = property;
            mEmitType = property.EmitType;
            mRadiusOffset = property.RadiusOffset;
            mPosOffset = property.PosOffset;

            if (mEmitType == EArcEmitType.EAE_Circle || mEmitType == EArcEmitType.EAE_Circle_RelativeSelfDir)
            {
                int idx = Random.Range(0, sRandomDegree.Length);
                mStarDegree = sRandomDegree[idx];
                mIntervalDegree = 360 / mProperty.Num;
            }
            else
            {
                mIntervalDegree = mProperty.IntervalDegree;
                mStarDegree = -(mProperty.IntervalDegree * (mProperty.Num - 1)) / 2;
            }
        }

        public override bool IsDead()
        {
            return mCurNum == mProperty.Num;
        }

        public override void Update(float fTick)
        {
            if (IsDead()) return;

            if (mFirstTime || mEmitTimer >= mProperty.Interval)
            {
                switch (mProperty.Type)
                {
                    case EEmitRuleTpye.EET_Interval:
                        {
                            AH.CreateAttackEntity(CalcStartPosition(), CalcStartRotation());
                            mCurNum++;
                        }
                        break;
                    case EEmitRuleTpye.EET_SameTime:
                        {
                            for (int i = 0; i < mProperty.Num; ++i)
                            {
                                AH.CreateAttackEntity(CalcStartPosition(), CalcStartRotation());
                                mCurNum++;
                            }
                        }
                        break;
                }

                if (mFirstTime)
                    mFirstTime = false;
                else
                    mEmitTimer -= mProperty.Interval;
            }
            else
            {
                mEmitTimer += fTick;
            }
        }

        protected override void OnDispose()
        {
            AH = null;
            mProperty = null;
        }

        private Vector3 CalcStartPosition()
        {
            Vector3 dir = CalcStartRotation();
            Vector3 pos = Vector3.zero;
            Vector3 fwd = Vector3.zero;
            switch (mProperty.PosType)
            {
                default:
                    pos = AH.Owner.Position;
                    fwd = AH.Owner.UObject.forward;
                    break;
            }
            return pos + Quaternion.LookRotation(fwd) * mPosOffset + dir * mRadiusOffset;

        }

        private Vector3 CalcStartRotation()
        {
            Vector3 direction = (AH.Owner.Target != null && mEmitType != EArcEmitType.EAE_Sector_RelativeSelfDir && mEmitType != EArcEmitType.EAE_Circle_RelativeSelfDir ? AH.Owner.Target.Position - AH.Owner.Position : AH.Owner.UObject.forward);
            direction = new Vector3(direction.x, 0, direction.z);
            direction = direction.normalized;

            // mCurNum 
            Quaternion qat = Quaternion.Euler(0, mStarDegree + mCurNum * mIntervalDegree, 0);
            return qat * direction;
        }
    }

}
