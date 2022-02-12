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
    using UnityEditor;
    using UnityEditor.Animations;
    using System.Collections.Generic;

    public class UnitWrapper : Singleton<UnitWrapper>
    {
        private bool mIsReady = false;
        private GameObject mUnit = null;
        private GameObject mWeapon = null;
        private WeaponProperty mWeaponProperty = null;
        private Animator mAnim = null;
        private string mActionGroupName = string.Empty;

        private List<string> mStateNameList = new List<string>();
        private List<string> mParameterList = new List<string>();
        private Dictionary<string, AnimatorState> mStateHash = new Dictionary<string, AnimatorState>();

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
            get { return mStateNameList; }
        }

        public List<string> ParameterList
        {
            get { return mParameterList; }
        }

        public Dictionary<string, AnimatorState> StateHash
        {
            get { return mStateHash; }
        }

        public GameObject UnitWrapperUnit
        {
            get { return mUnit; }
            set { mUnit = value; }
        }

        public Animator Anim
        {
            get { return mAnim; }
            set { mAnim = value; }
        }

        public GameObject EquipWeapon
        {
            get { return mWeapon; }
            set { mWeapon = value; }
        }

        public bool BuildUnit(IProperty property)
        {
            string prefabname = string.Empty;
            if (mUnit != null)
            {
                GameObject.DestroyImmediate(mUnit);
            }

            if (property is PlayerProperty)
            {
                PlayerProperty p = property as PlayerProperty;
                prefabname = p.Prefab;
                mActionGroupName = p.ActionGroup;
            }
            else if (property is MonsterProperty)
            {
                MonsterProperty m = property as MonsterProperty;
                prefabname = m.Prefab;
                mActionGroupName = m.ActionGroup;
            }

            if (string.IsNullOrEmpty(prefabname) || string.IsNullOrEmpty(mActionGroupName))
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
            mAnim = mUnit.GetComponent<Animator>();

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
            if (mAnim != null)
            {
                mAnim.Update(fTick);
            }
        }

        public void PlayAnimation(string name)
        {
            if (mAnim != null)
            {
                mAnim.Play(name, 0, 0);
            }
        }

        public void GetAllState()
        {
            mStateNameList.Clear();
            mStateHash.Clear();

            AnimatorController ac = mAnim.runtimeAnimatorController as AnimatorController;
            // TO CLine: more layer later
            ChildAnimatorState[] stList = ac.layers[0].stateMachine.states;

            for (int i = 0; i < stList.Length; ++i)
            {
                mStateNameList.Add(stList[i].state.name);
                mStateHash.Add(stList[i].state.name, stList[i].state);
            }
        }

        public void GetAllParameter()
        {
            mParameterList.Clear();

            for (int i = 0; i < mAnim.parameters.Length; ++i)
            {
                mParameterList.Add(mAnim.parameters[i].name);
            }
        }

        public override void Init()
        {
        }

        public override void Destroy()
        {
        }
    }
}