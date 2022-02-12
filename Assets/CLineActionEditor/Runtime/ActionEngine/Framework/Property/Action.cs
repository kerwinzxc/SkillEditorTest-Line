/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\Action.cs
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

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using System;
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
        [SerializeField] private string mID;
        [SerializeField] private string mName;
        [SerializeField] private string mDesc;
        [SerializeField] private bool mCanMove;
        [SerializeField] private bool mCanRotate;
        [SerializeField] private bool mCanHurt;
        [SerializeField] private bool mCanHit = true;
        [SerializeField] private bool mIgnoreGravity;
        [SerializeField] private bool mFaceTarget;
        [SerializeField] private bool mIsGod;
        [SerializeField] private int mLevel;
        [SerializeField] private EActionState mActionStatus;
        [SerializeField] private int mTotalTime;//ms
        [SerializeField] private string mDefaultAction;

        [SerializeReference] private List<ActionEvent> mEventList = new List<ActionEvent>();
        [SerializeReference] private List<ActionEvent> mInterruptList = new List<ActionEvent>();
        [SerializeReference] private List<ActionEvent> mAttackList = new List<ActionEvent>();

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

        public List<ActionEvent> EventList
        {
            get { return mEventList; }
            set { mEventList = value; }
        }
        public List<ActionEvent> InterruptList
        {
            get { return mInterruptList; }
            set { mInterruptList = value; }
        }
        public List<ActionEvent> AttackList
        {
            get { return mAttackList; }
            set { mAttackList = value; }
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

            DeserializeEvent(jd["Events"], (ae) => { EventList.Add(ae); });
            DeserializeEvent(jd["Interrupts"], (ae) => { InterruptList.Add(ae); });
            DeserializeEvent(jd["AttackDefs"], (ae) => { AttackList.Add(ae); });
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

        public void ForceSort()
        {
            EventList.Sort((l, r) =>
            {
                return l.TrackIndex < r.TrackIndex ? -1 : (l.TrackIndex == r.TrackIndex ? 0 : 1);
            });
            AttackList.Sort((l, r) =>
            {
                return l.TrackIndex < r.TrackIndex ? -1 : (l.TrackIndex == r.TrackIndex ? 0 : 1);
            });
            InterruptList.Sort((l, r) =>
            {
                return l.TrackIndex < r.TrackIndex ? -1 : (l.TrackIndex == r.TrackIndex ? 0 : 1);
            });
        }

        private void SerializeEvent(ref JsonWriter writer)
        {
            EventList.Sort((l, r) =>
            {
                return l.TriggerTime < r.TriggerTime ? -1 : (l.TriggerTime == r.TriggerTime ? 0 : 1);
            });
            writer.WritePropertyName("Events");
            writer.WriteArrayStart();
            using (List<ActionEvent>.Enumerator itr = EventList.GetEnumerator())
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
            AttackList.Sort((l, r) =>
            {
                return l.TriggerTime < r.TriggerTime ? -1 : (l.TriggerTime == r.TriggerTime ? 0 : 1);
            });
            writer.WritePropertyName("AttackDefs");
            writer.WriteArrayStart();
            using (List<ActionEvent>.Enumerator itr = AttackList.GetEnumerator())
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
            InterruptList.Sort((l, r) =>
            {
                return l.TrackIndex < r.TrackIndex ? -1 : (l.TrackIndex == r.TrackIndex ? 0 : 1);
            });

            writer.WritePropertyName("Interrupts");
            writer.WriteArrayStart();
            using (List<ActionEvent>.Enumerator itr = InterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer = itr.Current.Serialize(writer);
                }
            }
            writer.WriteArrayEnd();
        }

        private void DeserializeEvent(JsonData jd, System.Action<ActionEvent> callback)
        {
            for (int i=0; i<jd.Count; ++i)
            {
                ActionEvent ae = new ActionEvent();
                ae.Deserialize(jd[i]);
                callback?.Invoke(ae);
            }
        }


    }

}
