/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventCameraEffect.cs
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

    public sealed class EventCameraEffect : IEventData, IProperty
    {
        [SerializeField] private ECameraEffectType mType;

        #region property
        [EditorProperty("特效类型", EditorPropertyType.EEPT_Enum)]
        public ECameraEffectType Type
        {
            get { return mType; }
            set { mType = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_CameraEffect; }
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

        public IEventData Clone()
        {
            EventCameraEffect evt = new EventCameraEffect();
            evt.mType = this.mType;

            return evt;
        }
    }

}
