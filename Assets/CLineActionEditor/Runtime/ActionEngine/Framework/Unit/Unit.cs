/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\Unit.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : property=>表现属性, entity=>数值属性, attribute=>当前运行属性
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-4      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using NumericalType = System.Double;

    public enum EBloodType
    {
        EBT_Green = 0,
        EBT_Red,
    }

    public enum ECombatResult
    {
        ECR_Normal      = 0,    // 普通伤害
        ECR_Critical,           // 暴击伤害
        ECR_SkillNormal,        // 技能伤害
        ECR_SkillCritical,      // 技能暴击
        ECR_Dead,               // 致命伤害
        ECR_Block,              // 格挡
    }

    public enum ECampType
    {
        EFT_Neutrality = 1, // 中立 
        EFT_Friend = 2,     // 友方
        EFT_Enemy = 3,      // 敌方
    }

    public enum EUnitType : byte
    {
        EUT_None = 0,
        EUT_Player,
        EUT_Monster,
        EUT_Summon,

        EUT_Max,
    }

    public enum ECommandType
    {
        ECT_None = 0,
        ECT_BeHurt,
        ECT_ActionStart,
        ECT_ActionEnd,
        ECT_ActionChanging,
        ECT_ActionFinish,
    }

    public enum EAttributeType
    {
        EAT_MaxHp = 0,  // 最大血量
        EAT_CurHP,                  // 当前血量
        EAT_MaxSp,                  // 最大法量
        EAT_CurSp,                  // 当前法量 
        EAT_MoveSpeed,              // 基础移动速度
        EAT_CurMoveSpeed,           // 当前移动速度
        EAT_PhyAtk,                 // 物理攻击
        EAT_PhyDef,                 // 物理防御
        EAT_Tenacity,               // 韧性
        EAT_CriticalRate,           // 暴击
        EAT_CriticalDamage,         // 暴伤
        EAT_Hit,                    // 命中
        EAT_Evade,                  // 闪避

        EAT_AttackSpeed,            // 攻击速度
        EAT_KnockBackSpeed,         // 击退速度

        EAT_Max,
    }

    public abstract class Unit : XObject, IUObjectDetacher
    {
        private UUnit mUObject;
        private CharacterController mUController;
        private Collider mUCollider;
        private ObjectPool mPool;
        private ActionStatus mActionStatus;
        // BUFF
        private BuffManager mBuffManager;

        private Unit mTarget;

        // summon relationship
        private Unit mParent;
        private Dictionary<Unit, float> mChildren = new Dictionary<Unit, float>();
        private LinkedList<Unit> mChildDel = new LinkedList<Unit>();

        // weapon
        private Weapon mEquipWeapon;
        private Dictionary<string, Weapon> mWeaponHash = new Dictionary<string, Weapon>();

        // material
        private LinkedList<Material> mSkinnedMeshMaterialList = new LinkedList<Material>();

        private readonly float mSkinWidth = 0.05f;
        private float mRadius = 0.5f;
        private float mHeight = 2f;
        private bool mEnableCollision = false;
        private Vector3 mBornPosition = Vector3.zero;
        private Quaternion mToOrientation = Quaternion.identity;

        private bool mIsDead = false;
        private bool mIsDeleted = false;

        // movement
        private int mLayerMask; // walk layer
        private bool mOnGround;
        private bool mOnTouchSide;
        private bool mIsGod;

        private ECampType mCamp;
        private int mLevel = 1;

        private string mResID;

        private UnitAttrib mAttrib;
        
        #region property
        public UUnit UUnit
        { get { return mUObject; } }

        public Transform UObject
        { get { return mUObject.transform; } }

        public CharacterController UController
        { get { return mUController; } }

        public Collider UCollider
        { get { return mUCollider; } }

        public Vector3 Position
        { get { return UObject.position; } }

        public Vector3 BornPosition
        { get { return mBornPosition; } }

        public Vector3 CenterPosition
        { get { return Position + new Vector3(0, mHeight * 0.5f, 0); } }

        public ActionStatus ActionStatus
        { get { return mActionStatus; } }

        public BuffManager BuffManager
        { get { return mBuffManager; } }

        public Unit Target
        {
            get { return mTarget; }
            set { mTarget = value; }
        }

        public Unit Parent
        { get { return mParent; } }

        public Dictionary<Unit, float> Children
        {
            get { return mChildren; }
            set { mChildren = value; }
        }

        public PropertyContext PropertyContext
        {
            get;
            set;
        }

        public bool IsDead
        {
            get { return mIsDead; }
            set { mIsDead = value; }
        }

        public bool IsDeleted
        {
            get { return mIsDeleted; }
            set { mIsDeleted = value; }
        }

        public float Radius
        { get { return mRadius; } }

        public float Height
        { get { return mHeight; } }

        public bool EnableCollision
        {
            get { return mEnableCollision; }
            set
            {
                mEnableCollision = value;
                if (null != UController)
                    UController.enabled = value;
                if (null != UCollider)
                    UCollider.enabled = value;
            }
        }

        public ECampType Camp
        {
            get { return mCamp; }
            set { mCamp = value; }
        }

        public Vector3 HitPosition
        {
            get;
            set;
        }

        public int ULayerMask
        { get { return mLayerMask; } }

        public int Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }

        public bool IsGod
        {
            get { return mIsGod || ActionStatus.IsGod; }
            set { mIsGod = value; }
        }
        
        public bool OnGround
        { get { return mOnGround; } }

        public bool OnTouchSide
        { get { return mOnTouchSide; } }

        public Weapon EquipWeapon
        {
            get { return mEquipWeapon; }
            set { mEquipWeapon = value; }
        }

        public string ResID
        {
            get { return mResID; }
            set { mResID = value; }
        }
        public UnitAttrib Attrib
        { get { return mAttrib; } }

        public UIProgressBar HudBlood
        { get; set; }

        public UIHudText HudText
        { get; set; }
        #endregion

            #region func
        public void SetPosition(Vector3 pos)
        {
            UUnit.transform.position = pos;
        }
        public void SetBornPosition(Vector3 pos)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(pos, Vector3.down, out hitInfo, Helper.FLT_MAX, mLayerMask))
            {
                SetPosition(hitInfo.point);
                mBornPosition = hitInfo.point;
            }
            else
            {
                SetPosition(pos);
                mBornPosition = pos;
            }
        }
        public virtual void SetOrientation(float yaw, bool degree, bool smooth)
        {
            float toYaw = degree ? yaw : yaw * Mathf.Rad2Deg;
            Vector3 euler = UUnit.transform.eulerAngles;
            mToOrientation = Quaternion.Euler(euler.x, toYaw, euler.z);
            if (!smooth)
            {
                UUnit.transform.rotation = mToOrientation;
            }
        }
        public virtual void SetOrientation(Vector3 dir, bool smooth)
        {
            dir.y = 0f;
            if (dir.Equals(Vector3.zero))
                return;

            mToOrientation = Quaternion.LookRotation(dir);
            if (!smooth)
            {
                UUnit.transform.rotation = mToOrientation;
            }
        }
        public virtual void SetOrientation(Quaternion qat, bool smooth)
        {
            mToOrientation = qat;
            if (!smooth)
            {
                UUnit.transform.rotation = mToOrientation;
            }
        }
        public virtual void AddChild(Unit unit, float duration)
        {
            unit.mParent = this;
            unit.OnSettedParent();
            mChildren.Add(unit, duration);
        }
        public virtual void OnSettedParent()
        {

        }
        public bool HasChild()
        {
            return mChildren.Count != 0;
        }
        public void SetAnimatorLayerWeight(int layer, int weight)
        {
            UUnit.SetAnimatorLayerWeight(layer, weight);
        }
        public void PlayAnimation(string name)
        {
            UUnit.PlayAnimation(name);
        }
        public void PlayAnimation(EAnimType type, string name, string value)
        {
            UUnit.PlayAnimation(type, name, value);
        }
        public void PlayAnimation(string stateName, float transitionDuration, int layer, float normalizedTime)
        {
            UUnit.PlayAnimation(stateName, transitionDuration, layer, normalizedTime);
        }
        public void MaterialEffectColor(params object[] param)
        {
            Color color = (Color)param[0];
            bool timer = (bool)param[1];

            using (var itr = mSkinnedMeshMaterialList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.SetColor("_Color", color);
                }
            }

            if (timer)
            {
                object[] paramList = new object[] { Color.white, false };
                TimerMgr.Instance.AddTimer(0.25f, false, 0, MaterialEffectColor, paramList);
            }
        }
        public void PlayStraightAnimation(params object[] param)
        {
            float time = (float)param[0];//硬直时间
            float speed = (float)param[1];//动作速度
            bool timer = (bool)param[2];

            if (UUnit == null || IsDead)
                return;

            ActionStatus.RestrictFrozen = timer;
            float curspeed = UUnit.Anim.speed;
            UUnit.SetAnimationSpeed(speed);

            if (timer)
            {
                object[] paramList = new object[] { 0f, curspeed, false };
                TimerMgr.Instance.AddTimer(time, false, 0, PlayStraightAnimation, paramList);
            }
        }
        public void PlayDizzyAnimation(params object[] param)
        {
            float time = (float)param[0];//眩晕时间
            float speed = (float)param[1];//动作速度
            bool timer = (bool)param[2];

            if (UUnit == null || IsDead)
                return;

            ActionStatus.RestrictFrozen = timer;
            float curspeed = UUnit.Anim.speed;
            UUnit.SetAnimationSpeed(speed);

            if (timer)
            {
                Transform tr = Helper.Find(UObject, "dummy_head");
                EffectMgr.Instance.PlayEffect("effect_dizzy", tr.gameObject, Vector3.zero, Vector3.zero);
                object[] paramList = new object[] { 0f, curspeed, false };
                // TO CLine: time == effect time
                TimerMgr.Instance.AddTimer(time, false, 0, PlayDizzyAnimation, paramList);
            }
        }
        public void ChangeGodMode(params object[] param)
        {
            bool isGod = (bool)param[0];
            float time = (float)param[1];//无敌时间

            if (UUnit == null || IsDead)
                return;

            ActionStatus.IsGod = isGod;
            if (isGod)
            {
                object[] paramList = new object[] { false, 0f };
                TimerMgr.Instance.AddTimer(time, false, 0, ChangeGodMode, paramList);
            }
        }
        public void BuildLayerMask()
        {
            for (int i = 0; i < 32; ++i)
            {
                if (!Physics.GetIgnoreLayerCollision(UUnit.gameObject.layer, i))
                {
                    string layer = LayerMask.LayerToName(i);
                    if (layer == "Default")// walking with collider layer
                        mLayerMask |= (1 << i);
                }
            }
        }
        public void Move(Vector3 offset)
        {
            if (UnitType == EUnitType.EUT_Player)
            {
                if (offset.y != 0)
                    mOnGround = false;
                if (UController != null && UController.enabled)
                {
                    //float y = UUnit.transform.position.y;
                    CollisionFlags collisionFlags = UController.Move(offset);

                    if ((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below)
                        mOnGround = true;
                    if ((collisionFlags & CollisionFlags.Sides) == CollisionFlags.Sides)
                        mOnTouchSide = true;
                    else if (offset.x != 0f || offset.z != 0f)
                        mOnTouchSide = false;
                }
                else
                {
                    SetPosition(Position + offset);
                }

            }
            else
            {
                if (mEnableCollision)
                {
                    float radius2 = 2 * mRadius;
                    // xoz
                    if (offset.x != 0f && offset.z != 0f)
                    {
                        Vector3 trans = new Vector3(offset.x, 0, offset.z);
                        Vector3 dir = trans.normalized;
                        float dist = trans.magnitude + radius2 + mSkinWidth;
                        Vector3 origin = new Vector3(Position.x, Position.y + radius2, Position.z) - dir * mSkinWidth;

                        mOnTouchSide = false;
                        RaycastHit hitInfo;
                        if (Physics.Raycast(origin, dir, out hitInfo, dist, mLayerMask))
                        {
                            mOnTouchSide = true;
                            float num = hitInfo.distance - radius2;

                            offset.x = (num > 0f ? dir.x * num : 0f);
                            offset.z = (num > 0f ? dir.z * num : 0f);

                        }
                    }

                    // y
                    mOnGround = false;
                    if (offset.y < 0f)
                    {
                        Vector3 origin = new Vector3(Position.x, Position.y + radius2, Position.z);
                        float dist = radius2 - offset.y;

                        RaycastHit hitInfo;
                        if (Physics.Raycast(origin, Vector3.down, out hitInfo, dist, mLayerMask))
                        {
                            float num = hitInfo.distance - radius2;
                            offset.y = (Mathf.Abs(num) <= 0.001f ? 0f : -num);

                            mOnGround = true;
                        }
                    }
                }
                else
                {
                    mOnGround = false;
                }

                SetPosition(Position + offset);
            }
        }
        void UpdateChildren(float fTick)
        {
            using (var itr = mChildren.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Value != -1)
                        mChildren[itr.Current.Key] -= fTick;

                    if (mChildren[itr.Current.Key] <= 0 && mChildren[itr.Current.Key] != -1)
                        mChildDel.AddLast(itr.Current.Key);
                }
            }

            using (var itr = mChildDel.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    Unit unit = itr.Current;
                    if (!unit.IsDead)
                    {
                        //unit.ChangeActionDead();
                        unit.IsDead = true;
                        mChildren.Remove(unit);
                    }
                }
            }

            mChildDel.Clear();
        }

        public void AddBuff(string id)
        {
            mBuffManager.AddBuff(id);
        }

        public void DelBuff(string id)
        {
            mBuffManager.DelBuff(id);
        }

        public void AddBuff(List<string> li)
        {
            using (List<string>.Enumerator itr = li.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mBuffManager.AddBuff(itr.Current);
                }
            }
        }

        public void DelBuff(List<string> li)
        {
            using (List<string>.Enumerator itr = li.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mBuffManager.DelBuff(itr.Current);
                }
            }
        }
        #endregion

        public abstract EUnitType UnitType { get; }
        public abstract string ModelName { get; }
        public abstract void InitProperty(string resID);

        #region base impl
        protected override void OnDispose()
        {
            if (null != HudBlood)
            {
                HudMgr.Instance.CycleHudBlood(HudBlood);
                HudBlood = null;
            }

            if (null != HudText)
            {
                HudMgr.Instance.CycleHudText(HudText);
                HudText = null;
            }

            if (null != mBuffManager)
            {
                mBuffManager.Dispose();
                mBuffManager = null;
            }

            if (null != mUObject)
            {
                mPool.Cycle(mUObject.gameObject);
                mUObject = null;
            }

            if (mParent != null)
                mParent.RemoveChild(this);

            mEquipWeapon = null;
            using (var itr = mWeaponHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Value != null)
                        itr.Current.Value.Dispose();
                }
            }
            mWeaponHash.Clear();

            mIsDead = true;
            mIsDeleted = true;
            mUObject = null;
            mUCollider = null;
            mUController = null;
            mPool = null;
            mActionStatus = null;
            mTarget = null;
            mParent = null;
            mAttrib = null;

            mChildren.Clear();
            //mCustomPropertyHash.Clear();
            PropertyContext = null;
            mSkinnedMeshMaterialList.Clear();
        }

        public void OnDetached(MonoBehaviour obj)
        {
            if (mUObject != null && mUObject.gameObject == obj.gameObject)
            {
                mUObject = null;
                // TO CLine: don't cycle to pool
            }
        }

        #endregion

        #region virtual
        public virtual void PreUpdate(float fTick)
        { }
        public virtual void PostUpdate(float fTick)
        { }
        public virtual void Update(float fTick)
        {
            if (!IsDead)
            {
                mBuffManager.Update(fTick);
            }

            Attrib.CurSpeed = (float)GetAttribute(EAttributeType.EAT_CurMoveSpeed);

            if (Target != null && Target.IsDead)
                Target = null;

            UpdateChildren(fTick);
        }
        public virtual void FixedUpdate(float fTick)
        { }
        public virtual void LateUpdate(float fTick)
        { }
        public virtual void UpdateOrientation(float fTick)
        {
            UObject.transform.rotation = Quaternion.Slerp(UObject.transform.rotation, mToOrientation, 8 * fTick);

            if (Quaternion.Angle(UObject.transform.rotation, mToOrientation) < 1)
            {
                UObject.transform.rotation = mToOrientation;
            }
        }
        public virtual void OnMessage(Message msg)
        { }

        public virtual void Init(string resID, Vector3 pos, float yaw, ECampType campType, string debugName = null)
        {
            ResID = resID;

            InitProperty(resID);

            mPool = ObjectPoolMgr.Instance.GetPool(ModelName);

            GameObject go = mPool.Get();
            mUObject = go.GetComponent<UUnit>();
            if (mUObject == null)
                mUObject = go.AddComponent<UUnit>();
            mUObject.Owner = this;

            if (!string.IsNullOrEmpty(debugName))
            {
                mUObject.gameObject.name = debugName;
            }

            Init(pos, yaw, campType);
        }

        protected void Init(Vector3 pos, float yaw, ECampType campType)
        {
            mUController = mUObject.GetComponent<CharacterController>();
            if (mUController != null)
            {
                mUController.enabled = false;
                mRadius = Mathf.Max(mRadius, mUController.radius);
            }

            mUCollider = mUObject.GetComponent<CapsuleCollider>();
            if (mUCollider != null)
            {
                CapsuleCollider cc = mUCollider as CapsuleCollider;
                mRadius = cc.radius;
                mHeight = cc.height;
                cc.enabled = false;
            }
            else
            {
                mUCollider = mUObject.GetComponent<Collider>();
            }
            mRadius *= UUnit.transform.localScale.x;
            mHeight *= UUnit.transform.localScale.y;

            // physics movement layer
            BuildLayerMask();

            // material
            SkinnedMeshRenderer[] smrs = UObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < smrs.Length; ++i)
            {
                smrs[i].material.SetColor("_Color", Color.white);
                mSkinnedMeshMaterialList.AddLast(smrs[i].material);
            }

            mIsGod = false;
            mIsDead = false;
            mIsDeleted = false;
            mOnGround = false;
            mOnTouchSide = false;
            mCamp = campType;

            SetBornPosition(pos);
            SetOrientation(yaw, true, false);

            PropertyContext = new PropertyContext();

            mAttrib = new UnitAttrib();
            mActionStatus = new ActionStatus(this);
            mBuffManager = new BuffManager(this);

        }

        public virtual void UseWeapon(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            if (mEquipWeapon != null)
                mEquipWeapon.UnBind();

            Weapon wp = null;
            if (mWeaponHash.ContainsKey(id))
            {
                wp = mWeaponHash[id];
            }
            else
            {
                wp = CreateWeapon(id);
                mWeaponHash.Add(id, wp);
            }

            if (!string.IsNullOrEmpty(wp.Property.SetupAction))
            {
                bool changeaction = true;
                if (this is Player)
                {
                    Player p = this as Player;
                    if (p.ActionStatus.ActiveAction.ActionStatus == EActionState.Skill)
                        changeaction = false;
                }
                if (changeaction)
                    ActionStatus.ChangeAction(wp.Property.SetupAction, mWeaponHash.Count > 1 ? 1 : 0);
            }

            mEquipWeapon = wp;
            mEquipWeapon.Bind();
        }

        public virtual Weapon CreateWeapon(string id)
        {
            // "WQ" + "00"(type) + "000"(number)
            if (string.IsNullOrEmpty(id) || id.Length != 7 || !id.Contains("WQ"))
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Unit", "weapon id({0}) is error!!!", id);
                return null;
            }

            Weapon weapon = null;
            WeaponProperty wp = PropertyMgr.Instance.GetWeaponProperty(id);
            switch (wp.WeaponType)
            {
                case WeaponProperty.EWeaponType.EWT_Sword:
                    weapon = new Sword(this, wp);
                    break;
                default:
                    LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Unit", "Failed to create weapon({0})!!!", id);
                    break;
            }

            return weapon;
        }

        public virtual void OnCommand(ECommandType cmdType, params object[] cmdArgs)
        {
            switch (cmdType)
            {
                case ECommandType.ECT_ActionStart:
                    {
                        OnActionStart(cmdArgs[0] as Action);
                    }
                    break;
                case ECommandType.ECT_ActionEnd:
                    {
                        OnActionEnd(cmdArgs[0] as Action, (bool)cmdArgs[1]);
                    }
                    break;
                case ECommandType.ECT_ActionChanging:
                    {
                        OnActionChanging(cmdArgs[0] as Action, cmdArgs[1] as Action, (bool)cmdArgs[2]);
                    }
                    break;
                case ECommandType.ECT_ActionFinish:
                    {
                        OnActionFinish(cmdArgs[0] as Action);
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual ECombatResult Combat(Unit attackee, out NumericalType damage)
        {
            ECombatResult ret = ECombatResult.ECR_Normal;
            bool useSkill = ActionStatus.ActiveAction.ActionStatus == EActionState.Skill;
            if (useSkill)
            {
                ret = ECombatResult.ECR_SkillNormal;
            }

            // TO CLine: test, using combat formula at later.
            damage = UnityEngine.Random.Range(1, 15);

            return ret;
        }

        public virtual bool BeHurt(Unit attacker, double damage, ECombatResult result)
        {
            if (IsGod || !ActionStatus.CanHurt)
            {
                return true;
            }

            if (result == ECombatResult.ECR_Block)
            {
                AddHP(0, ECombatResult.ECR_Block);
            }
            else
            {
                AddHP(-damage, result);
            }

            return true;
        }

        public virtual void AddHP(double hp, ECombatResult result = ECombatResult.ECR_Normal)
        {
            if (result == ECombatResult.ECR_Dead)
            {
                Attrib.CurHP = 0;
            }
            else
            {
                Attrib.CurHP += hp;
                Attrib.CurHP = Helper.Clamp(Attrib.CurHP, 0, GetAttribute(EAttributeType.EAT_MaxHp));
            }

            IsDead = (Attrib.CurHP == 0 ? true : false);

            if (IsDead)
            {
                //EnableCollision = false;
                RemoveChild();
            }

        }

        public virtual void OnActionStart(Action action)
        {

        }

        public virtual void OnActionEnd(Action action, bool interrupt)
        {

        }

        public virtual void OnActionChanging(Action oldAction, Action newAction, bool interrupt)
        {

        }

        public virtual void OnActionFinish(Action action)
        {

        }

        public virtual void RemoveChild(Unit unit)
        {
            unit.mParent = null;
            mChildren.Remove(unit);
        }

        public virtual void RemoveChild()
        {
            using (Dictionary<Unit, float>.Enumerator itr = mChildren.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    Unit unit = itr.Current.Key;
                    if (!unit.IsDead)
                    {
                        //unit.ChangeActionDead();
                        unit.IsDead = true;
                    }
                }
            }
        }

        public virtual void UpdateAttributes(byte[] buf)
        {

        }

        public virtual NumericalType GetAttribute(EAttributeType type)
        {
            return 0;
        }

        #endregion

        public virtual void LocalMove(Vector2 move) { }
        public virtual void LocalMoveStop() { }
        public virtual void LocalAttack(string btnName) { }
        public virtual void LocalLongPressed(string btnName) { }
        public virtual void LocalLongPressedEnd(string btnName) { }

        public virtual bool HasInputStatus()
        {
            return false;
        }
    }
}
