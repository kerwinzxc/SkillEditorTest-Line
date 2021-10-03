/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\ActionInterrupt.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : TO CLine: using condition pattern at next version!!!
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-14      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using System;
    using LitJson;
    using System.Collections.Generic;

    [Serializable]
    public sealed class ActionInterrupt : IProperty
    {
        private string mInterruptName;
        private string mActionID;
        private bool mEnabled;
        private int mInterruptTime;//ms
        private bool mCheckAllCondition;

        private string mActorID;

        private List<InterruptCondition> mConditionList = new List<InterruptCondition>();
        
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
        [EditorProperty("打断时间", EditorPropertyType.EEPT_Int)]
        public int InterruptTime
        {
            get { return mInterruptTime; }
            set { mInterruptTime = value; }
        }
        [EditorProperty("检查所有条件", EditorPropertyType.EEPT_Bool)]
        public bool CheckAllCondition
        {
            get { return mCheckAllCondition; }
            set { mCheckAllCondition = value; }
        }
        #endregion

        public string DebugName
        {
            get { return mInterruptName; }
        }
        public string ActorID
        {
            get { return mActorID; }
            set { mActorID = value; }
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
            mInterruptTime = JsonHelper.ReadInt(jd["InterruptTime"]);
            mCheckAllCondition = JsonHelper.ReadBool(jd["CheckAllCondition"]);
            
            mActorID = JsonHelper.ReadString(jd["ActorID"]);

            JsonData jdConditions = jd["Conditions"];
            for (int i = 0; i < jdConditions.Count; ++i)
            {
                EInterruptType eit = JsonHelper.ReadEnum<EInterruptType>(jdConditions[i]["InterruptType"]);
                InterruptCondition cond = Add(eit);
                cond.Deserialize(jdConditions[i]);
            }
        }
        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "InterruptName", mInterruptName);
            JsonHelper.WriteProperty(ref writer, "ActionID", mActionID);
            JsonHelper.WriteProperty(ref writer, "Enabled", mEnabled);
            JsonHelper.WriteProperty(ref writer, "InterruptTime", mInterruptTime);
            JsonHelper.WriteProperty(ref writer, "CheckAllCondition", mCheckAllCondition);

            JsonHelper.WriteProperty(ref writer, "ActorID", mActorID);

            writer.WritePropertyName("Conditions");
            writer.WriteArrayStart();
            using (List<InterruptCondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer.WriteObjectStart();
                    JsonHelper.WriteProperty(ref writer, "InterruptType", itr.Current.InterruptType.ToString());
                    writer = itr.Current.Serialize(writer);
                    writer.WriteObjectEnd();
                }
            }
            writer.WriteArrayEnd();

            writer.WriteObjectEnd();

            return writer;
        }

        public ActionInterrupt Clone()
        {
            ActionInterrupt interrupt = new ActionInterrupt();
            interrupt.InterruptName = this.InterruptName;
            interrupt.ActionID = this.ActionID;
            interrupt.Enabled = this.Enabled;
            interrupt.InterruptTime = this.InterruptTime;
            interrupt.CheckAllCondition = this.CheckAllCondition;
            
            interrupt.ActorID = this.ActorID;

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

    }

}
