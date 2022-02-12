/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\Action\Event\EventSetCustomPropertyInt32Rand.cs
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

    public class EventSetCustomPropertyInt32Rand : IEventData, IProperty
    {
        [SerializeField] private string mProperty;
        [SerializeField] private int mValMin;
        [SerializeField] private int mValMax;

        #region property
        [EditorProperty("自定义属性名称", EditorPropertyType.EEPT_CustomProperty)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("随机最小值", EditorPropertyType.EEPT_Int)]
        public int ValMin
        {
            get { return mValMin; }
            set { mValMin = value; }
        }
        [EditorProperty("随机最大值", EditorPropertyType.EEPT_Int)]
        public int ValMax
        {
            get { return mValMax; }
            set { mValMax = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_SetCustomPropertyInt32Rand; }
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
            int v = UnityEngine.Random.Range(ValMin, ValMax);
            Helper.SetAny<int>(unit.PropertyContext.GetProperty(Property), v);
        }

        public void Deserialize(JsonData jd)
        {
            Property = JsonHelper.ReadString(jd["Property"]);
            ValMin = JsonHelper.ReadInt(jd["ValMin"]);
            ValMax = JsonHelper.ReadInt(jd["ValMax"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", Property);
            JsonHelper.WriteProperty(ref writer, "ValMin", ValMin);
            JsonHelper.WriteProperty(ref writer, "ValMax", ValMax);

            return writer;
        }

        public IEventData Clone()
        {
            EventSetCustomPropertyInt32Rand evt = new EventSetCustomPropertyInt32Rand();
            evt.Property = Property;
            evt.ValMin = ValMin;
            evt.ValMax = ValMax;

            return evt;
        }
    }

}