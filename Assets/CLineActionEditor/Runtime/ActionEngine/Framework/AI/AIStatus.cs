/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\AIStatus.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-23      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    public sealed class AIStatus
    {
        private Unit mOwner = null;
        private AISwitch mCurAI = null;
        private AISwitch mNextAI = null;

        private SortedList<int, List<AISwitch>> mAIList = new SortedList<int, List<AISwitch>>();
        private List<AISwitch> mRandomList = new List<AISwitch>();

        public Unit Owner
        {
            get { return mOwner; }
        }

        public NavMeshAgent NavAgent
        {
            get;
            set;
        }

        public void StartNavigation(Vector3 pos)
        {
            if (NavAgent != null && mOwner.UObject.gameObject.activeInHierarchy)
            {
                NavAgent.speed = (float)mOwner.GetAttribute(EAttributeType.EAT_CurMoveSpeed);
                NavAgent.destination = pos;
            }
            else
            {
                mOwner.SetPosition(pos);
            }
        }

        public void StopNavigation()
        {
            if (NavAgent.enabled)
            {
                NavAgent.isStopped = true;
                NavAgent.ResetPath();
            }
        }

        public void Init(Unit owner, string startupAI, List<AISwitch> aiList)
        {
            mOwner = owner;

            // agent
            NavAgent = owner.UObject.GetComponent<NavMeshAgent>();
            NavAgent.enabled = false;

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
            if (mOwner.ActionStatus.ActiveAction.ActionStatus == EActionState.Move)
            {
                NavAgent.enabled = true;
            }
            else
            {
                NavAgent.enabled = false;
            }

            if (action.ActionStatus != EActionState.BeHit)
            {
                mCurAI = mNextAI;
                mNextAI = null;
                mCurAI.OnAIStart(this);
            }
        }

        public void OnActionChanging(Action oldAction, Action newAction, bool interrupt)
        {
            if (newAction.ActionStatus == EActionState.BeHit)
            {
                return;
            }
            if (mCurAI != null)
            {
                mCurAI.OnAIChanging(this, mNextAI);
            }
        }

        public void OnActionEnd(Action action, bool interrupt)
        {
            if (action.ActionStatus == EActionState.BeHit)
            {
                return;
            }
            if (mCurAI != null)
            {
                mCurAI.OnAIEnd(this);
                mCurAI = null;
            }
        }

        public void OnMessage(Message msg)
        {
            using (var itrList = mAIList.GetEnumerator())
            {
                while (itrList.MoveNext())
                {
                    using (var itr = itrList.Current.Value.GetEnumerator())
                    {
                        while (itr.MoveNext())
                        {
                            itr.Current.OnMessage(this, msg);
                        }
                    }
                }
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
                            itr.Current.Update(this, fTick);
                        }
                    }
                }
            }
        }

        private void ProcessAI(float fTick)
        {
            AISwitch nextAI = GetPriorityAI(fTick);

            if (mNextAI != null && !mNextAI.CheckAI(this, fTick))
            {
                mNextAI = null;
            }

            if (mNextAI == null || nextAI.Order > mNextAI.Order)
            {
                mNextAI = nextAI;
            }

            if (mNextAI != null)
            {
                Helper.SetAny<string>(mOwner.PropertyContext.GetProperty(PropertyName.sAI), mNextAI.ActionID);
            }
        }

        private AISwitch GetPriorityAI(float fTick)
        {
            AISwitch nextAI = null;
            using (var itr = mAIList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    nextAI = ProcessInternalAI(itr.Current.Value, fTick);
                    if (nextAI != null)
                    {
                        break;
                    }
                }
            }

            return nextAI;
        }

        private AISwitch ProcessInternalAI(List<AISwitch> list, float fTick)
        {
            AISwitch nextAI = null;
            if (list.Count == 1)
            {
                if (list[0].CheckAI(this, fTick))
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
                        if (itr.Current.CheckAI(this, fTick))
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