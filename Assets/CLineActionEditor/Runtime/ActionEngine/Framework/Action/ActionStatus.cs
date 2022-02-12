/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionStatus.cs
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
    using System.Text;
    using UnityEngine;
    using System.Collections.Generic;


    public sealed class ActionStatus
    {
        // from action
        private bool mRestrictFrozen = false;
        private bool mCanMove;
        private bool mCanRotate;
        private bool mIgnoreGravity;
        private bool mCanHurt;
        private bool mCanHit;   //enable play hit action
        private bool mCanSkill; // enable play skill
        private bool mIsGod;
        private bool mFaceTarget;
        //private int mActionLevel; 
        private bool mIgnoreMove; // set by local unit or remote unit
        private float mActionScale = 1f; // time scale by event
        private int mActionInterruptEnabled;

        // runtime
        private bool mEnableBreak;
        private bool mCurActionFinished;
        private int mCurActionTime;
        private int mCurEventIndex;
        private int mCurAttackHitIndex;
        private float mTotalTime;

        private Unit mOwner;
        private Action mActiveAction;

        private Vector3 mMoveRelDistance = Vector3.zero;
        private Vector3 mMoveRelAcc = Vector3.zero;
        private Vector3 mVelocity = Vector3.zero;
        private Vector3 mAccelerate = Vector3.zero;
        private Vector3 mFaceDirection = Vector3.forward;

        private string mActionGroup;

        //timescale
        private float mTimeScale = 1f;

        private float mAnimCrossFadeDuration = 0;
        private List<ActionEvent> mEventList = new List<ActionEvent>();
        private List<ActionEvent> mHandleList = new List<ActionEvent>();

        #region property
        public Action ActiveAction
        {
            get { return mActiveAction; }
        }
        public bool CanMove
        {
            get
            {
                return mCanMove;
            }
            set
            {
                mCanMove = value;
            }
        }
        public bool CanRotate
        {
            get
            {
                return mCanRotate;
            }
            set
            {
                mCanRotate = value;
            }
        }
        public bool IgnoreGravity
        {
            get { return mIgnoreGravity; }
            set { mIgnoreGravity = value; }
        }
        public bool CanHurt
        {
            get { return mCanHurt; }
            set { mCanHurt = value; }
        }
        public bool CanHit
        {
            get { return mCanHit; }
            set { mCanHit = value; }
        }
        public bool CanSkill
        {
            get { return mCanSkill; }
            set { mCanSkill = value; }
        }
        public bool IsGod
        {
            get { return mIsGod; }
            set { mIsGod = value; }
        }
        public bool FaceTarget
        {
            get { return mFaceTarget; }
            set { mFaceTarget = value; }
        }
        public bool EnableBreak
        {
            get { return mEnableBreak; }
            set { mEnableBreak = value; }
        }
        public float TotalTime
        {
            get { return mTotalTime; }
            set { mTotalTime = value; }
        }
        public bool IgnoreMove
        {
            get { return mIgnoreMove; }
            set { mIgnoreMove = value; }
        }
        public string ActionGroup
        {
            get { return mActionGroup; }
            set { mActionGroup = value; }
        }
        public float TimeScale
        {
            set { mTimeScale = value; }
        }
        #endregion

        public ActionStatus(Unit owner)
        {
            mOwner = owner;
            mActiveAction = null;
            mIsGod = false;
        }

        public void Update(float fTick)
        {
            if (mActiveAction == null) return;

            fTick *= mActionScale;

            int t1 = (int)(mTotalTime * 1000f);
            mTotalTime += fTick * mTimeScale;
            int t2 = (int)(mTotalTime * 1000f);


            if (mRestrictFrozen)
                return;

            TickAction(t2 - t1);
        }

        private void TickAction(int deltaTime)
        {
            //ProcessAnimSpeed(ref deltaTime);

            int nextActionTime = 0;
            if (mCurActionTime + deltaTime >= mActiveAction.TotalTime)
            {
                int delta = deltaTime;
                deltaTime = mActiveAction.TotalTime - mCurActionTime;
                nextActionTime = delta - deltaTime;

                mCurActionFinished = true;
            }

            mCurActionTime += deltaTime;

            ProcessOrientation();
            ProcessEventList(deltaTime);
            ProcessAttackDefList(deltaTime);

            if (ProcessActionInterruptList(deltaTime)) return;

            ProcessMoving(deltaTime);

            if (mCurActionFinished)
            {
                ProcessActionFinish(nextActionTime);
            }
        }

        void ProcessAnimSpeed(ref int deltaTime)
        {
            if (mOwner.UUnit != null)
            {
                if (mActiveAction.ActionStatus == EActionState.Attack && mOwner.GetAttribute(EAttributeType.EAT_AttackSpeed) > 0)
                {
                    float speed = Mathf.Clamp((float)mOwner.GetAttribute(EAttributeType.EAT_AttackSpeed) * 0.001f, 0f, float.MaxValue);
                    deltaTime = (int)(deltaTime * speed);
                    mOwner.UUnit.SetAnimationSpeed(speed);
                }
                else if (mActiveAction.ActionStatus == EActionState.Move && mOwner.GetAttribute(EAttributeType.EAT_CurMoveSpeed) != (mOwner.GetAttribute(EAttributeType.EAT_MoveSpeed) * 0.001f))
                {
                    float speed = (float)(mOwner.GetAttribute(EAttributeType.EAT_CurMoveSpeed) / (mOwner.GetAttribute(EAttributeType.EAT_MoveSpeed) * 0.001f));
                    speed = Mathf.Clamp(speed, 0.3f, float.MaxValue);
                    deltaTime = (int)(deltaTime * speed);
                    mOwner.UUnit.SetAnimationSpeed(speed);
                }
                else
                {
                    mOwner.UUnit.SetAnimationSpeed(1f);
                }
            }
        }

        private void ProcessOrientation()
        {
            if (mActiveAction.FaceTarget)
            {
                if (mOwner.Target != null)
                {
                    mOwner.SetOrientation(mOwner.Target.Position - mOwner.Position, mOwner.UnitType == EUnitType.EUT_Monster ? false : true);
                }
                else
                {
                    mFaceDirection.x = mOwner.UObject.transform.forward.x;
                    mFaceDirection.y = 0f;
                    mFaceDirection.z = mOwner.UObject.transform.forward.z;
                    mOwner.SetOrientation(mFaceDirection, mOwner.UnitType == EUnitType.EUT_Monster ? false : true);
                }
            }
            else if (!mVelocity.Equals(Vector3.zero))
            {
                mFaceDirection.x = mVelocity.x;
                mFaceDirection.y = 0f;
                mFaceDirection.z = mVelocity.z;
                if (!mFaceDirection.Equals(Vector3.zero))
                {
                    mOwner.SetOrientation(mFaceDirection, mOwner.UnitType == EUnitType.EUT_Monster ? false : true);
                }
            }

        }

        private void Reset()
        {
            mEnableBreak = false;
            mCurActionFinished = false;
            mCurActionTime = 0;
            mCurEventIndex = 0;
            mCurAttackHitIndex = 0;
            mActionScale = 1f;
            mActionInterruptEnabled = 0;
            mIgnoreMove = false;

            mIgnoreGravity = mActiveAction.IgnoreGravity;
            mCanHurt = mActiveAction.CanHurt;
            mCanHit = mActiveAction.CanHit;
            mIsGod = mActiveAction.IsGod;
            mFaceTarget = mActiveAction.FaceTarget;
            mCanMove = mRestrictFrozen ? false : mActiveAction.CanMove;
            mCanRotate = mRestrictFrozen ? false : mActiveAction.CanRotate;

            mMoveRelDistance = Vector3.zero;

            for (int index = 0; index < this.mActiveAction.EventList.Count; ++index)
            {
                this.mActiveAction.EventList[index].Reset();
            }
            for (int index = 0; index < this.mActiveAction.InterruptList.Count; ++index)
            {
                var interrupt = this.mActiveAction.InterruptList[index].EventData as ActionInterrupt;
                if (interrupt.Enabled)
                    this.mActionInterruptEnabled |= 1 << index;
            }

            mOwner.SetAnimatorLayerWeight(1, 0);
            mAnimCrossFadeDuration = 0;

            mEventList.Clear();
            mHandleList.Clear();
        }

        private void SwitchStatus(string name, bool on)
        {
            switch (name)
            {
                case "IgnoreGravity":
                    mIgnoreGravity = on;
                    break;
                case "CanMove":
                    mCanMove = on;
                    break;
                case "CanRotate":
                    mCanRotate = on;
                    break;
                case "CanHurt":
                    mCanHurt = on;
                    break;
                case "FaceTarget":
                    mFaceTarget = on;
                    break;
            }
        }

        private bool OnTriggerEvent(ActionEvent evt, int deltaTime)
        {
            //TO CLine: add event condition at later.
            return true;
        }

        private void TriggerEvent(ActionEvent evt, int deltaTime)
        {
            if (evt.EventData is EventPlayAnim epa)
            {
                epa.AnimCrossFadeDuration = mAnimCrossFadeDuration;
            }

            switch (evt.TriggerType)
            {
                case ETriggerType.Signal:
                    evt.EventData.Execute(mOwner);
                    break;
                case ETriggerType.Duration:
                    {
                        evt.EventData.Enter(mOwner);
                        mEventList.Add(evt);
                    }
                    break;
            }
        }

        private void ProcessEventList(int deltaTime)
        {
            // process event only one time
            for (; mCurEventIndex < mActiveAction.EventList.Count; ++mCurEventIndex)
            {
                ActionEvent evt = mActiveAction.EventList[mCurEventIndex];
                if (evt.TriggerTime < mCurActionTime)
                {
                    if (OnTriggerEvent(evt, deltaTime))
                    {
                        TriggerEvent(evt, deltaTime);
                    }
                }
                else
                {
                    break;
                }
            }

            // process event every time
            mHandleList.Clear();
            using (var itr = mEventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.CheckTime(deltaTime))
                    {
                        itr.Current.EventData.Update(mOwner, deltaTime);
                    }
                    else
                    {
                        mHandleList.Add(itr.Current);
                    }
                }
            }
            using (var itr = mHandleList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.EventData.Exit(mOwner);
                    mEventList.Remove(itr.Current);
                }
            }
        }

        private void ProcessEventWhenActionEnd()
        {
            using (var itr = mEventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.EventData.Exit(mOwner);
                }
            }
            mEventList.Clear();
        } 

        private void ProcessAttackDefList(int deltaTime)
        {
            for (; mCurAttackHitIndex < mActiveAction.AttackList.Count; ++mCurAttackHitIndex)
            {
                var evt = mActiveAction.AttackList[mCurAttackHitIndex];
                var atk = evt.EventData as ActionAttackDef;
                if (evt.TriggerTime < mCurActionTime)
                {
                    AttackHitMgr.Instance.Create(mOwner, atk, ActiveAction.ID);
                }
                else
                {
                    break;
                }
            }
        }

        public bool CheckActionInterrupt(ActionInterrupt interrupt)
        {
            if (!interrupt.CheckAllCondition)
            {
                for (int i = 0; i < interrupt.ConditionList.Count; ++i)
                {
                    if (interrupt.ConditionList[i].CheckInterrupt(mOwner))
                        return true;
                }
                return false;
            }
            else
            {
                for (int i = 0; i < interrupt.ConditionList.Count; ++i)
                {
                    if (!interrupt.ConditionList[i].CheckInterrupt(mOwner))
                        return false;
                }
                return true;
            }
        }

        private void EnableActionInterrupt(int statusIdx, bool enabled)
        {
            if (enabled)
            {
                mActionInterruptEnabled |= 1 << statusIdx;
            }
            else
            {
                mActionInterruptEnabled &= ~(1 << statusIdx);
            }
        }

        public bool GetInterruptEnabled(int idx)
        {
            return (mActionInterruptEnabled & (1 << idx)) != 0;
        }

        private bool ProcessActionInterrupt(ActionInterrupt interrupt)
        {
            if (!CheckActionInterrupt(interrupt))
            {
                return false;
            }
            else
            {
                ProcessEventWhenActionEnd();
                ChangeAction(interrupt.ActionID, 0, true, interrupt.CrossFadeDuration);

                return true;
            }
        }

        private bool ProcessActionInterruptList(int deltaTime)
        {
            int idx = 0;
            using (var itr = mActiveAction.InterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (mCurActionTime >= itr.Current.TriggerTime)
                    {
                        EnableActionInterrupt(idx, true);
                    }
                    else
                    {
                        EnableActionInterrupt(idx, false);
                    }

                    if (GetInterruptEnabled(idx++) && ProcessActionInterrupt(itr.Current.EventData as ActionInterrupt))
                        return true;
                }
            }

            return false;
        }

        private void ProcessMoving(int deltaTime)
        {
            if (!mIgnoreMove)
            {
                float dt = deltaTime * 0.001f;
                ProcessActionMove(ref mVelocity, dt);

                float x = mMoveRelDistance.x;
                float z = mMoveRelDistance.z;
                if (x != 0f || z != 0f)
                    Helper.Rotate(ref x, ref z, 0, true);

                mOwner.Move(new Vector3(x, mMoveRelDistance.y, z));

                mMoveRelDistance = Vector3.zero;
            }

            Helper.SetAny<float>(mOwner.PropertyContext.GetProperty(PropertyName.sVelocityY), mVelocity.y);
        }

        private void ProcessActionMove(ref Vector3 velocity, float dt)
        {
            if (mIgnoreGravity)
            {
                mMoveRelAcc = mAccelerate;
            }
            else
            {
                mMoveRelAcc = mAccelerate + Physics.gravity;
            }

            mMoveRelDistance.x += velocity.x * dt + 0.5f * mMoveRelAcc.x * dt * dt;
            mMoveRelDistance.z += velocity.z * dt + 0.5f * mMoveRelAcc.z * dt * dt;
            mMoveRelDistance.y += velocity.y * dt + 0.5f * mMoveRelAcc.y * dt * dt;

            velocity.x = velocity.x + mMoveRelAcc.x * dt;
            velocity.z = velocity.z + mMoveRelAcc.z * dt;
            velocity.y = velocity.y + mMoveRelAcc.y * dt;
            velocity.y = Mathf.Clamp(velocity.y, -256f, 256f);
        }

        private void ProcessActionFinish(int deltaTime)
        {
            ProcessEventWhenActionEnd();
            OnActionFinish(mActiveAction);

            string action = mActiveAction.DefaultAction;
            if (!string.IsNullOrEmpty(action))
                ChangeAction(action, 0, false);
        }

        public void SetVelocity(Vector3 velocity)
        {
            mVelocity = velocity;
        }

        public void ApplyVelocity(Vector3 velocity)
        {
            mVelocity += velocity;
        }

        public void SetAccelerate(Vector3 acc)
        {
            mAccelerate = acc;
        }

        public void ApplyAccelerate(Vector3 acc)
        {
            mAccelerate += acc;
        }

        public bool HasAction(string id)
        {
            return PropertyMgr.Instance.HasAction(mActionGroup, id);
        }

        private void OnActionStart(Action action)
        {
            mOwner.OnCommand(ECommandType.ECT_ActionStart, action);
        }

        private void OnChangingAction(Action oldAction, Action newAction, bool interrupt)
        {
            mOwner.OnCommand(ECommandType.ECT_ActionChanging, oldAction, newAction, interrupt);
        }

        private void OnActionEnd(Action action, bool interrupt)
        {
            mOwner.OnCommand(ECommandType.ECT_ActionEnd, action, interrupt);
        }

        private void OnActionFinish(Action action)
        {
            mOwner.OnCommand(ECommandType.ECT_ActionFinish, action);
        }

        public void ChangeAction(string id, int deltaTime = 0, bool interrupt = false, float animCrossFadeDuration = 0)
        {
            Action action = PropertyMgr.Instance.GetAction(mActionGroup, id);

            if (action == null)
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Action", "the action of \"{0}\" is not exist!!!", id);
                return;
            }

            if (mActiveAction != null)
            {
                OnChangingAction(mActiveAction, action, interrupt);
                OnActionEnd(mActiveAction, interrupt);
            }

            mActiveAction = action;

            OnActionStart(mActiveAction);

            Reset();
            if (interrupt)
                mAnimCrossFadeDuration = animCrossFadeDuration;

            if (deltaTime > 0)
                TickAction(deltaTime);
        }

        // Straight, Frozen
        public bool RestrictFrozen
        {
            set
            {
                mCanMove = value ? false : mActiveAction.CanMove;
                mCanRotate = value ? false : mActiveAction.CanRotate;
                mRestrictFrozen = value;
            }
        }

        public bool RestrictMove
        {
            set
            {
                mCanMove = value ? false : mActiveAction.CanMove;
            }
        }

        public bool RestrictHit
        {
            set
            {
                mCanHit = value ? false : mActiveAction.CanHit;
            }
        }

        public bool RestricSkill
        {
            set
            {
                mCanSkill = value;
            }
        }
    }

}
