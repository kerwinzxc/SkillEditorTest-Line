/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventHideTrail.cs
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

    public sealed class EventHideTrail : EventData, IEventWhenActionEndExecute
    {
        private float mStopSmoothlyFadeTime = 0.3f;

        #region property
        [EditorProperty("停止平滑过渡时间", EditorPropertyType.EEPT_Float)]
        public float StopSmoothlyFadeTime
        {
            get { return mStopSmoothlyFadeTime; }
            set { mStopSmoothlyFadeTime = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_HideTrail; }
        }

        public void Execute(Unit unit)
        {
            unit.EquipWeapon.HideTrail(mStopSmoothlyFadeTime);
        }

        public void ExecuteOnActionEnd(Unit unit)
        {
            Execute(unit);
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

        public EventData Clone()
        {
            EventHideTrail evt = new EventHideTrail();
            evt.mStopSmoothlyFadeTime = this.mStopSmoothlyFadeTime;

            return evt;
        }
    }

}
