/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventCameraEffect.cs
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

    public sealed class EventCameraEffect : EventData
    {
        private ECameraEffectType mType;

        #region property
        [EditorProperty("特效类型", EditorPropertyType.EEPT_Enum)]
        public ECameraEffectType Type
        {
            get { return mType; }
            set { mType = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_CameraEffect; }
        }

        public void Execute(Unit unit)
        {
            if (mType == ECameraEffectType.ECET_None) return;

            CameraEffect.PlayCameraEffect(mType);
        }

        public void Deserialize(JsonData jd)
        {
            mType = JsonHelper.ReadEnum<ECameraEffectType>(jd["Type"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Type", mType.ToString());

            return writer;
        }

        public EventData Clone()
        {
            EventCameraEffect evt = new EventCameraEffect();
            evt.mType = this.mType;

            return evt;
        }
    }

}
