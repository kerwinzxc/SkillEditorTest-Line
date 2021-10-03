/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\AIStatus.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-23      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class AIStatus
    {
        private Unit mOwner = null;
        private AISwitch mCurAI = null;
        private AISwitch mNextAI = null;

        private SortedList<int, List<AISwitch>> mAIList = new SortedList<int, List<AISwitch>>();
        private List<AISwitch> mRandomList = new List<AISwitch>();

        public void Init(Unit owner, string startupAI, List<AISwitch> aiList)
        {
            mOwner = owner;

            using (var itr = aiList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    List<AISwitch> l = null;
                    int order = itr.Current.Order;
                    if (!mAIList.TryGetValue(order, out l))
                    {
                        l = new List<AISwitch>();
                        mAIList.Add(order, l);
                    }

                    l.Add(itr.Current);

                    if (mNextAI == null && itr.Current.ID == startupAI)
                    {
                        mNextAI = itr.Current;
                    }
                }
            }

            Debug.Assert(mNextAI != null, string.Format("'{0}' is not exist.", startupAI));
            mOwner.ActionStatus.ChangeAction(mNextAI.ActionID);
        }

        public void Update(float fTick)
        {
            ProcessUpdate(fTick);
            ProcessAI(fTick);
        }

        public void OnActionFinish(Action action)
        {
            
        }

        public void OnActionStart(Action action)
        {
            if (action.ActionStatus != EActionState.BeHit)
            {
                mCurAI = mNextAI;
                mNextAI = null;

                mCurAI.OnAIStart(mOwner);
            }
        }

        public void OnActionChanging(Action oldAction, Action newAction, bool interrupt)
        {

        }

        public void OnActionEnd(Action action, bool interrupt)
        {
            if (mCurAI != null)
            {
                mCurAI.OnAIEnd(mOwner);
                mCurAI = null;
            }
        }

        private void ProcessUpdate(float fTick)
        {
            using (var itrList = mAIList.GetEnumerator())
            {
                while (itrList.MoveNext())
                {
                    using (var itr = itrList.Current.Value.GetEnumerator())
                    {
                        while (itr.MoveNext())
                        {
                            itr.Current.Update(mOwner, fTick);
                        }
                    }
                }
            }
        }

        private void ProcessAI(float fTick)
        {
            if (mNextAI != null && mNextAI.CheckAI(mOwner, fTick))
            {
                return;
            }

            mNextAI = null;
            using (var itr = mAIList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mNextAI = ProcessInternalAI(itr.Current.Value, fTick);
                    if (mNextAI != null)
                    {
                        mOwner.CustomPropertyHash[CustomProperty.sAI].Value = mNextAI.ActionID;
                        break;
                    }
                }
            }
        }

        private AISwitch ProcessInternalAI(List<AISwitch> list, float fTick)
        {
            AISwitch nextAI = null;
            if (list.Count == 1)
            {
                if (list[0].CheckAI(mOwner, fTick))
                    nextAI = list[0];
            }
            else
            {
                mRandomList.Clear();

                int max = 0;
                using (var itr = list.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        if (itr.Current.CheckAI(mOwner, fTick))
                        {
                            max += itr.Current.RandomWeight;
                            mRandomList.Add(itr.Current);
                        }
                    }
                }

                if (max > 0)
                {
                    int rand = Random.Range(0, max);
                    int weight = 0;
                    using (var itr = mRandomList.GetEnumerator())
                    {
                        while (itr.MoveNext())
                        {
                            weight += itr.Current.RandomWeight;
                            if (rand < weight)
                            {
                                nextAI = itr.Current;
                                break;
                            }
                        }
                    }
                }
            }
            
            return nextAI;
        }

    }
}