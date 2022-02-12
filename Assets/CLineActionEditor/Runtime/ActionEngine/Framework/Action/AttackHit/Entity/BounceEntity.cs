/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\BounceEntity.cs
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

    public class BounceEntity : AttackEntity
    {
        private GameObject mEntity;
        private int mCurTimes = 0;

        public BounceEntity(AttackHit ah, AttackEntityProperty property, Vector3 pos, Vector3 dir)
            : base(ah, property, pos, dir)
        {

        }

        public override void BuildEntity()
        {
            base.BuildEntity();

            BounceEntityProperty p = Property as BounceEntityProperty;
            mEntity = ObjectPoolMgr.Instance.GetPool(p.ModelPrefab).Get(StartPosition, Helper.LookRotation(StartDirection));

            UPhysicsAction pa = mEntity.GetComponent<UPhysicsAction>();
            if (pa == null)
                pa = mEntity.AddComponent<UPhysicsAction>();
            pa.OnTriggerEnterAction = OnTriggerEnter;
            pa.OnCollisionEnterAction = OnCollisionEnter;
        }

        public override bool Update(float fTick)
        {
            if (!base.Update(fTick)) return false;

            if (null != Animator)
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

            if (EnableAnimator && null != Animator)
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

        protected override void OnDispose()
        {
            if (mEntity != null)
            {
                BounceEntityProperty p = Property as BounceEntityProperty;
                ObjectPoolMgr.Instance.GetPool(p.ModelPrefab).Cycle(mEntity);

                mEntity = null;
            }

            mCurTimes = 0;

            base.OnDispose();
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
            BounceEntityProperty p = Property as BounceEntityProperty;
            if (mCurTimes >= p.BounceTimes) return;

            if (LayerMask.LayerToName(collideTransform.gameObject.layer) == "Role")
            {
                UUnit uunit = collideTransform.GetComponent<UUnit>();
                if (uunit != null && uunit.Owner.Camp != AH.Owner.Camp)
                {
                    Unit[] units = Helper.Search(uunit.Owner.Position, p.BounceRandomRadius + uunit.Owner.Radius, LayerMask.GetMask("Role"), new Transform[] { uunit.transform }, AH.Owner.Camp, true, Vector3.zero);
                    if (units.Length > 0)
                    {
                        switch (Animator.Inpl.InplType)
                        {
                            case EInterpolatorType.EIT_Line:
                                {
                                    LineAnimatorProperty lip = Animator.Property as LineAnimatorProperty;
                                    Animator.Inpl.Reset();

                                    LineInterpolator li = Animator.Inpl as LineInterpolator;
                                    li.Init(uunit.Owner.CenterPosition, units[0].CenterPosition, lip.Speed, lip.Acc);
                                }
                                break;
                            case EInterpolatorType.EIT_CatmullRom:
                                {
                                    CurveAnimatorProperty cip = Animator.Property as CurveAnimatorProperty;
                                    Animator.Inpl.Reset();

                                    CatmullRomCurveInterpolator ci = Animator.Inpl as CatmullRomCurveInterpolator;
                                    ci.Init(uunit.Owner.CenterPosition, units[0].CenterPosition, cip.Speed, cip.CurveHeightCoeff);
                                }
                                break;
                        }
                    }
                    else
                    {
                        IsDead = true;
                    }

                    mCurTimes++;

                    // do hurt logic at later.
                    AH.TargetList.Add(uunit.Owner);
                }
            }
        }

        
    }

}
