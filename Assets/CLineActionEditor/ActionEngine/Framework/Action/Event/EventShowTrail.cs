/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventShowTrail.cs
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

    public class EventShowTrail : EventData
    {
        public EEventType EventType
        {
            get { return EEventType.EET_ShowTrail; }
        }

        public void Execute(Unit unit)
        {
            unit.EquipWeapon.ShowTrail();
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
            EventShowTrail evt = new EventShowTrail();

            return evt;
        }
    }

}
