/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\Monster.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-3      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;
    using UnityEngine.AI;
    using NumericalType = System.Double;

    public class Monster : Unit
    {
        protected int mSearchLayerMask = 0;
        protected MonsterProperty mProperty = null;
        protected AIStatus mAI = null;
        private TargetHateStatus mTargetSystem = null;

        public override string ModelName
        {
            get { return mProperty.Prefab; }
        }

        public override EUnitType UnitType
        {
            get { return EUnitType.EUT_Monster; }
        }

        public override void InitProperty(string resID)
        {
            mProperty = PropertyMgr.Instance.GetMonsterProperty(resID);
        }

        public Monster() : base()
        { }

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

            mTargetSystem = new TargetHateStatus() { owner = this };

            // action group
            ActionStatus.ActionGroup = mProperty.ActionGroup;

            // collision
            EnableCollision = true;
            mSearchLayerMask = LayerMask.GetMask("Role", "LocalPlayer");

            // hud
            Transform hud = Helper.Find(UObject, "hud");
            HudBlood = HudMgr.Instance.GetHudBlood(hud, EBloodType.EBT_Red);
            HudText = HudMgr.Instance.GetHudText(hud);

            // custom variable
            // TO CLine: using entity excel data for your game.
            // here just for test.

            PropertyContext.AddProperty<float>(PropertyName.sSearchDist, ETAnyType.Float, 10f);
            PropertyContext.AddProperty<float>(PropertyName.sAttackDist, ETAnyType.Float, 1.5f);
            PropertyContext.AddProperty(PropertyName.sVelocityY, ETAnyType.Float);

            PropertyContext.AddProperty(PropertyName.sAI, ETAnyType.String);
            PropertyContext.AddProperty(PropertyName.sHitAction, ETAnyType.String);

            // AI
            mAI = new AIStatus();
            mAI.Init(this, mProperty.StartupAI, mProperty.AISwitch);
        }

        public override void Update(float fTick)
        {
            base.Update(fTick);

            mTargetSystem.Update(fTick);

            mAI.Update(fTick);
            ActionStatus.Update(fTick);
        }

        

        public override void OnActionStart(Action action)
        {
            base.OnActionStart(action);

            Helper.SetAny<string>(PropertyContext.GetProperty(PropertyName.sHitAction), "");

            if (ActionStatus.ActiveAction.ActionStatus == EActionState.Dead)
            {
                ActionStatus.SetVelocity(Vector3.zero);
            }

            mAI.OnActionStart(action);
        }

        public override void OnActionEnd(Action action, bool interrupt)
        {
            base.OnActionEnd(action, interrupt);

            mAI.OnActionEnd(action, interrupt);

            ActionStatus.SetVelocity(Vector3.zero);
        }

        public override void OnActionChanging(Action oldAction, Action newAction, bool interrupt)
        {
            base.OnActionChanging(oldAction, newAction, interrupt);

            mAI.OnActionChanging(oldAction, newAction, interrupt);
        }

        public override void OnActionFinish(Action action)
        {
            base.OnActionFinish(action);

            mAI.OnActionFinish(action);
        }

        public override void UpdateAttributes(byte[] buf)
        {
            base.UpdateAttributes(buf);

            if (buf == null)
            {
                // TO CLine: update attr from entity
                Attrib.BaseHP = 100;
                Attrib.CurHP = 100;
                Attrib.BaseSpeed = 2.5f;
                Attrib.CurSpeed = 2.5f;
            }
            else
            {
                // TO CLine: update attr from server
            }
        }

        public override NumericalType GetAttribute(EAttributeType type)
        {
            double num = 0;
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
                EHudType hudType = EHudType.EHT_MonsterBehurt_Normal;
                switch (result)
                {
                    case ECombatResult.ECR_Critical:
                        hudType = EHudType.EHT_MonsterBehurt_Critical;
                        break;
                    case ECombatResult.ECR_SkillNormal:
                        hudType = EHudType.EHT_MonsterBehurt_Normal_Skill;
                        break;
                    case ECombatResult.ECR_SkillCritical:
                        hudType = EHudType.EHT_MonsterBehurt_Critical_SKill;
                        break;
                }

                HudText.ShowText(hudType, hp, 0);
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
