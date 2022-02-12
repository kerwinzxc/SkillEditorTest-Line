/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\ActionInterrupt.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : TO CLine: using condition pattern at next version!!!
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-14      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;
    using LitJson;
    using System.Collections.Generic;

    public sealed class ActionInterrupt : IEventData, IProperty
    {
        [SerializeField] private string mInterruptName;
        [SerializeField] private string mActionID;
        [SerializeField] private bool mEnabled;
        [SerializeField] private bool mCheckAllCondition;
        [SerializeField] private float mCrossFadeDuration;
        [SerializeReference] private List<InterruptCondition> mConditionList = new List<InterruptCondition>();

        #region property
        [EditorProperty("打断名称", EditorPropertyType.EEPT_String)]
        public string InterruptName
        {
            get { return mInterruptName; }
            set { mInterruptName = value; }
        }
        [EditorProperty("Action ID", EditorPropertyType.EEPT_String, Edit = false)]
        public string ActionID
        {
            get { return mActionID; }
            set { mActionID = value; }
        }
        [EditorProperty("打断开启", EditorPropertyType.EEPT_Bool)]
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }
        [EditorProperty("检查所有条件", EditorPropertyType.EEPT_Bool)]
        public bool CheckAllCondition
        {
            get { return mCheckAllCondition; }
            set { mCheckAllCondition = value; }
        }
        [EditorProperty("动画过渡时间", EditorPropertyType.EEPT_Float)]
        public float CrossFadeDuration
        {
            get { return mCrossFadeDuration; }
            set { mCrossFadeDuration = value; }
        }
        #endregion

        public string DebugName
        {
            get { return mInterruptName; }
        }
        public List<InterruptCondition> ConditionList
        {
            get { return mConditionList; }
            set { mConditionList = value; }
        }

        public void Deserialize(JsonData jd)
        {
            mInterruptName = JsonHelper.ReadString(jd["InterruptName"]);
            mActionID = JsonHelper.ReadString(jd["ActionID"]);
            mEnabled = JsonHelper.ReadBool(jd["Enabled"]);
            mCheckAllCondition = JsonHelper.ReadBool(jd["CheckAllCondition"]);
            mCrossFadeDuration = JsonHelper.ReadFloat(jd["CrossFadeDuration"]);

            JsonData jdConditions = jd["Conditions"];
            for (int i = 0; i < jdConditions.Count; ++i)
            {
                EInterruptType eit = JsonHelper.ReadEnum<EInterruptType>(jdConditions[i]["InterruptType"]);
                InterruptCondition cond = Add(eit);
                IProperty p = cond as IProperty;
                p.Deserialize(jdConditions[i]);
            }
        }
        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "InterruptName", mInterruptName);
            JsonHelper.WriteProperty(ref writer, "ActionID", mActionID);
            JsonHelper.WriteProperty(ref writer, "Enabled", mEnabled);
            JsonHelper.WriteProperty(ref writer, "CheckAllCondition", mCheckAllCondition);
            JsonHelper.WriteProperty(ref writer, "CrossFadeDuration", mCrossFadeDuration);

            writer.WritePropertyName("Conditions");
            writer.WriteArrayStart();
            using (List<InterruptCondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer.WriteObjectStart();
                    JsonHelper.WriteProperty(ref writer, "InterruptType", itr.Current.InterruptType.ToString());
                    IProperty p = itr.Current as IProperty;
                    writer = p.Serialize(writer);
                    writer.WriteObjectEnd();
                }
            }
            writer.WriteArrayEnd();

            return writer;
        }

        public IEventData Clone()
        {
            ActionInterrupt interrupt = new ActionInterrupt();
            interrupt.InterruptName = this.InterruptName;
            interrupt.ActionID = this.ActionID;
            interrupt.Enabled = this.Enabled;
            interrupt.CheckAllCondition = this.CheckAllCondition;
            interrupt.CrossFadeDuration = this.CrossFadeDuration;

            using (List<InterruptCondition>.Enumerator itr = this.ConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    interrupt.ConditionList.Add(itr.Current.Clone());
                }
            }

            return interrupt;
        }

        public InterruptCondition Add(EInterruptType t)
        {
            InterruptCondition cond = null;
            switch (t)
            {
                case EInterruptType.EIT_CheckCustomPropertyBool:
                    cond = new CheckCustomPropertyBool();
                    break;
                case EInterruptType.EIT_CheckCustomPropertyInt:
                    cond = new CheckCustomPropertyInt();
                    break;
                case EInterruptType.EIT_CheckCustomPropertyFloat:
                    cond = new CheckCustomPropertyFloat();
                    break;
                case EInterruptType.EIT_CheckCustomPropertyString:
                    cond = new CheckCustomPropertyString();
                    break;
                case EInterruptType.EIT_CheckCustomPropertyCount:
                    cond = new CheckCustomPropertyCount();
                    break;
                case EInterruptType.EIT_CheckDead:
                    cond = new CheckDead();
                    break;
                case EInterruptType.EIT_CheckTargetNotNull:
                    cond = new CheckTargetNotNull();
                    break;
                case EInterruptType.EIT_CheckTargetIsNull:
                    cond = new CheckTargetIsNull();
                    break;
                case EInterruptType.EIT_CheckOnGround:
                    cond = new CheckOnGround();
                    break;
                case EInterruptType.EIT_CheckActionState:
                    cond = new CheckActionState();
                    break;
                case EInterruptType.EIT_CheckWeaponType:
                    cond = new CheckWeaponType();
                    break;
                case EInterruptType.EIT_CheckVelocityY:
                    cond = new CheckVelocityY();
                    break;
                case EInterruptType.EIT_CheckInputSkillPosition:
                    cond = new CheckInputSkillPosition();
                    break;
            }

            mConditionList.Add(cond);

            return cond;
        }

        public void Del(InterruptCondition cond)
        {
            mConditionList.Remove(cond);
        }

        public void DelAll()
        {
            mConditionList.Clear();
        }

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_Interrupt; }
        }

        public void Enter(Unit unit)
        {

        }
        public void Update(Unit unit, int deltaTime)
        {

        }
        public void Exit(Unit unit)
        {

        }

        public void Execute(Unit unit)
        {
        }

    }

}
