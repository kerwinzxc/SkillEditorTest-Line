/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventStopEffect.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-6-12      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class EventStopEffect : IEventData, IProperty
    {
        [SerializeField] private string mLoopEffectName = string.Empty;

        #region property
        [EditorProperty("循环特效唯一名字", EditorPropertyType.EEPT_String)]
        public string LoopEffectName
        {
            get { return mLoopEffectName; }
            set { mLoopEffectName = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_StopEffect; }
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
            EffectMgr.Instance.StopEffect(mLoopEffectName);
        }

        public void Deserialize(JsonData jd)
        {
            mLoopEffectName = JsonHelper.ReadString(jd["LoopEffectName"]);
        }

        public LitJson.JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "LoopEffectName", mLoopEffectName);

            return writer;
        }

        public IEventData Clone()
        {
            EventStopEffect evt = new EventStopEffect();
            evt.mLoopEffectName = this.mLoopEffectName;

            return evt;
        }
    }
}