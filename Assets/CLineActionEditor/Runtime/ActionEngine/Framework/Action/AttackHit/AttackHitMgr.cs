/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\AttackHitMgr.cs
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
    using System.Collections.Generic;

    public sealed class AttackHitMgr : Singleton<AttackHitMgr>
    {
        private LinkedList<IAttackHit> mAttackHitList = new LinkedList<IAttackHit>();
        private LinkedList<IAttackHit> mHandleList = new LinkedList<IAttackHit>();

        public override void Init()
        {
            
        }

        public override void Destroy()
        {
            using (var itr = mAttackHitList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }

            mAttackHitList.Clear();
            mHandleList.Clear();
        }

        public AttackHit Create(Unit owner, ActionAttackDef data, params object[] param)
        {
            AttackHit ah = null;
            switch (data.AttackHitType)
            {
                case EAttackHitType.EAHT_Normal:
                    ah = new AttackHit(param) { Owner = owner, Data = data };
                    break;
                default:
                    break;
            }

            if (ah != null)
            {
                ah.Init();
                mAttackHitList.AddLast(ah);
            }

            return ah;
        }

        public void Update(float fTick)
        {
            using (var itr = mAttackHitList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.IsDead)
                        mHandleList.AddLast(itr.Current);
                    else
                        itr.Current.Update(fTick);
                }
            }

            using (var itr = mHandleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                    mAttackHitList.Remove(itr.Current);
                }
            }

            mHandleList.Clear();
        }

        public void FixedUpdate(float fTick)
        {
            using (var itr = mAttackHitList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.FixedUpdate(fTick);
                }
            }
        }

        
    }

}
