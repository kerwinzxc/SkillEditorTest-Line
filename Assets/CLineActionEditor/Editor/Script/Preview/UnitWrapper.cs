/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Preview\UnitWrapper.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-31      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UnitWrapper : Singleton<UnitWrapper>
    {
        private bool mIsReady = false;
        private GameObject mUnit = null;
        private GameObject mWeapon = null;
        private WeaponProperty mWeaponProperty = null;
        private string mActionGroupName = string.Empty;

        private IUnitAnimator mAnimator;

        public bool IsReady
        {
            get { return mIsReady; }
        }

        public string ActionGroupName
        {
            get { return mActionGroupName; }
        }

        public List<string> StateNameList
        {
            get { return mAnimator.StateNameList; }
        }

        public List<string> ParameterList
        {
            get { return mAnimator.ParameterList; }
        }

        public Dictionary<string, UnitAnimatorData> StateHash
        {
            get { return mAnimator.StateHash; }
        }

        public GameObject UnitWrapperUnit
        {
            get { return mUnit; }
            set { mUnit = value; }
        }

        public IUnitAnimator Anim
        {
            get { return mAnimator; }
            set { mAnimator = value; }
        }

        public GameObject EquipWeapon
        {
            get { return mWeapon; }
            set { mWeapon = value; }
        }

        public bool BuildUnit(IProperty property)
        {
            string prefabname = string.Empty;
            string animatorTypeName = string.Empty;
            if (mUnit != null)
            {
                GameObject.DestroyImmediate(mUnit);
            }

            if (property is PlayerProperty)
            {
                PlayerProperty p = property as PlayerProperty;
                prefabname = p.Prefab;
                animatorTypeName = p.AnimatorTypeName;
                mActionGroupName = p.ActionGroup;
            }
            else if (property is MonsterProperty)
            {
                MonsterProperty m = property as MonsterProperty;
                prefabname = m.Prefab;
                animatorTypeName = m.AnimatorTypeName;
                mActionGroupName = m.ActionGroup;
            }

            if (string.IsNullOrEmpty(prefabname) || string.IsNullOrEmpty(animatorTypeName) || string.IsNullOrEmpty(mActionGroupName))
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ActionEditor", "please set prefab or action group name!");
                return false;
            }
            GameObject go = ResourceMgr.Instance.LoadObject(prefabname, typeof(GameObject)) as GameObject;
            if (go == null)
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ActionEditor", "prefab is not exist!");
                return false;
            }

            mUnit = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;
            System.Type type = Utility.Assembly.GetType(animatorTypeName);
            var ins = System.Activator.CreateInstance(type);
            mAnimator = (IUnitAnimator) ins;
            if (mAnimator == null)
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ActionEditor", "IUnitAnimator is null.");
                return false;
            }
            mAnimator.Init(mUnit);

            CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_SmoothFollow, mUnit.transform, 0.1f, 0.6f, new Vector3(0, 3.5f, -6.5f), new Vector3(20, 0, 0));

            GetAllState();
            GetAllParameter();

            mIsReady = true;

            return true;
        }

        public void BuildWeapon(WeaponProperty wp)
        {
            mWeaponProperty = wp;

            GameObject go = ResourceMgr.Instance.LoadObject(wp.Prefab, typeof(GameObject)) as GameObject;
            if (go == null) return;

            mWeapon = GameObject.Instantiate(go) as GameObject;
            Transform hand = Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, wp.AttachDummyIdle);
            Helper.AddChild(mWeapon, hand.gameObject);
        }

        public void DoWeaponAttack(bool left)
        {
            if (left)
            {
                Transform hand = Helper.Find(mUnit.transform, mWeaponProperty.AttachDummyAttackL);
                Helper.AddChild(mWeapon, hand.gameObject);
            }
            else
            {
                Transform hand = Helper.Find(mUnit.transform, mWeaponProperty.AttachDummyAttackR);
                Helper.AddChild(mWeapon, hand.gameObject);
            }
        }

        public void DoWeaponIdle()
        {
            Transform hand = Helper.Find(mUnit.transform, mWeaponProperty.AttachDummyIdle);
            Helper.AddChild(mWeapon, hand.gameObject);
        }

        public void Tick(float fTick)
        {
            if (mAnimator != null)
            {
                mAnimator.Update(fTick);
            }
        }

        public void PlayAnimation(string name)
        {
            if (mAnimator != null)
            {
                mAnimator.Play(name);
            }
        }

        public void GetAllState()
        {
            mAnimator.GetAllState();
        }

        public void GetAllParameter()
        {
            mAnimator.GetAllParameter();
        }

        public override void Init()
        {
        }

        public override void Destroy()
        {
        }
    }
}