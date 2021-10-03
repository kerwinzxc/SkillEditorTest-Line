/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventWeaponIdle.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-9-7      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    public sealed class EventWeaponIdle : EventData
    {
        public EEventType EventType
        {
            get { return EEventType.EET_WeaponIdle; }
        }

        public void Execute(Unit unit)
        {
            unit.EquipWeapon.DoIdle();
        }

        public void Deserialize(LitJson.JsonData jd)
        {

        }

        public LitJson.JsonWriter Serialize(LitJson.JsonWriter writer)
        {
            return writer;
        }

        public EventData Clone()
        {
            EventWeaponIdle evt = new EventWeaponIdle();

            return evt;
        }
    }
}