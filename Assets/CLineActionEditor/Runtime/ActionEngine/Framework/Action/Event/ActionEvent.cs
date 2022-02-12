/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\ActionEvent.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using System;
    using UnityEngine;

    public enum ETriggerType
    {
        Signal = 0,
        Duration,
    }

    public sealed class ActionEvent : IProperty
    {
        [SerializeField] private string mTrackName;
        [SerializeField] private int mTrackIndex;
        [SerializeField] private int mTriggerTime;
        [SerializeField] private int mDuration;
        [SerializeField] private ETriggerType mTriggerType;
        [SerializeField] private EEventDataType mEventType;
        [SerializeReference] private IEventData mEventData;

        [System.NonSerialized] private int mCurTime = 0;

        #region property
        [EditorProperty("事件触发类型", EditorPropertyType.EEPT_Enum, Edit = false)]
        public ETriggerType TriggerType
        {
            get { return mTriggerType; }
            set { mTriggerType = value; }
        }
        [EditorProperty("事件触发时间", EditorPropertyType.EEPT_Int)]
        public int TriggerTime
        {
            get { return mTriggerTime; }
            set { mTriggerTime = value; }
        }
        [EditorProperty("事件持续时长", EditorPropertyType.EEPT_Int)]
        public int Duration
        {
            get { return mDuration; }
            set { mDuration = value; }
        }
        [EditorProperty("事件类型", EditorPropertyType.EEPT_Enum)]
        public EEventDataType EventType
        {
            get { return mEventType; }
            set
            {
                if (mEventType != value)
                {
                    mEventType = value;
                    switch (mEventType)
                    {
                        case EEventDataType.EET_None:
                            mEventData = null;
                            break;
                        case EEventDataType.EET_PlayAnim:
                            mEventData = new EventPlayAnim();
                            break;
                        case EEventDataType.EET_PlayEffect:
                            mEventData = new EventPlayEffect();
                            break;
                        case EEventDataType.EET_PlaySound:
                            mEventData = new EventPlaySound();
                            break;
                        case EEventDataType.EET_AttackDef:
                            mEventData = new ActionAttackDef();
                            break;
                        case EEventDataType.EET_Interrupt:
                            mEventData = new ActionInterrupt();
                            break;
                        case EEventDataType.EET_CameraShake:
                            mEventData = new EventCameraShake();
                            break;
                        case EEventDataType.EET_CameraEffect:
                            mEventData = new EventCameraEffect();
                            break;
                        case EEventDataType.EET_SendMessage:
                            mEventData = new EventSendMessage();
                            break;
                        case EEventDataType.EET_Move:
                            mEventData = new EventMove();
                            break;
                        case EEventDataType.EET_PlayAnimatorAnim:
                            mEventData = new EventPlayAnimatorAnim();
                            break;
                        case EEventDataType.EET_ShowTrail:
                            mEventData = new EventShowTrail();
                            break;
                        case EEventDataType.EET_HideTrail:
                            mEventData = new EventHideTrail();
                            break;
                        case EEventDataType.EET_WeaponAttack:
                            mEventData = new EventWeaponAttack();
                            break;
                        case EEventDataType.EET_WeaponIdle:
                            mEventData = new EventWeaponIdle();
                            break;
                        case EEventDataType.EET_StopEffect:
                            mEventData = new EventStopEffect();
                            break;
                        case EEventDataType.EET_DelUnit:
                            mEventData = new EventDelUnit();
                            break;
                        case EEventDataType.EET_SetCustomPropertyBool:
                            mEventData = new EventSetCustomPropertyBool();
                            break;
                        case EEventDataType.EET_SetCustomPropertyInt32:
                            mEventData = new EventSetCustomPropertyInt32();
                            break;
                        case EEventDataType.EET_SetCustomPropertyFloat:
                            mEventData = new EventSetCustomPropertyFloat();
                            break;
                        case EEventDataType.EET_SetCustomPropertyString:
                            mEventData = new EventSetCustomPropertyString();
                            break;
                        case EEventDataType.EET_SetCustomPropertyInt32Rand:
                            mEventData = new EventSetCustomPropertyInt32Rand();
                            break;
                        case EEventDataType.EET_EventHoldBoolTag:
                            mEventData = new EventHoldBoolTag();
                            break;
                    }
                }
            }
        }
        #endregion

        public string TrackName
        {
            get { return mTrackName; }
            set { mTrackName = value; }
        }
        public int TrackIndex
        {
            get { return mTrackIndex; }
            set { mTrackIndex = value; }
        }

        public IEventData EventData
        {
            get { return mEventData; }
            set { mEventData = value; }
        }

        public string DebugName
        {
            get { return TrackIndex.ToString(); }
        }

        public void Deserialize(JsonData jd)
        {
            mTrackName = JsonHelper.ReadString(jd["TrackName"]);
            mTrackIndex = JsonHelper.ReadInt(jd["TrackIndex"]);

            mTriggerType = JsonHelper.ReadEnum<ETriggerType>(jd["TriggerType"]);
            mTriggerTime = JsonHelper.ReadInt(jd["TriggerTime"]);
            mDuration = JsonHelper.ReadInt(jd["Duration"]);
            EventType = JsonHelper.ReadEnum<EEventDataType>(jd["EventType"]);

            (mEventData as IProperty).Deserialize(jd["EventData"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "TrackName", mTrackName);
            JsonHelper.WriteProperty(ref writer, "TrackIndex", mTrackIndex);

            JsonHelper.WriteProperty(ref writer, "TriggerType", mTriggerType.ToString());
            JsonHelper.WriteProperty(ref writer, "TriggerTime", mTriggerTime);
            JsonHelper.WriteProperty(ref writer, "Duration", mDuration);
            JsonHelper.WriteProperty(ref writer, "EventType", mEventType.ToString());

            writer.WritePropertyName("EventData");
            writer.WriteObjectStart();
            writer = (mEventData as IProperty).Serialize(writer);
            writer.WriteObjectEnd();

            writer.WriteObjectEnd();

            return writer;
        }

        public ActionEvent Clone()
        {
            ActionEvent evt = new ActionEvent();
            evt.mTrackName = this.mTrackName;
            evt.mTrackIndex = this.TrackIndex;
            evt.mTriggerTime = this.mTriggerTime;
            evt.mDuration = this.mDuration;
            evt.mTriggerType = this.mTriggerType;
            evt.mEventType = this.mEventType;

            return evt;
        }

        public bool CheckTime(int deltaTime)
        {
            mCurTime += deltaTime;
            return mCurTime <= Duration;
        }

        public void Reset()
        {
            mCurTime = 0;
        }
    }

}
