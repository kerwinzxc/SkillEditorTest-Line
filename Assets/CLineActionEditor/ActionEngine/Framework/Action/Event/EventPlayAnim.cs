/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventPlayAnim.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-19      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class EventPlayAnim : EventData
    {
        private string mAnimName;
        private EAnimType mAnimType = EAnimType.EAT_Force;
        private string mTriggerName;
        private string mTriggerValue;


        #region property
        [EditorProperty("动作", EditorPropertyType.EEPT_AnimatorStateToString)]
        public string AnimName
        {
            get { return mAnimName; }
            set { mAnimName = value; }
        }

        [EditorProperty("动作触发类型", EditorPropertyType.EEPT_Enum)]
        public EAnimType AnimType
        {
            get { return mAnimType; }
            set { mAnimType = value; }
        }

        [EditorProperty("动作触发条件", EditorPropertyType.EEPT_AnimatorParamToString)]
        public string TriggerName
        {
            get { return mTriggerName; }
            set { mTriggerName = value; }
        }

        [EditorProperty("动作触发值", EditorPropertyType.EEPT_String)]
        public string TriggerValue
        {
            get { return mTriggerValue; }
            set { mTriggerValue = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_PlayAnim; }
        }

        public void Execute(Unit unit)
        {
            if (mAnimType == EAnimType.EAT_Force)
                unit.PlayAnimation(mAnimType, mAnimName, TriggerValue);
            else
                unit.PlayAnimation(mAnimType, TriggerName, TriggerValue);
        }

        public void Deserialize(JsonData jd)
        {
            mAnimName = JsonHelper.ReadString(jd["AnimName"]);
            mAnimType = JsonHelper.ReadEnum<EAnimType>(jd["AnimType"]);
            mTriggerName = JsonHelper.ReadString(jd["TriggerName"]);
            mTriggerValue = JsonHelper.ReadString(jd["TriggerValue"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "AnimName", mAnimName);
            JsonHelper.WriteProperty(ref writer, "AnimType", mAnimType.ToString());
            JsonHelper.WriteProperty(ref writer, "TriggerName", mTriggerName);
            JsonHelper.WriteProperty(ref writer, "TriggerValue", mTriggerValue);

            return writer;
        }

        public EventData Clone()
        {
            EventPlayAnim evt = new EventPlayAnim();
            evt.mAnimName = this.mAnimName;
            evt.mAnimType = this.mAnimType;
            evt.mTriggerName = this.mTriggerName;
            evt.mTriggerValue = this.mTriggerValue;

            return evt;
        }
    }

}
