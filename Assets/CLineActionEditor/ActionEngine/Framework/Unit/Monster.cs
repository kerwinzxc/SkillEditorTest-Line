/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\Monster.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-3      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
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
        protected NavMeshAgent mNavAgent = null;

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

        public bool EnableAI
        { get; set; }

        public Monster() : base()
        { }

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
            mSearchLayerMask = LayerMask.GetMask("Role", "LocalPlayer");

            // agent
            mNavAgent = UObject.GetComponent<NavMeshAgent>();
            mNavAgent.enabled = false;

            // hud
            Transform hud = Helper.Find(UObject, "hud");
            HudBlood = HudMgr.Instance.GetHudBlood(hud, EBloodType.EBT_Red);
            HudText = HudMgr.Instance.GetHudText(hud);

            // custom variable
            // TO CLine: using entity excel data for your game.
            // here just for test.
            CustomPropertyHash.Add(CustomProperty.sAttackDist, new CustomProperty(CustomProperty.sAttackDist, EProperyType.EPT_Float, 1.5f));
            CustomPropertyHash.Add(CustomProperty.sSearchDist, new CustomProperty(CustomProperty.sSearchDist, EProperyType.EPT_Float, 10f));
            CustomPropertyHash.Add(CustomProperty.sAI, new CustomProperty(CustomProperty.sAI, EProperyType.EPT_String, ""));
            CustomPropertyHash.Add(CustomProperty.sHitAction, new CustomProperty(CustomProperty.sHitAction, EProperyType.EPT_String, ""));
            CustomPropertyHash.Add(CustomProperty.sVelocityY, new CustomProperty(CustomProperty.sVelocityY, EProperyType.EPT_Float, 0f));

            // AI
            EnableAI = true;
            mAI = new AIStatus();
            mAI.Init(this, mProperty.StartupAI, mProperty.AISwitch);
        }

        public override void Update(float fTick)
        {
            base.Update(fTick);

            if (EnableAI)
            {
                mAI.Update(fTick);
            }

            ActionStatus.Update(fTick);

            if (IsDead)
            {
                return;
            }

            if (Target == null)
            {
                float radius = (float)CustomPropertyHash[CustomProperty.sSearchDist].Value;
                Unit[] units = Helper.Search(Position, radius, mSearchLayerMask, new Transform[] { UObject }, Camp, true, Vector3.zero);
                if (units.Length > 0)
                {
                    Target = units[0];
                }
            }

            if (Target != null && 
                ActionStatus.ActiveAction.ActionStatus == EActionState.Move && 
                ActionStatus.CanMove && mNavAgent.enabled)
            {
                StartNavigation(Target.Position);
            }
            else if (ActionStatus.ActiveAction.ActionStatus == EActionState.Move && !ActionStatus.CanMove && mNavAgent.enabled)
            {
                StopNavigation();
            }
        }

        public void StartNavigation(Vector3 pos)
        {
            if (mNavAgent != null && UObject.gameObject.activeInHierarchy)
            {
                mNavAgent.speed = (float)GetAttribute(EAttributeType.EAT_CurMoveSpeed);
                mNavAgent.destination = pos;
            }
            else
            {
                SetPosition(pos);
            }
        }

        public void StopNavigation()
        {
            if (mNavAgent.enabled)
            {
                mNavAgent.isStopped = true;
                mNavAgent.ResetPath();
            }
        }

        public override void OnActionStart(Action action)
        {
            base.OnActionStart(action);

            CustomPropertyHash[CustomProperty.sHitAction].Value = "";

            mAI.OnActionStart(action);

            if (ActionStatus.ActiveAction.ActionStatus == EActionState.Move)
            {
                mNavAgent.enabled = true;
            }
            else
            {
                mNavAgent.enabled = false;
            }

            if (ActionStatus.ActiveAction.ActionStatus == EActionState.Dead)
            {
                ActionStatus.SetVelocity(Vector3.zero);
            }
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
