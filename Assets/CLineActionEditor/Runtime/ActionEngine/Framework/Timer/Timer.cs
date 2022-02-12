/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Timer\Timer.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-1      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using System.Collections.Generic;

    public sealed class Timer : XObject
    {
        public delegate void TimerAction(params object[] param);

        private float mCurTime;
        private float mTotalTime;
        private bool mRepeat;
        private int mRepeatCount; // -1 forever
        private int mCurCount;
        private TimerAction mAction = null;
        private List<object> mParamList = new List<object>();

        public Timer(float duration, bool repeat, int repeatCount, TimerAction action, params object[] param)
        {
            mCurTime = 0f;
            mCurCount = 0;
            mTotalTime = duration;
            mRepeat = repeat;
            mRepeatCount = repeatCount;

            mAction = action;
            mParamList.AddRange(param);
        }

        public bool Update(float fTick)
        {
            if (null == mAction)
                return true;

            mCurTime += fTick;
            if (mCurTime >= mTotalTime)
            {
                mAction(mParamList.ToArray());

                mCurTime -= mTotalTime;
                mCurCount++;

                return mRepeat ? (mRepeatCount < 0 ? false : (mCurCount < mRepeatCount ? false : true)) : true;
            }
            else
            {
                return false;
            }
        }

        public void Stop()
        {
            mAction = null;
        }

        protected override void OnDispose()
        {
            mAction = null;
            mParamList.Clear();
        }

    }
}
