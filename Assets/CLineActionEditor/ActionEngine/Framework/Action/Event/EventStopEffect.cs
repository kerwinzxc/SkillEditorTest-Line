/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventStopEffect.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-6-12      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public sealed class EventStopEffect : EventData
    {
        private string mLoopEffectName = string.Empty;

        #region property
        [EditorProperty("循环特效唯一名字", EditorPropertyType.EEPT_String)]
        public string LoopEffectName
        {
            get { return mLoopEffectName; }
            set { mLoopEffectName = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_StopEffect; }
        }

        public void Execute(Unit unit)
        {
            EffectMgr.Instance.StopEffect(mLoopEffectName);
        }

        public void Deserialize(LitJson.JsonData jd)
        {
            mLoopEffectName = JsonHelper.ReadString(jd["LoopEffectName"]);
        }

        public LitJson.JsonWriter Serialize(LitJson.JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "LoopEffectName", mLoopEffectName);

            return writer;
        }

        public EventData Clone()
        {
            EventStopEffect evt = new EventStopEffect();
            evt.mLoopEffectName = this.mLoopEffectName;

            return evt;
        }
    }
}