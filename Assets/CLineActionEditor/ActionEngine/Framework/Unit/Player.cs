/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\Player.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-4      CLine           Created
|
+-----------------------------------------------------------------------------*/

using System;
using UnityEngine;

namespace CAE.Core
{
    using NumericalType = System.Double;

    public class Player : Unit
    {
        protected PlayerProperty mProperty = null;

        public override string ModelName
        {
            get{ return mProperty.Prefab; }
        }

        public override EUnitType UnitType
        {
            get { return EUnitType.EUT_Player; }
        }

        public override void InitProperty(string resID)
        {
            mProperty = PropertyMgr.Instance.GetPlayerProperty(resID);
        }

        public Player() : base()
        {

        }

        protected override void OnDispose()
        {
            mProperty = null;

            base.OnDispose();
        }

        public override void Init(string resID, Vector3 pos, float yaw, ECampType campType, string debugName = null)
        {
            base.Init(resID, pos, yaw, campType, debugName);

            // action group
            ActionStatus.ActionGroup = mProperty.ActionGroup;

            // collision
            EnableCollision = true;

            // hud
            Transform hud = Helper.Find(UObject, "hud");
            EBloodType bloodType = campType == ECampType.EFT_Friend ? EBloodType.EBT_Green : EBloodType.EBT_Red;
            HudBlood = HudMgr.Instance.GetHudBlood(hud, bloodType);
            HudText = HudMgr.Instance.GetHudText(hud);

            // custom variable
            CustomPropertyHash.Add(CustomProperty.sInputMove, new CustomProperty(CustomProperty.sInputMove, EProperyType.EPT_Bool, false));
            CustomPropertyHash.Add(CustomProperty.sInputAttack, new CustomProperty(CustomProperty.sInputAttack, EProperyType.EPT_Bool, false));
            CustomPropertyHash.Add(CustomProperty.sInputRoll, new CustomProperty(CustomProperty.sInputRoll, EProperyType.EPT_Bool, false));
            CustomPropertyHash.Add(CustomProperty.sInputJump, new CustomProperty(CustomProperty.sInputJump, EProperyType.EPT_Bool, false));
            CustomPropertyHash.Add(CustomProperty.sAttackCount, new CustomProperty(CustomProperty.sAttackCount, EProperyType.EPT_Int, 0));
            CustomPropertyHash.Add(CustomProperty.sInputSkill, new CustomProperty(CustomProperty.sInputSkill, EProperyType.EPT_String, ""));
            CustomPropertyHash.Add(CustomProperty.sInputSwitch, new CustomProperty(CustomProperty.sInputSwitch, EProperyType.EPT_Bool, false));
            CustomPropertyHash.Add(CustomProperty.sSkillCount, new CustomProperty(CustomProperty.sSkillCount, EProperyType.EPT_Int, 0));
            CustomPropertyHash.Add(CustomProperty.sVelocityY, new CustomProperty(CustomProperty.sVelocityY, EProperyType.EPT_Float, 0f));
            CustomPropertyHash.Add(CustomProperty.sAttackCounting, new CustomProperty(CustomProperty.sAttackCounting, EProperyType.EPT_Int, 0));
            CustomPropertyHash.Add(CustomProperty.sKeepAttakRandom, new CustomProperty(CustomProperty.sKeepAttakRandom, EProperyType.EPT_Float, 0f));
            CustomPropertyHash.Add(CustomProperty.sSkillCounting, new CustomProperty(CustomProperty.sSkillCounting, EProperyType.EPT_Int, 0));
            CustomPropertyHash.Add(CustomProperty.sJumpCount, new CustomProperty(CustomProperty.sJumpCount, EProperyType.EPT_Int, 0));
            CustomPropertyHash.Add(CustomProperty.sJumpCounting, new CustomProperty(CustomProperty.sJumpCounting, EProperyType.EPT_Int, 0));
            CustomPropertyHash.Add(CustomProperty.sInputLongPressed, new CustomProperty(CustomProperty.sInputLongPressed, EProperyType.EPT_Bool, false));
            CustomPropertyHash.Add(CustomProperty.sInputSkillPosition, new CustomProperty(CustomProperty.sInputSkillPosition, EProperyType.EPT_Vector3, Vector3.positiveInfinity));
            CustomPropertyHash.Add(CustomProperty.sSearchDist, new CustomProperty(CustomProperty.sSearchDist, EProperyType.EPT_Float, 10f));

            // action
            ActionStatus.ChangeAction(mProperty.StartupAction);
        }

        public override void Update(float fTick)
        {
            base.Update(fTick);

            ActionStatus.Update(fTick);

            if (Target == null)
            {
                float radius = (float)CustomPropertyHash[CustomProperty.sSearchDist].Value;
                Unit[] units = Helper.Search(Position, radius, LayerMask.GetMask("Role", "LocalPlayer"), new Transform[] { UObject }, Camp, true, Vector3.zero);
                if (units.Length > 0)
                {
                    Target = units[0];
                }
            }
        }
        
        public override void OnActionStart(Action action)
        {
            base.OnActionStart(action);

            CustomPropertyHash[CustomProperty.sInputSkill].Value = "";
            CustomPropertyHash[CustomProperty.sInputAttack].Value = false;
            CustomPropertyHash[CustomProperty.sInputJump].Value = false;
        }

        public override void OnActionChanging(Action oldAction, Action newAction, bool interrupt)
        {
            base.OnActionChanging(oldAction, newAction, interrupt);

            //LogMgr.Instance.Logf(ELogType.ELT_DEBUG, "Action", "---OnActionChanging{0}---{1} ===> {2}    {3}", c++, oldAction != null ? oldAction.ID : "null", newAction.ID, interrupt);
        }

        public override void OnActionEnd(Action action, bool interrupt)
        {
            base.OnActionEnd(action, interrupt);

        }

        protected void KeepAttack()
        {
            int attackcount = (int)CustomPropertyHash[CustomProperty.sAttackCounting].Value;
            int count = attackcount;
            //if (ActionStatus.ActiveAction.ActionStatus != EActionState.Attack && attackcount == 0)
            if (attackcount == 0)
            {
                count = 1;
            }
            else if (ActionStatus.ActiveAction.ActionStatus == EActionState.Attack && ActionStatus.EnableBreak)
            {
                switch (attackcount)
                {
                    case 1:
                        count = 2;
                        break;
                    case 2:
                        count = 3;
                        CustomPropertyHash[CustomProperty.sKeepAttakRandom].Value = UnityEngine.Random.Range(0f, 1f);
                        break;
                    case 3:
                        count = 4;
                        break;
                    case 4:
                        count = 5;
                        break;
                    case 5:
                        count = 6;
                        break;
                }
            }

            CustomPropertyHash[CustomProperty.sAttackCounting].Value = count;
            CustomPropertyHash[CustomProperty.sAttackCount].Value = count;
            CustomPropertyHash[CustomProperty.sInputAttack].Value = true;
        }

        protected void KeepSkill()
        {
            int skillcount = (int)CustomPropertyHash[CustomProperty.sSkillCounting].Value;
            int count = skillcount;

            if (skillcount == 0)
            {
                count = 1;
            }
            else if (ActionStatus.ActiveAction.ActionStatus == EActionState.Skill && ActionStatus.EnableBreak)
            {
                switch (skillcount)
                {
                    case 1:
                        count = 2;
                        break;
                    case 2:
                        count = 3;
                        break;
                    case 3:
                        count = 4;
                        break;
                    case 4:
                        count = 5;
                        break;
                }
            }

            CustomPropertyHash[CustomProperty.sSkillCounting].Value = count;
            CustomPropertyHash[CustomProperty.sSkillCount].Value = count;
        }

        protected void KeepJump()
        {
            int jumpcount = (int)CustomPropertyHash[CustomProperty.sJumpCounting].Value;
            int count = jumpcount;

            if (jumpcount == 0)
            {
                count = 1;
            }
            else if (ActionStatus.ActiveAction.ActionStatus == EActionState.Jump && ActionStatus.EnableBreak)
            {
                switch (jumpcount)
                {
                    case 1:
                        count = 2;
                        break;
                    //case 2:
                    //    count = 3;
                    //    break;
                    //case 3:
                    //    count = 4;
                    //    break;
                }
            }

            CustomPropertyHash[CustomProperty.sJumpCounting].Value = count;
            CustomPropertyHash[CustomProperty.sJumpCount].Value = count;
            CustomPropertyHash[CustomProperty.sInputJump].Value = true;
        }

        public override void UpdateAttributes(byte[] buf)
        {
            base.UpdateAttributes(buf);

            if (buf == null)
            {
                // TO CLine: update attr from entity
                Attrib.BaseHP = 100;
                Attrib.CurHP = 100;
                Attrib.BaseSpeed = 5f;
                Attrib.CurSpeed = 5f;
            }
            else
            {
                // TO CLine: update attr from server
            }
        }

        public override NumericalType GetAttribute(EAttributeType type)
        {
            NumericalType num = 0;
            switch (type)
            {
                case EAttributeType.EAT_MaxHp:
                    return Attrib.BaseHP;
                case EAttributeType.EAT_MoveSpeed:
                    return Attrib.BaseSpeed;
                case EAttributeType.EAT_CurMoveSpeed:
                    num = Attrib.BaseSpeed;
                    break;
            }

            return BuffManager.Apply(type, num);
        }

        public override void AddHP(double hp, ECombatResult result = ECombatResult.ECR_Normal)
        {
            base.AddHP(hp, result);

            if (!IsDead)
            {
                HudText.ShowText(EHudType.EHT_PlayerBehurt, hp, 0);
                HudBlood.Progress = (float)(Attrib.CurHP / GetAttribute(EAttributeType.EAT_MaxHp));
            }
            else
            {
                if (null != HudBlood)
                {
                    HudMgr.Instance.CycleHudBlood(HudBlood);
                    HudBlood = null;
                }
            }
        }

    }
}
