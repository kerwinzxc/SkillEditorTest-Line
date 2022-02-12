/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\AttackHit.cs
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
    using System.Text;
    using System.Collections.Generic;
    using NumericalType = System.Double;

    public class AttackHit : IAttackHit
    {
        public Unit Owner { get; set; }
        public ActionAttackDef Data { get; set; }
        // 发射器
        public EmitComponent Emit { get; set; }
        // 攻击体
        public List<AttackEntity> EntityList { get; set; }
        public List<Unit> TargetList { get; set; }

        private string mActionID = string.Empty;
        private float mDelayTime = 0f;

        public AttackHit(params object[] param)
        {
            mActionID = (string)param[0];
        }

        public void Init()
        {
            mDelayTime = Data.Delay;
            IsDead = false;
            EntityList = new List<AttackEntity>();
            TargetList = new List<Unit>();

            switch (Data.EmitType)
            {
                case EEmitType.EET_Normal:
                    Emit = new NormalEmitComponent(this, Data.EmitProperty as NormalEmitProperty);
                    break;
                case EEmitType.EET_ARC:
                    Emit = new ArcEmitComponent(this, Data.EmitProperty as ArcEmitProperty);
                    break;
            }

        }

        public void AddTarget(Unit unit)
        {
            //lock (TargetList)
            {
                TargetList.Add(unit);
            }
        }

        public override void Update(float fTick)
        {
            if (Owner.IsDead)
            {
                IsDead = true;
                return;
            }

            if (mDelayTime > fTick)
            {
                mDelayTime -= fTick;
            }
            else
            {
                UpdateEmit(fTick);
                UpdateEntity(fTick);
                UpdateHit();
                CheckDead();
            }
        }

        public override void FixedUpdate(float fTick)
        {
            using (List<AttackEntity>.Enumerator itr = EntityList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.FixedUpdate(fTick);
                }
            }
        }

        public AttackEntity CreateAttackEntity(Vector3 pos, Vector3 dir)
        {
            AttackEntity entity = null;

            switch (Data.EntityType)
            {
                case EEntityType.EET_FrameCub:
                case EEntityType.EET_FrameCylinder:
                case EEntityType.EET_FrameFan:
                case EEntityType.EET_FrameRing:
                case EEntityType.EET_FrameSphere:
                    entity = new FrameEntity(this, Data.EntityProperty, pos, dir);
                    break;
                case EEntityType.EET_Physical:
                    entity = new PhysicalEntity(this, Data.EntityProperty, pos, dir);
                    break;
                case EEntityType.EET_Bounce:
                    entity = new BounceEntity(this, Data.EntityProperty, pos, dir);
                    break;
                case EEntityType.EET_Hook:
                    entity = new HookEntity(this, Data.EntityProperty, pos, dir);
                    break;
            }

            if (entity != null)
            {
                EntityList.Add(entity);
            }

            return entity;
        }

        private void UpdateEmit(float fTick)
        {
            Emit.Update(fTick);
        }

        private void UpdateEntity(float fTick)
        {
            using (List<AttackEntity>.Enumerator itr = EntityList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Update(fTick);
                }
            }
        }

        private void UpdateHit()
        {
            using (List<Unit>.Enumerator itr = TargetList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    // check camp later.
                    ProcessHit(Owner, itr.Current);
                }
            }

            //lock (TargetList)
            {
                TargetList.Clear();
            }
        }

        private void ProcessHit(Unit attacker, Unit attackee)
        {
            if (attacker == null || 
                attackee == null || 
                attackee == attacker || 
                attackee.Camp == attacker.Camp || 
                attacker.IsDead || 
                attackee.IsDead || 
                Data == null)
            {
                return;
            }

            if (attackee.Target == null && !attacker.IsDead)
            {
                attackee.Target = attacker;
            }

            //ProcessBuff(attacker, attackee);

            NumericalType damage = 0;
            ECombatResult result = attacker.Combat(attackee, out damage);
            using (List<IProperty>.Enumerator itr = Data.HitFeedbackList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    IHitFeedback fb = itr.Current as IHitFeedback;
                    fb.OnHitFeedback(attacker, attackee, result, damage);
                }
            }
        }

        private void CheckDead()
        {
            if (Data.DeadActionChanged && Owner.ActionStatus.ActiveAction.ID != mActionID)
            {
                IsDead = true;
                return;
            }

            bool dead = true;
            using (List<AttackEntity>.Enumerator itr = EntityList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (!itr.Current.IsDead)
                    {
                        dead = false;
                        break;
                    }
                }
            }

            if (dead)
                IsDead = Emit.IsDead();
        }

        protected override void OnDispose()
        {
            Owner = null;
            Data = null;

            if (Emit != null)
            {
                Emit.Dispose();
                Emit = null;
            }

            using (List<AttackEntity>.Enumerator itr = EntityList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }

            EntityList.Clear();
            TargetList.Clear();
        }
    }

}
