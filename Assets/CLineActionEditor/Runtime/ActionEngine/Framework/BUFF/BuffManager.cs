/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\BuffManager.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-24      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System.Collections.Generic;
    using NumericalType = System.Double;

    public sealed class BuffManager : XObject
    {
        private const float mPrecision = 0.001f;

        private Unit mOwner = null;
        private LinkedList<Buff> mBuffList = new LinkedList<Buff>();
        private LinkedList<Buff> mAddList = new LinkedList<Buff>();
        private LinkedList<Buff> mHandleList = new LinkedList<Buff>();

        public Unit Owner
        {
            get { return mOwner; }
        }

        protected override void OnDispose()
        {
            using (LinkedList<Buff>.Enumerator itr = mBuffList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mBuffList.Clear();

            mOwner = null;
        }

        public BuffManager(Unit owner)
        {
            mOwner = owner;
        }

        public void Update(float fTick)
        {
            using (LinkedList<Buff>.Enumerator itr = mBuffList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Update(fTick);
                    if (itr.Current.HasFinished())
                    {
                        mHandleList.AddLast(itr.Current);
                    }
                }
            }

            using (LinkedList<Buff>.Enumerator itr = mHandleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                    mBuffList.Remove(itr.Current);
                }
            }
            mHandleList.Clear();
        }

        public void AddBuff(string id)
        {
            BuffFactoryProperty bpFactory = PropertyMgr.Instance.GetBuffProperty(id);
            if (bpFactory == null)
                return;

            BuffProperty bp = bpFactory.BuffProperty;
            using (LinkedList<Buff>.Enumerator itr = mBuffList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (id == itr.Current.ID)
                    {
                        mAddList.AddLast(itr.Current);
                    }
                }
            }

            if (mAddList.Count == bp.AddNum)
            {
                if (bp.AddType == EBuffAddType.EAT_AddNone)
                {
                    return;
                }
                else
                {
                    mAddList.First.Value.Dispose();
                    mAddList.RemoveFirst();
                }
            }
            mAddList.Clear();

            Buff buff = null;
            switch (bpFactory.BuffType)
            {
                case EBuffType.EBT_NumericalBuff:
                    buff = new NBuff();
                    break;
                case EBuffType.EBT_DeltaBuff:
                    buff = new DBuff();
                    break;
                case EBuffType.EBT_ConditionBuff:
                    buff = new CBuff();
                    break;
                case EBuffType.EBT_SpecialBuffHP:
                    buff = new SBuffHP();
                    break;
                case EBuffType.EBT_SpecialBuffDizzy:
                    buff = new SBuffDizzy();
                    break;
                case EBuffType.EBT_SpecialBuffGodMode:
                    buff = new SBuffGodMode();
                    break;
                case EBuffType.EBT_SpecialBuffResetSkillCD:
                    buff = new SBuffResetSkillCD();
                    break;
                case EBuffType.EBT_SpecialBuffSummon:
                    buff = new SBuffSummon();
                    break;
                default:
                    break;
            }

            if (buff.Init(this, bp))
            {
                mBuffList.AddLast(buff);
            }
            else
            {
                buff.Dispose();
            }
        }

        public void DelBuff(string id)
        {
            using (LinkedList<Buff>.Enumerator itr = mBuffList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (id == itr.Current.ID)
                    {
                        itr.Current.Dispose();
                        mBuffList.Remove(itr.Current);

                        break;
                    }
                }
            }
        }

        public NumericalType Apply(EAttributeType attrType, NumericalType val)
        {
            int addVal = 0;
            int mulVal = 0;

            using (LinkedList<Buff>.Enumerator itr = mBuffList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Apply(attrType, ref addVal, ref mulVal);
                }
            }

            val = val + addVal * mPrecision + val * mulVal * mPrecision * 0.01f;

            return val;
        }

    }
}