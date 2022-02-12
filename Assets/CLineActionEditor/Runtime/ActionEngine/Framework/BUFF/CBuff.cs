/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\CBuff.cs
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

    public sealed class CBuff : Buff
    {
        private List<BuffCondition> mConditionList = new List<BuffCondition>();

        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            CBuffProperty cp = property as CBuffProperty;
            using (List<BuffCondition>.Enumerator itr = cp.ConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    BuffCondition bc = itr.Current.Clone();
                    bc.Init(this);

                    mConditionList.Add(bc);
                }
            }

            return true;
        }

        public void OnEvent()
        {
            bool check = true;
            using (List<BuffCondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    check = itr.Current.CheckBuff(mMgr.Owner);
                    if (!check)
                        break;
                }
            }

            if (check)
            {
                CBuffProperty cp = mProperty as CBuffProperty;
                if (cp.AddBuffIDList.Count > 0)
                {
                    mMgr.Owner.AddBuff(cp.AddBuffIDList);
                }
                if (cp.DelBuffIDList.Count > 0)
                {
                    mMgr.Owner.DelBuff(cp.DelBuffIDList);
                }

                using (List<BuffCondition>.Enumerator itr = mConditionList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        itr.Current.Reset();
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            using (List<BuffCondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Destroy();
                }
            }
            mConditionList.Clear();

            base.OnDispose();
        }

    }
}