/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventCameraShake.cs
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

    public sealed class EventCameraShake : EventData
    {
        private float mDuration = 0.2f;
        private float mShakeIntensity = 0.7f;
        private bool mDisableX = true;
        private bool mDisableY = false;
        private bool mIsDelay = false;
        private float mDelayTime;

        #region property
        [EditorProperty("振动时长", EditorPropertyType.EEPT_Float)]
        public float Duration
        {
            get { return mDuration; }
            set { mDuration = value; }
        }
        [EditorProperty("振幅", EditorPropertyType.EEPT_Float)]
        public float ShakeIntensity
        {
            get { return mShakeIntensity; }
            set { mShakeIntensity = value; }
        }
        [EditorProperty("不允许X方向振动", EditorPropertyType.EEPT_Bool)]
        public bool DisableX
        {
            get { return mDisableX; }
            set { mDisableX = value; }
        }
        [EditorProperty("不允许Y方向振动", EditorPropertyType.EEPT_Bool)]
        public bool DisableY
        {
            get { return mDisableY; }
            set { mDisableY = value; }
        }
        [EditorProperty("是否延迟生效", EditorPropertyType.EEPT_Bool)]
        public bool IsDelay
        {
            get { return mIsDelay; }
            set { mIsDelay = value; }
        }
        [EditorProperty("延迟时间", EditorPropertyType.EEPT_Float)]
        public float DelayTime
        {
            get { return mDelayTime; }
            set { mDelayTime = value; }
        }
        #endregion


        public EEventType EventType
        {
            get { return EEventType.EET_CameraShake; }
        }

        public void Execute(Unit unit)
        {
            if (!IsDelay)
                DoCameraShake();
            else
                TimerMgr.Instance.AddTimer(mDelayTime, false, 0, DoCameraShake);
        }

        public void Deserialize(JsonData jd)
        {
            mDuration = JsonHelper.ReadFloat(jd["Duration"]);
            mShakeIntensity = JsonHelper.ReadFloat(jd["ShakeIntensity"]);
            mDisableX = JsonHelper.ReadBool(jd["DisableX"]);
            mDisableY = JsonHelper.ReadBool(jd["DisableY"]);
            mIsDelay = JsonHelper.ReadBool(jd["IsDelay"]);
            mDelayTime = JsonHelper.ReadFloat(jd["DelayTime"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Duration", mDuration);
            JsonHelper.WriteProperty(ref writer, "ShakeIntensity", mShakeIntensity);
            JsonHelper.WriteProperty(ref writer, "DisableX", mDisableX);
            JsonHelper.WriteProperty(ref writer, "DisableY", mDisableY);
            JsonHelper.WriteProperty(ref writer, "IsDelay", mIsDelay);
            JsonHelper.WriteProperty(ref writer, "DelayTime", mDelayTime);

            return writer;
        }

        private void DoCameraShake(params object[] param)
        {
            CameraMgr.Instance.AddShakeEffect(mDuration, mShakeIntensity, mDisableX, mDisableY);
        }

        public EventData Clone()
        {
            EventCameraShake evt = new EventCameraShake();
            evt.mDuration = this.mDuration;
            evt.mShakeIntensity = this.mShakeIntensity;
            evt.mDisableX = this.mDisableX;
            evt.mDisableY = this.mDisableY;
            evt.mIsDelay = this.mIsDelay;
            evt.mDelayTime = this.mDelayTime;

            return evt;
        }
    }

}
