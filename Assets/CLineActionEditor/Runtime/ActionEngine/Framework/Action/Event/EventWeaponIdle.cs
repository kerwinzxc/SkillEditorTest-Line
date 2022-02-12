/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventWeaponIdle.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-9-7      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;

    public sealed class EventWeaponIdle : IEventData, IProperty
    {
        public EEventDataType EventType
        {
            get { return EEventDataType.EET_WeaponIdle; }
        }

        public void Execute(Unit unit)
        {
            unit.EquipWeapon.DoIdle();
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

        public void Deserialize(JsonData jd)
        {

        }

        public LitJson.JsonWriter Serialize(JsonWriter writer)
        {
            return writer;
        }

        public IEventData Clone()
        {
            EventWeaponIdle evt = new EventWeaponIdle();

            return evt;
        }
    }
}