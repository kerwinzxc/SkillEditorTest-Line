/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventCameraShake.cs
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

    public sealed class EventCameraShake : IEventData, IProperty
    {
        [SerializeField] private float mDelayTime = 0;
        [SerializeField] private float mDuration = 0.2f;
        [SerializeField] private float mIntensity = 0.7f;
        [SerializeField] private int mVibrato = 3;
        [SerializeField] private bool mDisableX = true;
        [SerializeField] private bool mDisableY = false;
        [SerializeField] private bool mAttenuation = true;

        #region property
        [EditorProperty("延迟振屏(ms)", EditorPropertyType.EEPT_Float)]
        public float DelayTime
        {
            get { return mDelayTime; }
            set { mDelayTime = value; }
        }
        [EditorProperty("振屏时间(s)", EditorPropertyType.EEPT_Float)]
        public float Duration
        {
            get { return mDuration; }
            set { mDuration = value; }
        }
        [EditorProperty("振幅", EditorPropertyType.EEPT_Float)]
        public float Intensity
        {
            get { return mIntensity; }
            set { mIntensity = value; }
        }
        [EditorProperty("振屏次数", EditorPropertyType.EEPT_Int)]
        public int Vibrato
        {
            get { return mVibrato; }
            set { mVibrato = value; }
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
        [EditorProperty("是否衰减", EditorPropertyType.EEPT_Bool)]
        public bool Attenuation
        {
            get { return mAttenuation; }
            set { mAttenuation = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_CameraShake; }
        }

        public string DebugName
        {
            get { return GetType().ToString(); }
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
            if (DelayTime > 0)
            {
                TimerMgr.Instance.AddTimer(mDelayTime, false, 0, (paramList) =>
                {
                    CameraMgr.Instance.AddShakeEffect(mDuration, mIntensity, mVibrato, mDisableX, mDisableY, mAttenuation);
                });
            }
            else
            {
                CameraMgr.Instance.AddShakeEffect(mDuration, mIntensity, mVibrato, mDisableX, mDisableY, mAttenuation);
            }
        }

        public void Deserialize(JsonData jd)
        {
            mDelayTime = JsonHelper.ReadFloat(jd["DelayTime"]);
            mDuration = JsonHelper.ReadFloat(jd["Duration"]);
            mIntensity = JsonHelper.ReadFloat(jd["Intensity"]);
            mVibrato = JsonHelper.ReadInt(jd["Vibrato"]);
            mDisableX = JsonHelper.ReadBool(jd["DisableX"]);
            mDisableY = JsonHelper.ReadBool(jd["DisableY"]);
            mAttenuation = JsonHelper.ReadBool(jd["Attenuation"]);
    }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "DelayTime", mDelayTime);
            JsonHelper.WriteProperty(ref writer, "Duration", mDuration);
            JsonHelper.WriteProperty(ref writer, "Intensity", mIntensity);
            JsonHelper.WriteProperty(ref writer, "Vibrato", mVibrato);
            JsonHelper.WriteProperty(ref writer, "DisableX", mDisableX);
            JsonHelper.WriteProperty(ref writer, "DisableY", mDisableY);
            JsonHelper.WriteProperty(ref writer, "Attenuation", mAttenuation);

            return writer;
        }

        private void DoCameraShake(params object[] param)
        {
            
        }

        public IEventData Clone()
        {
            EventCameraShake evt = new EventCameraShake();
            evt.mDelayTime = this.mDelayTime;
            evt.mDuration = this.mDuration;
            evt.mIntensity = this.mIntensity;
            evt.mDisableX = this.mDisableX;
            evt.mDisableY = this.mDisableY;
            evt.mAttenuation = this.mAttenuation;

            return evt;
        }
    }

}
