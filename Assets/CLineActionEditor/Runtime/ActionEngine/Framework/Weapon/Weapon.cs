/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Weapon\Weapon.cs
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
    using System;
    using UnityEngine;
    using System.Collections.Generic;
#if USE_PLUGINWEAPONTRAIL
    using PluginWeaponTrail;
#endif

    public class Weapon : XObject
    {
        protected Unit mOwner;
        protected WeaponProperty mProperty;
        protected GameObject mUObject;
        protected Transform mDummyIdle;
        protected Transform mDummyAttackL;
        protected Transform mDummyAttackR;
#if USE_PLUGINWEAPONTRAIL
        protected XWeaponTrail mXWeaponTrail;
#endif
        private List<Material> mSkinnedMeshMaterialList = new List<Material>();

    #region
        public Unit Owner
        {
            get { return mOwner; }
        }
        public WeaponProperty Property
        {
            get { return mProperty; }
        }
        public Transform UObject
        {
            get { return mUObject.transform; }
        }
        public Transform DummyIdle
        {
            get { return mDummyIdle; }
        }
        public Transform DummyAttackL
        {
            get { return mDummyAttackL; }
        }
        public Transform DummyAttackR
        {
            get { return mDummyAttackR; }
        }
        #if USE_PLUGINWEAPONTRAIL
        public XWeaponTrail XWeaponTrail
        {
            get { return mXWeaponTrail; }
        }
        #endif

        #endregion

        public Weapon(Unit owner, WeaponProperty property)
        {
            mOwner = owner;
            mProperty = property;
            mUObject = ObjectPoolMgr.Instance.GetPool(mProperty.Prefab).Get();
            mDummyIdle = Helper.Find(mOwner.UObject.transform, mProperty.AttachDummyIdle);
            mDummyAttackL = Helper.Find(mOwner.UObject.transform, mProperty.AttachDummyAttackL);
            mDummyAttackR = Helper.Find(mOwner.UObject.transform, mProperty.AttachDummyAttackR);

            //Helper.AddChild(mUObject, mDummyIdle.gameObject);

#if USE_PLUGINWEAPONTRAIL
            Transform trail = Helper.Find(mUObject.transform, "WeaponTrail", true);
            if (trail)
            {
                mXWeaponTrail = trail.GetComponent<XWeaponTrail>();
                mXWeaponTrail.Deactivate();
            }
#endif
        }

        protected override void OnDispose()
        {
            if (mUObject != null)
            {
                ObjectPoolMgr.Instance.GetPool(mProperty.Prefab).Cycle(mUObject);
                mUObject = null;
            }

            mProperty = null;
            mOwner = null;
        }

        public virtual void Bind()
        {
            mUObject.SetActive(true);
        }

        public virtual void UnBind()
        {
            mUObject.SetActive(false);
        }

        public virtual void DoAttack(bool left)
        {
            if (left)
            {
                if (mUObject.transform.parent != mDummyAttackL)
                    Helper.AddChild(mUObject, mDummyAttackL.gameObject);
            }
            else
            {
                if (mUObject.transform.parent != mDummyAttackR)
                    Helper.AddChild(mUObject, mDummyAttackR.gameObject);
            }
        }

        public virtual void DoIdle()
        {
            if (mUObject.transform.parent != mDummyIdle)
                Helper.AddChild(mUObject, mDummyIdle.gameObject);
        }

        public void ShowTrail()
        {
#if USE_PLUGINWEAPONTRAIL
            if (mXWeaponTrail && !mXWeaponTrail.isActiveAndEnabled)
                mXWeaponTrail.Activate();
#endif
        }

        public void HideTrail(float fadeTime = 0.3f)
        {
#if USE_PLUGINWEAPONTRAIL
            if (mXWeaponTrail)
                mXWeaponTrail.Deactivate();
                //mXWeaponTrail.StopSmoothly(fadeTime);
#endif
        }

    }

}
