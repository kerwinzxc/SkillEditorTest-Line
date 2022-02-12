/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\Buff.cs
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
    public class Buff : XObject
    {
        private float mLifeTime = 0f;
        private float mTotalTime = 0f;

        protected BuffManager mMgr = null;
        protected BuffProperty mProperty = null;

        protected const float mPrecision = 0.001f;

        public string ID
        {
            get { return mProperty.ID; }
        }

        public BuffManager Mgr
        {
            get { return mMgr; }
        }

        public BuffProperty Property
        {
            get { return mProperty; }
        }

        public virtual bool Init(BuffManager mgr, BuffProperty property)
        {
            mMgr = mgr;
            mProperty = property;
            mLifeTime = 0f;
            mTotalTime = property.Duration * mPrecision;

            return true;
        }

        public virtual void Update(float fTick)
        {
            mLifeTime += fTick;
        }

        public virtual bool HasFinished()
        {
            return mProperty.Duration > 0 ? mLifeTime >= mTotalTime : false;
        }

        public virtual void Apply(EAttributeType attrType, ref int addVal, ref int mulVal)
        {

        }

        protected override void OnDispose()
        {
            mMgr = null;
            mProperty = null;
        }
    }

}