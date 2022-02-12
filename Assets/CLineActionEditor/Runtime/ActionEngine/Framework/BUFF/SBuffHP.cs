/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\BUFF\SBuffHP.cs
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
    using NumericalType = System.Double;

    public sealed class SBuffHP : Buff
    {
        public override bool Init(BuffManager mgr, BuffProperty property)
        {
            base.Init(mgr, property);

            SBuffHPProperty p = mProperty as SBuffHPProperty;

            if (!string.IsNullOrEmpty(p.BuffDummy))
            {
                Transform dummy = Helper.Find(mMgr.Owner.UObject, p.BuffDummy);
                Debug.Assert(dummy != null, "dummy is not exist.");

                if (!string.IsNullOrEmpty(p.StartEffect))
                {
                    EffectMgr.Instance.PlayEffect(p.StartEffect, dummy.gameObject, Vector3.zero, Vector3.zero);
                }
            }

            NumericalType hp = mMgr.Owner.GetAttribute(p.Attr);
            hp = hp * (p.MulValue * mPrecision) + (p.AddValue * mPrecision);
            mMgr.Owner.AddHP(hp);

            return false;
        }

    }
}