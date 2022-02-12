/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\NBuff.cs
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
    using UnityEngine;

    public sealed class NBuff : Buff
    {
        private string mLoopEffect = string.Empty;

        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            NBuffProperty p = mProperty as NBuffProperty;

            if (!string.IsNullOrEmpty(p.BuffDummy))
            {
                Transform dummy = Helper.Find(mMgr.Owner.UObject, p.BuffDummy);
                Debug.Assert(dummy != null, "dummy is not exist.");
                
                if (!string.IsNullOrEmpty(p.StartEffect))
                {
                    EffectMgr.Instance.PlayEffect(p.StartEffect, dummy.gameObject, Vector3.zero, Vector3.zero);
                }

                if (!string.IsNullOrEmpty(p.KeepEffect))
                {
                    mLoopEffect = Helper.NonceStr();
                    EffectMgr.Instance.PlayEffect(p.KeepEffect, dummy.gameObject, Vector3.zero, Vector3.zero, true, mLoopEffect);
                }
            }

            return true;
        }

        public override void Apply(EAttributeType attrType, ref int addVal, ref int mulVal)
        {
            NBuffProperty p = mProperty as NBuffProperty;
            if (p.Attr == attrType)
            {
                addVal += p.AddValue;
                mulVal += p.MulValue;
            }
        }

        protected override void OnDispose()
        {
            if (!string.IsNullOrEmpty(mLoopEffect))
            {
                EffectMgr.Instance.StopEffect(mLoopEffect);
            }

            base.OnDispose();
        }
    }
}