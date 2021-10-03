/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\Action.cs
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
    using LitJson;
    using System.Collections.Generic;

    public enum EAnimType
    {
        EAT_SetBool = 0,
        EAT_SetFloat = 1,
        EAT_SetInt = 2,
        EAT_SetTrigger = 3,
        EAT_Force = 4,
    }

    public enum EActionState
    {
        Idle = 0,
        Move = 1,
        Attack = 2,
        Skill = 3,
        Dead = 4,
        Relive = 5,
        BeHit = 6,
        Roll = 7,
        Jump = 8
    }

    // motion: M  skill: S
    public sealed class Action : IProperty
    {
        private string mID;
        private string mName;
        private string mDesc;
        private bool mCanMove;
        private bool mCanRotate;
        private bool mCanHurt;
        private bool mCanHit = true;
        private bool mIgnoreGravity;
        private bool mFaceTarget;
        private bool mIsGod;
        private int mLevel;
        private EActionState mActionStatus;
        private int mTotalTime;//ms
        private string mDefaultAction;
        
        private List<Event> mEventList = new List<Event>();
        private List<ActionInterrupt> mInterruptList = new List<ActionInterrupt>();
        private List<ActionAttackDef> mAttackDefList = new List<ActionAttackDef>();

        #region
        [EditorProperty("Action ID", EditorPropertyType.EEPT_String)]
        public string ID
        {
            set
            {
                mID = value;
            }
            get
            {
                return mID;
            }
        }
        [EditorProperty("Action 名称", EditorPropertyType.EEPT_String)]
        public string Name
        {
            set
            {
                mName = value;
            }
            get
            {
                return mName;
            }
        }
        [EditorProperty("Action 描述", EditorPropertyType.EEPT_String)]
        public string Desc
        {
            set
            {
                mDesc = value;
            }
            get
            {
                return mDesc;
            }
        }
        [EditorProperty("允许移动", EditorPropertyType.EEPT_Bool)]
        public bool CanMove
        {
            set
            {
                mCanMove = value;
            }
            get
            {
                return mCanMove;
            }
        }
        [EditorProperty("允许旋转", EditorPropertyType.EEPT_Bool)]
        public bool CanRotate
        {
            set
            {
                mCanRotate = value;
            }
            get
            {
                return mCanRotate;
            }
        }
        [EditorProperty("允许受伤", EditorPropertyType.EEPT_Bool)]
        public bool CanHurt
        {
            get { return mCanHurt; }
            set { mCanHurt = value; }
        }
        [EditorProperty("允许受击", EditorPropertyType.EEPT_Bool)]
        public bool CanHit
        {
            get { return mCanHit; }
            set { mCanHit = value; }
        }
        [EditorProperty("忽略重力", EditorPropertyType.EEPT_Bool)]
        public bool IgnoreGravity
        {
            get { return mIgnoreGravity; }
            set { mIgnoreGravity = value; }
        }
        [EditorProperty("面向目标", EditorPropertyType.EEPT_Bool)]
        public bool FaceTarget
        {
            get { return mFaceTarget; }
            set { mFaceTarget = value; }
        }
        [EditorProperty("无敌", EditorPropertyType.EEPT_Bool)]
        public bool IsGod
        {
            get { return mIsGod; }
            set { mIsGod = value; }
        }
        [EditorProperty("Action 等级", EditorPropertyType.EEPT_Int)]
        public int Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }
        [EditorProperty("状态", EditorPropertyType.EEPT_Enum)]
        public EActionState ActionStatus
        {
            get { return mActionStatus; }
            set { mActionStatus = value; }
        }
        [EditorProperty("总时间(ms)", EditorPropertyType.EEPT_Int)]
        public int TotalTime
        {
            get { return mTotalTime; }
            set { mTotalTime = value; }
        }
        [EditorProperty("默认Action", EditorPropertyType.EEPT_String)]
        public string DefaultAction
        {
            get { return mDefaultAction; }
            set { mDefaultAction = value; }
        }

        public List<Event> EventList
        {
            get { return mEventList; }
            set { mEventList = value; }
        }
        public List<ActionInterrupt> InterruptList
        {
            get { return mInterruptList; }
            set { mInterruptList = value; }
        }
        public List<ActionAttackDef> AttackList
        {
            get { return mAttackDefList; }
            set { mAttackDefList = value; }
        }
        #endregion

        public string DebugName
        {
            get { return mName; }
        }

        public void Deserialize(JsonData jd)
        {
            mID = JsonHelper.ReadString(jd["ID"]);
            mName = JsonHelper.ReadString(jd["Name"]);
            mCanMove = JsonHelper.ReadBool(jd["CanMove"]);
            mCanRotate = JsonHelper.ReadBool(jd["CanRotate"]);
            mCanHurt = JsonHelper.ReadBool(jd["CanHurt"]);
            mCanHit = JsonHelper.ReadBool(jd["CanHit"]);
            mIgnoreGravity = JsonHelper.ReadBool(jd["IgnoreGravity"]);
            mFaceTarget = JsonHelper.ReadBool(jd["FaceTarget"]);
            mIsGod = JsonHelper.ReadBool(jd["IsGod"]);
            mLevel = JsonHelper.ReadInt(jd["Level"]);
            mActionStatus = JsonHelper.ReadEnum<EActionState>(jd["ActionStatus"]);
            mTotalTime = JsonHelper.ReadInt(jd["TotalTime"]);
            mDefaultAction = JsonHelper.ReadString(jd["DefaultAction"]);

            DeserializeEvent(jd["Events"]);
            DeserializeInterrupt(jd["Interrupts"]);
            DeserializeAttackDef(jd["AttackDefs"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "ID", mID);
            JsonHelper.WriteProperty(ref writer, "Name", mName);
            JsonHelper.WriteProperty(ref writer, "CanMove", mCanMove);
            JsonHelper.WriteProperty(ref writer, "CanRotate", mCanRotate);
            JsonHelper.WriteProperty(ref writer, "CanHurt", mCanHurt);
            JsonHelper.WriteProperty(ref writer, "CanHit", mCanHit);
            JsonHelper.WriteProperty(ref writer, "IgnoreGravity", mIgnoreGravity);
            JsonHelper.WriteProperty(ref writer, "FaceTarget", mFaceTarget);
            JsonHelper.WriteProperty(ref writer, "IsGod", mIsGod);
            JsonHelper.WriteProperty(ref writer, "Level", mLevel);
            JsonHelper.WriteProperty(ref writer, "ActionStatus", mActionStatus.ToString());
            JsonHelper.WriteProperty(ref writer, "TotalTime", mTotalTime);
            JsonHelper.WriteProperty(ref writer, "DefaultAction", mDefaultAction);

            SerializeEvent(ref writer);
            SerializeInterrupt(ref writer);
            SerializeAttackDef(ref writer);

            writer.WriteObjectEnd();

            return writer;
        }

        private void SerializeEvent(ref JsonWriter writer)
        {
            for (int i = 0; i < mEventList.Count; ++i)
            {
                for (int j = i; j < mEventList.Count; ++j)
                {
                    if (mEventList[j].TriggerTime < mEventList[i].TriggerTime)
                    {
                        Event temp = mEventList[j];
                        mEventList[j] = mEventList[i];
                        mEventList[i] = temp;
                    }
                }
            }
            writer.WritePropertyName("Events");
            writer.WriteArrayStart();
            using (List<Event>.Enumerator itr = mEventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer = itr.Current.Serialize(writer);
                }
            }
            writer.WriteArrayEnd();
        }

        private void SerializeInterrupt(ref JsonWriter writer)
        {
            writer.WritePropertyName("Interrupts");
            writer.WriteArrayStart();
            using (List<ActionInterrupt>.Enumerator itr = mInterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer = itr.Current.Serialize(writer);
                }
            }
            writer.WriteArrayEnd();
        }

        private void SerializeAttackDef(ref JsonWriter writer)
        {
            for (int i = 0; i < mAttackDefList.Count; ++i)
            {
                for (int j = i; j < mAttackDefList.Count; ++j)
                {
                    if (mAttackDefList[j].TriggerTime < mAttackDefList[i].TriggerTime)
                    {
                        ActionAttackDef temp = mAttackDefList[j];
                        mAttackDefList[j] = mAttackDefList[i];
                        mAttackDefList[i] = temp;
                    }
                }
            }
            writer.WritePropertyName("AttackDefs");
            writer.WriteArrayStart();
            using (List<ActionAttackDef>.Enumerator itr = mAttackDefList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer = itr.Current.Serialize(writer);
                }
            }
            writer.WriteArrayEnd();
        }

        private void DeserializeEvent(JsonData jd)
        {
            for (int i = 0; i < jd.Count; ++i)
            {
                Event ev = new Event();
                ev.Deserialize(jd[i]);
                mEventList.Add(ev);
            }
        }

        private void DeserializeInterrupt(JsonData jd)
        {
            for (int i = 0; i < jd.Count; ++i)
            {
                ActionInterrupt aci = new ActionInterrupt();
                aci.Deserialize(jd[i]);
                mInterruptList.Add(aci);
            }
        }

        private void DeserializeAttackDef(JsonData jd)
        {
            for (int i = 0; i < jd.Count; ++i)
            {
                ActionAttackDef atd = new ActionAttackDef();
                atd.Deserialize(jd[i]);
                mAttackDefList.Add(atd);
            }
        }

    }

}
