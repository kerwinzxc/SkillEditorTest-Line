/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventHideTrail.cs
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

    public sealed class EventHideTrail : IEventData, IProperty
    {
        [SerializeField] private float mStopSmoothlyFadeTime = 0.3f;

        #region property
        [EditorProperty("停止平滑过渡时间", EditorPropertyType.EEPT_Float)]
        public float StopSmoothlyFadeTime
        {
            get { return mStopSmoothlyFadeTime; }
            set { mStopSmoothlyFadeTime = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_HideTrail; }
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
            unit.EquipWeapon.HideTrail(mStopSmoothlyFadeTime);
        }

        public void Deserialize(LitJson.JsonData jd)
        {
            mStopSmoothlyFadeTime = JsonHelper.ReadFloat(jd["StopSmoothlyFadeTime"]);
        }

        public JsonWriter Serialize(LitJson.JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "StopSmoothlyFadeTime", mStopSmoothlyFadeTime);

            return writer;
        }

        public IEventData Clone()
        {
            EventHideTrail evt = new EventHideTrail();
            evt.mStopSmoothlyFadeTime = this.mStopSmoothlyFadeTime;

            return evt;
        }
    }

}
