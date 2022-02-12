/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\DBuff.cs
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
    public sealed class DBuff : Buff
    {
        private float mCurTime = 0f;
        private float mInterval = 0f;

        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            DBuffProperty dp = property as DBuffProperty;
            mCurTime = 0f;
            mInterval = dp.Interval * mPrecision;

            return true;
        }

        public override void Update(float fTick)
        {
            base.Update(fTick);

            mCurTime += fTick;
            if (mCurTime >= mInterval)
            {
                mCurTime -= mInterval;

                DBuffProperty dp = mProperty as DBuffProperty;
                mMgr.AddBuff(dp.AddBuffID);
            }
        }
    }
}