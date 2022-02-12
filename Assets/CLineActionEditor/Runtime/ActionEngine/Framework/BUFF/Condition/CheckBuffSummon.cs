/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\Condition\CheckBuffSummon.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-11      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckBuffSummon : BuffCondition, IProperty
    {
        private CBuff mOwner = null;
        private bool mSummon = false;

        public EBuffConditionType BuffCondType
        {
            get { return EBuffConditionType.ECT_CheckBuffSummon; }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void Init(CBuff owner)
        {
            mOwner = owner;
            mSummon = false;

            MessageMgr.Instance.Register("EVT_SUMMON", OnEvent);
        }

        public void Destroy()
        {
            MessageMgr.Instance.Unregister("EVT_SUMMON", OnEvent);

            mOwner = null;
        }

        public bool CheckBuff(Unit unit)
        {
            return mSummon;
        }

        public void Reset()
        {
            mSummon = false;
        }

        private void OnEvent(Message msg)
        {
            Unit obj = (Unit)msg.GetArg("Unit");
            if (obj == mOwner.Mgr.Owner)
            {
                mSummon = true;

                mOwner.OnEvent();
            }
        }

        public void Deserialize(JsonData jd)
        {

        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return writer;
        }

        public BuffCondition Clone()
        {
            CheckBuffSummon cb = new CheckBuffSummon();
            return cb;
        }
    }
}