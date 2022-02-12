/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Timer\Ticker.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-2      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

using System;

namespace SuperCLine.ActionEngine
{
    public sealed class Ticker : XObject
    {
        public delegate void TickerUpdateAction(float time, float duration);
        public delegate void TickerEndAction();

        private float mCurTime = 0f;
        private float mTotalTime = 0f;
        private TickerUpdateAction mUpdateAction = null;
        private TickerEndAction mEndAction = null;

        public Ticker(float duration, TickerUpdateAction updateHandler, TickerEndAction endHandler)
        {
            mCurTime = 0f;
            mTotalTime = duration;
            mUpdateAction = updateHandler;
            mEndAction = endHandler;
        }

        public bool Update(float fTick)
        {
            mCurTime += fTick;
            if (null != mUpdateAction)
                mUpdateAction(mCurTime, mTotalTime);

            if (mCurTime >= mTotalTime)
            {
                if (null != mEndAction)
                    mEndAction();

                mCurTime -= mTotalTime;

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Stop()
        {
            mTotalTime = 0f;
        }

        protected override void OnDispose()
        {
            mUpdateAction = null;
            mEndAction = null;
        }
    }
}
