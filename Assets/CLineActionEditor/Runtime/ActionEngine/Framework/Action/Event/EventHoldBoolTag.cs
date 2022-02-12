/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Runtime\ActionEngine\Framework\Action\Event\EventHoldBoolTag.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-11-10      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class EventHoldBoolTag : IEventData, IProperty
    {
        [SerializeField] private string mPropertySet;
        [SerializeField] private string mPropertyGet;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string PropertySet
        {
            get { return mPropertySet; }
            set { mPropertySet = value; }
        }
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string PropertyGet
        {
            get { return mPropertyGet; }
            set { mPropertyGet = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_EventHoldBoolTag; }
        }

        public string DebugName
        {
            get { return GetType().ToString(); }
        }
        public void Enter(Unit unit)
        {
            Execute(unit);
        }
        public void Update(Unit unit, int deltaTime)
        {
            Execute(unit);
        }
        public void Exit(Unit unit)
        {
            Helper.SetAny<bool>(unit.PropertyContext.GetProperty(PropertySet), false);
        }

        public void Execute(Unit unit)
        {
            var val = Helper.GetAny<bool>(unit.PropertyContext.GetProperty(PropertyGet));
            if (val)
            {
                Helper.SetAny<bool>(unit.PropertyContext.GetProperty(PropertySet), val);
            }
        }

        public void Deserialize(JsonData jd)
        {
            PropertySet = JsonHelper.ReadString(jd["PropertySet"]);
            PropertyGet = JsonHelper.ReadString(jd["PropertyGet"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "PropertySet", PropertySet);
            JsonHelper.WriteProperty(ref writer, "PropertyGet", PropertyGet);

            return writer;
        }

        public IEventData Clone()
        {
            EventHoldBoolTag evt = new EventHoldBoolTag();
            evt.PropertySet = PropertySet;
            evt.PropertyGet = PropertyGet;

            return evt;
        }
    }
}
