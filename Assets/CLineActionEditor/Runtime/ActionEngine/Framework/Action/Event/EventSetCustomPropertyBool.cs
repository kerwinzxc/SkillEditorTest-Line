/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\Action\Event\EventSetCustomPropertyBool.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-8-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public class EventSetCustomPropertyBool : IEventData, IProperty
    {
        [SerializeField] private string mProperty;
        [SerializeField] private bool mVal;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("值", EditorPropertyType.EEPT_Bool)]
        public bool Val
        {
            get { return mVal; }
            set { mVal = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_SetCustomPropertyBool; }
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

        }
        public void Exit(Unit unit)
        {
            Helper.SetAny<bool>(unit.PropertyContext.GetProperty(Property), false);
        }

        public void Execute(Unit unit)
        {
            Helper.SetAny<bool>(unit.PropertyContext.GetProperty(Property), Val);
        }

        public void Deserialize(JsonData jd)
        {
            Property = JsonHelper.ReadString(jd["Property"]);
            Val = JsonHelper.ReadBool(jd["Val"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", Property);
            JsonHelper.WriteProperty(ref writer, "Val", Val);

            return writer;
        }

        public IEventData Clone()
        {
            EventSetCustomPropertyBool evt = new EventSetCustomPropertyBool();
            evt.Property = Property;
            evt.Val = Val;

            return evt;
        }
    }

}