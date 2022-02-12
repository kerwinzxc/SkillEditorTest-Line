/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventPlayAnim.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-19      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class EventPlayAnim : IEventData, IProperty
    {
        [SerializeField] private string mAnimName;
        [SerializeField] private EAnimType mAnimType = EAnimType.EAT_Force;
        [SerializeField] private string mTriggerName;
        [SerializeField] private string mTriggerValue;
        [SerializeField] public float AnimCrossFadeDuration = 0;

        #region property
        [EditorProperty("动作", EditorPropertyType.EEPT_AnimatorState)]
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

        [EditorProperty("动作触发条件", EditorPropertyType.EEPT_AnimatorParam)]
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

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_PlayAnim; }
        }

        public string DebugName
        {
            get { return GetType().ToString(); }
        }
        public void Enter(Unit unit)
        {
            if (AnimCrossFadeDuration != 0)
            {
                unit.PlayAnimation(mAnimName, AnimCrossFadeDuration, 0, 0);
                AnimCrossFadeDuration = 0;
            }
            else
            {
                if (mAnimType == EAnimType.EAT_Force)
                    unit.PlayAnimation(mAnimType, mAnimName, TriggerValue);
                else
                    unit.PlayAnimation(mAnimType, TriggerName, TriggerValue);
            }
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

        public IEventData Clone()
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
