/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\PhysicalEntity.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-16      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public sealed class PhysicalEntity : AttackEntity
    {
        private GameObject mEntity;

        private int mCurPassNum = 1;

        public PhysicalEntity(AttackHit ah, AttackEntityProperty property, Vector3 pos, Vector3 dir)
            : base(ah, property, pos, dir)
        {

        }

        public override void BuildEntity()
        {
            base.BuildEntity();

            PhysicalEntityProperty p = Property as PhysicalEntityProperty;

            mEntity = ObjectPoolMgr.Instance.GetPool(p.ModlePrefab).Get(StartPosition, Helper.LookRotation(StartDirection));

            UPhysicsAction pa = mEntity.GetComponent<UPhysicsAction>();
            if (pa == null)
                pa = mEntity.AddComponent<UPhysicsAction>();
            pa.OnCollisionEnterAction = OnCollisionEnter;
            pa.OnTriggerEnterAction = OnTriggerEnter;
        }

        protected override void OnDispose()
        {
            DestroyEntity(false);

            base.OnDispose();
        }

        public override bool Update(float fTick)
        {
            if (!base.Update(fTick)) return false;

            if (Animator != null)
            {
                Animator.Update(fTick);
                IsDead = Animator.IsDead;

                if (IsDead && null != mEntity)
                {
                    DestroyEntity(true);
                }
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

            if (Animator != null && EnableAnimator)
            {
                Animator.FixedUpdate(fTick);

                switch (Animator.Inpl.InplType)
                {
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

        private void DestroyEntity(bool playEffect)
        {
            if (mEntity != null)
            {
                PhysicalEntityProperty p = Property as PhysicalEntityProperty;

                if (playEffect && !string.IsNullOrEmpty(p.EntityEndEffect))
                {
                    EffectMgr.Instance.PlayEffect(p.EntityEndEffect, mEntity.transform.position, Quaternion.identity);
                }

                ObjectPoolMgr.Instance.GetPool(p.ModlePrefab).Cycle(mEntity);

                mEntity = null;
                mCurPassNum = 0;
            }
        }

        private void ProcessEntity(Transform collideTransform)
        {
            string collideLayer = LayerMask.LayerToName(collideTransform.gameObject.layer);
            if (collideLayer == "Role")
            {
                UUnit uunit = collideTransform.GetComponent<UUnit>();
                if (uunit != null && uunit.Owner.Camp != AH.Owner.Camp)
                {
                    PhysicalEntityProperty p = Property as PhysicalEntityProperty;
                    uunit.Owner.HitPosition = mEntity.transform.position;

                    if (mCurPassNum >= p.MaxPassNum)
                    {
                        DestroyEntity(true);
                        IsDead = true;
                    }

                    mCurPassNum++;
                    AH.TargetList.Add(uunit.Owner);
                }
            }
            else
            {
                Vector3 entityPosition = mEntity.transform.position;
                PhysicalEntityProperty p = Property as PhysicalEntityProperty;

                if (mCurPassNum >= p.MaxPassNum)
                {
                    DestroyEntity(true);
                    IsDead = true;

                    //just like phy_bullet to get a effect when to attack wall or class
                    int idx = p.EntityColliderOtherEffectLayerNameList.FindIndex((string layername) => { return layername.Equals(collideLayer); });
                    if (idx != -1 && p.EntityColliderOtherEffectLayerEffectList.Count > idx)
                    {
                        EffectMgr.Instance.PlayEffect(p.EntityColliderOtherEffectLayerEffectList[idx], entityPosition, Quaternion.identity);
                    }
                }

                mCurPassNum++;
            }
        }

    }

}
