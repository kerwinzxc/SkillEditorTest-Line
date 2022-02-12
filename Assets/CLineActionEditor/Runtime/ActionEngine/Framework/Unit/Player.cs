/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\Player.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
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

using System;
using UnityEngine;

namespace SuperCLine.ActionEngine
{
    using NumericalType = System.Double;

    public class Player : Unit
    {
        private Vector2 mInputMoveDir = Vector2.zero;
        protected PlayerProperty mProperty = null;
        private TargetStatus mTargetSystem = null;

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
            if (mTargetSystem != null)
            {
                mTargetSystem.Destroy();
                mTargetSystem = null;
            }

            base.OnDispose();
        }

        public override void Init(string resID, Vector3 pos, float yaw, ECampType campType, string debugName = null)
        {
            base.Init(resID, pos, yaw, campType, debugName);

            mTargetSystem = new TargetStatus() { owner = this };

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
            PropertyContext.AddProperty(PropertyName.sInputMove, ETAnyType.Bool);
            PropertyContext.AddProperty(PropertyName.sInputAttack, ETAnyType.Bool);
            PropertyContext.AddProperty(PropertyName.sInputRoll, ETAnyType.Bool);
            PropertyContext.AddProperty(PropertyName.sInputJump, ETAnyType.Bool);
            PropertyContext.AddProperty(PropertyName.sInputSwitch, ETAnyType.Bool);
            PropertyContext.AddProperty(PropertyName.sInputLongPressed, ETAnyType.Bool);
            PropertyContext.AddProperty(PropertyName.VarBoolTag, ETAnyType.Bool);

            PropertyContext.AddProperty(PropertyName.sInputSkill, ETAnyType.String);
            PropertyContext.AddProperty(PropertyName.VarStringTag, ETAnyType.String);

            PropertyContext.AddProperty(PropertyName.VarInt32AttackRandom, ETAnyType.Int32);

            PropertyContext.AddProperty(PropertyName.sVelocityY, ETAnyType.Float);
            PropertyContext.AddProperty<float>(PropertyName.sSearchDist, ETAnyType.Float, 5f);

            PropertyContext.AddProperty<Vector3>(PropertyName.sInputSkillPosition, ETAnyType.Vector3, Vector3.positiveInfinity);

            // action
            ActionStatus.ChangeAction(mProperty.StartupAction);
        }

        public override void Update(float fTick)
        {
            base.Update(fTick);

            mTargetSystem.Update(fTick);

            ActionStatus.Update(fTick);
            UpdateOrientation(fTick);
        }

        public override void PostUpdate(float fTick)
        {
            base.PostUpdate(fTick);

            Helper.SetAny<string>(PropertyContext.GetProperty(PropertyName.sInputSkill), "");
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputAttack), false);
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputJump), false);
        }

        public override void OnActionStart(Action action)
        {
            base.OnActionStart(action);

            Helper.SetAny<string>(PropertyContext.GetProperty(PropertyName.sInputSkill), "");
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputAttack), false);
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputJump), false);
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

        public override void LocalMove(Vector2 move)
        {
            Vector3 camDir = CameraMgr.Instance.Camera.transform.forward * move.y + CameraMgr.Instance.Camera.transform.right * move.x;

            Vector3 moveDir = camDir.normalized * Attrib.CurSpeed * Time.deltaTime;
            float x = moveDir.x;
            float z = moveDir.z;

            mInputMoveDir.x = x;
            mInputMoveDir.y = z;

            Helper.Rotate(ref x, ref z, 0, false);
            if (ActionStatus.CanRotate && !(ActionStatus.FaceTarget && Target != null))
            {
                SetOrientation(Mathf.Atan2(x, z), false, true);
            }

            if (ActionStatus.CanMove)
            {
                if (ActionStatus.ActiveAction.ActionStatus == EActionState.Jump && !OnGround)
                {
                    Move(new Vector3(x, 0f, z));
                }
                else
                {
                    Move(new Vector3(x, -UController.stepOffset, z));
                }
            }

            ActionStatus.IgnoreMove = true;
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputMove), true);
        }

        public override void LocalMoveStop()
        {
            mInputMoveDir = Vector2.zero;
            ActionStatus.IgnoreMove = false;
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputMove), false);
        }

        public override void LocalAttack(string btnName)
        {
            switch (btnName)
            {
                case "Skill_Attack":
                    Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputAttack), true);
                    break;
                case "Skill_Slot1":
                    {
                        Helper.SetAny<string>(PropertyContext.GetProperty(PropertyName.sInputSkill), "skill_air_atk");
                    }
                    break;
                case "Skill_Slot2":
                    {
                        Helper.SetAny<string>(PropertyContext.GetProperty(PropertyName.sInputSkill), "skill_hitground");
                    }
                    break;
                case "Skill_Slot3":
                    {
                        Helper.SetAny<string>(PropertyContext.GetProperty(PropertyName.sInputSkill), "skill_xuanfengzhan");
                    }
                    break;
                case "Skill_Slot4":
                    {
                        Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputJump), true);
                    }
                    break;
            }
        }

        public override void LocalLongPressed(string btnName)
        {
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputLongPressed), true);
        }

        public override void LocalLongPressedEnd(string btnName)
        {
            Helper.SetAny<bool>(PropertyContext.GetProperty(PropertyName.sInputLongPressed), false);
        }

        public override bool HasInputStatus()
        {
            return mInputMoveDir != Helper.Vec2Zero;
        }
    }
}
