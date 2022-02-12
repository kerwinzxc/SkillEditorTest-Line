/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\HookEntity.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-17      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public class HookEntity : AttackEntity
    {
        private GameObject mEntity;
        private GameObject mFollowEffect;
        private bool mUnitIsFollow;
        private List<Unit> mCollidedUnit = new List<Unit>();
        private List<Unit> mReleaseUnit = new List<Unit>();

        public HookEntity(AttackHit ah, AttackEntityProperty property, Vector3 pos, Vector3 dir)
            : base(ah, property, pos, dir)
        {

        }

        public override void BuildEntity()
        {
            base.BuildEntity();

            HookEntityProperty p = Property as HookEntityProperty;
            mUnitIsFollow = p.IsUnitFollow;

            mEntity = ObjectPoolMgr.Instance.GetPool(p.ModelPrefab).Get(StartPosition, Helper.LookRotation(StartDirection));

            if (!string.IsNullOrEmpty(p.FollowEffect))
            {
                mFollowEffect = ObjectPoolMgr.Instance.GetPool(p.FollowEffect).Get();
                Helper.AddChild(mFollowEffect, mEntity);
            }

            UPhysicsAction pa = mEntity.GetComponent<UPhysicsAction>();
            if (pa == null)
                pa = mEntity.AddComponent<UPhysicsAction>();
            pa.OnTriggerEnterAction = OnTriggerEnter;
            pa.OnCollisionEnterAction = OnCollisionEnter;
        }

        protected override void OnDispose()
        {
            HookEntityProperty p = Property as HookEntityProperty;
            if (mEntity != null)
            {
                ObjectPoolMgr.Instance.GetPool(p.ModelPrefab).Cycle(mEntity);
                mEntity = null;
            }

            if (mFollowEffect != null)
            {
                ObjectPoolMgr.Instance.GetPool(p.FollowEffect).Cycle(mFollowEffect);
                mFollowEffect = null;
            }

            mCollidedUnit.Clear();
            mReleaseUnit.Clear();

            base.OnDispose();
        }

        public override bool Update(float fTick)
        {
            if (!base.Update(fTick)) return false;

            if (Animator != null)
            {
                Animator.Update(fTick);
                IsDead = Animator.IsDead;
            }
            else
            {
                IsDead = true;
            }

            return true;
        }

        public override bool FixedUpdate(float fTick)
        {
            if (!base.FixedUpdate(fTick)) return false;

            if (EnableAnimator &&  Animator != null)
            {
                Animator.FixedUpdate(fTick);

                switch (Animator.Inpl.InplType)
                {
                    case EInterpolatorType.EIT_PingPong:
                        mEntity.transform.position += Animator.Inpl.Displace;
                        break;
                    case EInterpolatorType.EIT_Line:
                        mEntity.transform.position += Animator.Inpl.Displace;
                        mEntity.transform.rotation = Animator.Inpl.Rotation;
                        break;
                    case EInterpolatorType.EIT_CatmullRom:
                        mEntity.transform.position = Animator.Inpl.Displace;
                        mEntity.transform.rotation = Animator.Inpl.Rotation;
                        break;
                    case EInterpolatorType.EIT_None:
                        mEntity.transform.position = Animator.GetInplDisplace();
                        break;
                }

                if (mUnitIsFollow)
                {
                    HookEntityProperty p = Property as HookEntityProperty;
                    using (List<Unit>.Enumerator itr = mCollidedUnit.GetEnumerator())
                    {
                        while (itr.MoveNext())
                        {
                            itr.Current.SetPosition(new Vector3(mEntity.transform.position.x, itr.Current.Position.y, mEntity.transform.position.z));
                            float dist = Vector3.Distance(itr.Current.Position, StartPosition) - itr.Current.Radius - AH.Owner.Radius;
                            if (dist <= p.UnitStopDistance)
                                mReleaseUnit.Add(itr.Current);
                        }
                    }
                    using (List<Unit>.Enumerator itrRelease = mReleaseUnit.GetEnumerator())
                    {
                        while (itrRelease.MoveNext())
                        {
                            mCollidedUnit.Remove(itrRelease.Current);
                        }
                    }
                    mReleaseUnit.Clear();
                }
            }

            return true;
        }

        private void OnCollisionEnter(Collision collision, Transform transform)
        {
            if (!IsDead && collision.transform != null && collision.transform != AH.Owner.UObject.transform)
            {
                ProcessEntity(collision.transform);
            }
        }

        private void OnTriggerEnter(Collider collider, Transform transform)
        {
            ProcessEntity(collider.transform);
        }

        private void ProcessEntity(Transform collideTransform)
        {
            if (LayerMask.LayerToName(collideTransform.gameObject.layer) == "Role")
            {
                UUnit uunit = collideTransform.GetComponent<UUnit>();
                if (uunit != null && uunit.Owner.Camp != AH.Owner.Camp)
                {
                    if (Animator.Inpl.InplType.Equals(EInterpolatorType.EIT_PingPong))
                    {
                        PingPongInterpolator inpl = Animator.Inpl as PingPongInterpolator;
                        inpl.SetTrigger();
                    }

                    mCollidedUnit.Add(uunit.Owner);

                    AH.TargetList.Add(uunit.Owner);
                }
            }
        }

    }

}
