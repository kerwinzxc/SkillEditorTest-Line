/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Emit\NormalEmitComponent.cs
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

    public class NormalEmitComponent : EmitComponent
    {
        protected NormalEmitProperty mProperty;
        protected int mCurNum = 0;
        protected float mEmitTimer = 0f;
        protected bool mFirstTime = true;

        protected Vector3 mLockPos;
        protected Vector3 mLockDirection;

        public NormalEmitComponent(AttackHit ah, NormalEmitProperty property)
        {
            AH = ah;
            mProperty = property;

            switch (mProperty.PosType)
            {
                default: break;
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
                        AH.CreateAttackEntity(CalcStartPosition(), CalcStartRotation());
                        mCurNum++;
                        break;
                    case EEmitRuleTpye.EET_SameTime:
                        break;
                    case EEmitRuleTpye.EET_Random:
                        AH.CreateAttackEntity(CalcStartPosition(true), CalcStartRotation());
                        mCurNum++;
                        break;
                    case EEmitRuleTpye.EET_RandomOnNavmesh:
                        AH.CreateAttackEntity(CalcStartPosition(true, true), CalcStartRotation());
                        mCurNum++;
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

            mLockPos = Vector3.zero;
            mLockDirection = Vector3.zero;
        }

        protected virtual Vector3 CalcStartPosition(bool random = false, bool randomonnavmesh = false)
        {
            Vector3 pos = Vector3.zero;
            Vector3 fward = Vector3.zero;
            switch (mProperty.PosType)
            {
                case EEmitterPosType.EEPT_AttackerCurrentPosAndDir:
                    {
                        pos = AH.Owner.Position;
                        fward = AH.Owner.UObject.forward;
                    }
                    break;
                case EEmitterPosType.EEPT_AttackerCurrentPosAndTargetDir:
                    {
                        pos = AH.Owner.Position;
                        fward = AH.Owner.Target != null ? new Vector3(AH.Owner.Target.Position.x, 0f, AH.Owner.Target.Position.z) - new Vector3(AH.Owner.Position.x, 0f, AH.Owner.Position.z) : AH.Owner.UObject.forward;
                    }
                    break;
            }

            if (random)
            {
                Vector2 rpos = Random.insideUnitCircle;
                rpos *= mProperty.EmitOffset.x;
                pos = new Vector3(pos.x + rpos.x, pos.y, pos.z + rpos.y);
                if (randomonnavmesh)
                    pos = Helper.ConvertNavMeshPoint(pos);
            }
            else
            {
                pos += Quaternion.LookRotation(fward) * mProperty.EmitOffset;
            }

            return pos;
        }

        protected virtual Vector3 CalcStartRotation()
        {
            Vector3 direction;
            switch (mProperty.PosType)
            {
                case EEmitterPosType.EEPT_AttackerCurrentPosAndDir:
                    direction = AH.Owner.UObject.forward;
                    break;
                case EEmitterPosType.EEPT_AttackerCurrentPosAndTargetDir:
                    Vector3 pos = AH.Owner.Position + Quaternion.LookRotation(AH.Owner.UObject.forward) * mProperty.EmitOffset;
                    direction = AH.Owner.Target != null ? new Vector3(AH.Owner.Target.Position.x, 0f, AH.Owner.Target.Position.z) - new Vector3(pos.x, 0f, pos.z) : AH.Owner.UObject.forward;
                    break;
                default:
                    direction = AH.Owner.UObject.forward;
                    break;
            }

            if (mProperty.EmitRotation != Vector3.zero)
            {
                Vector3 direuler = Quaternion.LookRotation(direction).eulerAngles;
                direction = Quaternion.Euler(direuler + mProperty.EmitRotation) * Vector3.forward;
            }

            return direction;
        }
    }
}
