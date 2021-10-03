/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventActionBreak.cs
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

    public sealed class EventActionBreak : EventData
    {
        private bool mEnableBreak = true;

        #region property
        [EditorProperty("开启打断", EditorPropertyType.EEPT_Bool)]
        public bool EnableBreak
        {
            get { return mEnableBreak; }
            set { mEnableBreak = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_ActionBreak; }
        }

        public void Execute(Unit unit)
        {
            unit.ActionStatus.EnableBreak = mEnableBreak;
        }

        public void Deserialize(JsonData jd)
        {
            mEnableBreak = JsonHelper.ReadBool(jd["EnableBreak"]);
        }
        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "EnableBreak", mEnableBreak);

            return writer;
        }

        public EventData Clone()
        {
            EventActionBreak evt = new EventActionBreak();
            evt.mEnableBreak = this.mEnableBreak;

            return evt;
        }
    }

}
