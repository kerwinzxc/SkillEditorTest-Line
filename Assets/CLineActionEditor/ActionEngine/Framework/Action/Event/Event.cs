/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\Event.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using System;

    public sealed class Event
    {
        private int mTriggerTime;
        private EEventType mEventType;
        private EventData mEventData;
        private string mActorID;

        [EditorProperty("事件触发时间", EditorPropertyType.EEPT_Int)]
        public int TriggerTime
        {
            get { return mTriggerTime; }
            set { mTriggerTime = value; }
        }

        [EditorProperty("事件类型", EditorPropertyType.EEPT_Enum)]
        public EEventType EventType
        {
            get { return mEventType; }
            set
            {
                if (mEventType != value)
                {
                    mEventType = value;
                    switch (mEventType)
                    {
                        case EEventType.EET_None:
                            mEventData = null;
                            break;
                        case EEventType.EET_PlayAnim:
                            mEventData = new EventPlayAnim();
                            break;
                        case EEventType.EET_PlayEffect:
                            mEventData = new EventPlayEffect();
                            break;
                        case EEventType.EET_PlaySound:
                            mEventData = new EventPlaySound();
                            break;
                        case EEventType.EET_CameraShake:
                            mEventData = new EventCameraShake();
                            break;
                        case EEventType.EET_CameraEffect:
                            mEventData = new EventCameraEffect();
                            break;
                        case EEventType.EET_ActionBreak:
                            mEventData = new EventActionBreak();
                            break;
                        case EEventType.EET_SendMessage:
                            mEventData = new EventSendMessage();
                            break;
                        case EEventType.EET_Move:
                            mEventData = new EventMove();
                            break;
                        case EEventType.EET_PlayAnimatorAnim:
                            mEventData = new EventPlayAnimatorAnim();
                            break;
                        case EEventType.EET_ShowTrail:
                            mEventData = new EventShowTrail();
                            break;
                        case EEventType.EET_HideTrail:
                            mEventData = new EventHideTrail();
                            break;
                        case EEventType.EET_WeaponAttack:
                            mEventData = new EventWeaponAttack();
                            break;
                        case EEventType.EET_WeaponIdle:
                            mEventData = new EventWeaponIdle();
                            break;
                        case EEventType.EET_SetCustomProperty:
                            mEventData = new EventSetCustomProperty();
                            break;
                        case EEventType.EET_StopEffect:
                            mEventData = new EventStopEffect();
                            break;
                        case EEventType.EET_DelUnit:
                            mEventData = new EventDelUnit();
                            break;
                    }
                }
            }
        }

        public string ActorID
        {
            get { return mActorID; }
            set { mActorID = value; }
        }

        public EventData EventData
        {
            get { return mEventData; }
        }

        public void Deserialize(JsonData jd)
        {
            mTriggerTime = JsonHelper.ReadInt(jd["TriggerTime"]);
            EventType = JsonHelper.ReadEnum<EEventType>(jd["EventType"]);
            mEventData.Deserialize(jd["EventData"]);

            mActorID = Convert.ToString(jd["ActorID"]);

        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "TriggerTime", mTriggerTime);
            JsonHelper.WriteProperty(ref writer, "EventType", mEventType.ToString());

            writer.WritePropertyName("EventData");
            writer.WriteObjectStart();
            writer = mEventData.Serialize(writer);
            writer.WriteObjectEnd();

            JsonHelper.WriteProperty(ref writer, "ActorID", mActorID);
            writer.WriteObjectEnd();

            return writer;
        }

        public Event Clone()
        {
            Event evt = new Event();
            evt.mTriggerTime = this.mTriggerTime;
            evt.mEventType = this.mEventType;
            evt.mActorID = this.mActorID;

            return evt;
        }
    }

}
