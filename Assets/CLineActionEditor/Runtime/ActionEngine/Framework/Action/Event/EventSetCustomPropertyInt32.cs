/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\Action\Event\EventSetCustomPropertyInt32.cs
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

    public class EventSetCustomPropertyInt32 : IEventData, IProperty
    {
        [SerializeField] private string mProperty;
        [SerializeField] private int mVal;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("值", EditorPropertyType.EEPT_Int)]
        public int Val
        {
            get { return mVal; }
            set { mVal = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_SetCustomPropertyInt32; }
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
            Helper.SetAny<int>(unit.PropertyContext.GetProperty(Property), 0);
        }

        public void Execute(Unit unit)
        {
            Helper.SetAny<int>(unit.PropertyContext.GetProperty(Property), Val);
        }

        public void Deserialize(JsonData jd)
        {
            Property = JsonHelper.ReadString(jd["Property"]);
            Val = JsonHelper.ReadInt(jd["Val"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", Property);
            JsonHelper.WriteProperty(ref writer, "Val", Val);

            return writer;
        }

        public IEventData Clone()
        {
            EventSetCustomPropertyInt32 evt = new EventSetCustomPropertyInt32();
            evt.Property = Property;
            evt.Val = Val;

            return evt;
        }
    }

}