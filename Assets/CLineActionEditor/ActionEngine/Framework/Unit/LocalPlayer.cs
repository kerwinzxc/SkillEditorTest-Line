/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\LocalPlayer.cs
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


namespace CAE.Core
{
    using UnityEngine;

    public sealed class LocalPlayer : Player
    {
        public LocalPlayer() : base()
        { }

        public override EUnitType UnitType
        {
            get { return EUnitType.EUT_LocalPlayer; }
        }

        protected override void OnDispose()
        {
            MessageMgr.Instance.Unregister("CMD_MOVESTART", OnMessageMoveStart);
            MessageMgr.Instance.Unregister("CMD_MOVE", OnMessageMove);
            MessageMgr.Instance.Unregister("CMD_MOVESTOP", OnMessageMoveStop);
            MessageMgr.Instance.Unregister("CMD_ATTACK", OnMessageAttack);
            MessageMgr.Instance.Unregister("CMD_SKILL", OnMessageSkill);
            MessageMgr.Instance.Unregister("CMD_LONGPRESSED", OnLongPressed);
            MessageMgr.Instance.Unregister("CMD_LONGPRESSEDEND", OnLongPressedEnd);


            base.OnDispose();
        }

        public override void Init(string resID, Vector3 pos, float yaw, ECampType campType, string debugName = null)
        {
            base.Init(resID, pos, yaw, campType, debugName);

            // message
            MessageMgr.Instance.Register("CMD_MOVESTART", OnMessageMoveStart);
            MessageMgr.Instance.Register("CMD_MOVE", OnMessageMove);
            MessageMgr.Instance.Register("CMD_MOVESTOP", OnMessageMoveStop);
            MessageMgr.Instance.Register("CMD_ATTACK", OnMessageAttack);
            MessageMgr.Instance.Register("CMD_SKILL", OnMessageSkill);
            MessageMgr.Instance.Register("CMD_LONGPRESSED", OnLongPressed);
            MessageMgr.Instance.Register("CMD_LONGPRESSEDEND", OnLongPressedEnd);

            // camera
            CameraMgr.Instance.BindCamera(GameObject.FindWithTag("MainCamera").transform);

            CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_SmoothFollow, UObject);
        }

        public override void Update(float fTick)
        {
            base.Update(fTick);

            
        }

        public override void OnMessage(Message msg)
        {
            
        }

        private void OnMessageMoveStart(Message msg)
        {

        }

        private void OnMessageMove(Message msg)
        {
            if (!ActionStatus.CanMove)
                return;

            Vector2 move = (Vector2)msg.GetArg("Joystick");

            Vector3 camDir = CameraMgr.Instance.Camera.forward * move.y + CameraMgr.Instance.Camera.right * move.x;

            Vector3 moveDir = camDir.normalized * Attrib.CurSpeed * Time.deltaTime;
            float x = moveDir.x;
            float z = moveDir.z;

            Helper.Rotate(ref x, ref z, 0, false);
            if (ActionStatus.CanRotate && !(ActionStatus.FaceTarget && Target != null))
            {
                SetOrientation(Mathf.Atan2(x, z), false);
            }

            if (ActionStatus.ActiveAction.ActionStatus == EActionState.Jump && !OnGround)
            {
                Move(new Vector3(x, 0f, z));
            }
            else
            {
                Move(new Vector3(x, -UController.stepOffset, z));

                ActionStatus.IgnoreMove = true;
                CustomPropertyHash[CustomProperty.sInputMove].Value = true;
            }
                
        }

        private void OnMessageMoveStop(Message msg)
        {
            ActionStatus.IgnoreMove = false;
            CustomPropertyHash[CustomProperty.sInputMove].Value = false;
        }

        private void OnMessageAttack(Message msg)
        {
            if (ActionStatus.ActiveAction.ActionStatus == EActionState.Skill)
            {
                KeepSkill();
            }
            else
            {
                KeepAttack();
            }
        }

        private void OnMessageSkill(Message msg)
        {
            string skillName = (string)msg.GetArg("SkillSlot");
            switch (skillName)
            {
                case "Skill_Slot1":
                    {
                        KeepSkill();
                        CustomPropertyHash[CustomProperty.sInputSkill].Value = "skill_air_atk";
                    }
                    break;
                case "Skill_Slot2":
                    {
                        CustomPropertyHash[CustomProperty.sInputSkill].Value = "skill_hitground";
                    }
                    break;
                case "Skill_Slot3":
                    {
                        CustomPropertyHash[CustomProperty.sInputSkill].Value = "skill_xuanfengzhan";
                    }
                    break;
                case "Skill_Slot4":
                    {
                        KeepJump();
                    }
                    break;
            }
        }

        private void OnLongPressed(Message msg)
        {
            CustomPropertyHash[CustomProperty.sInputLongPressed].Value = true;
        }

        private void OnLongPressedEnd(Message msg)
        {
            CustomPropertyHash[CustomProperty.sInputLongPressed].Value = false;
        }

    }
}
