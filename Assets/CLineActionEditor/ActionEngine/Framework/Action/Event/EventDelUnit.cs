/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventDelUnit.cs
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
    using UnityEngine;

    public class EventDelUnit : EventData
    {
        private bool mDelete = false;
        
        #region property
        [EditorProperty("是否真实删除", EditorPropertyType.EEPT_Bool)]
        public bool Delete
        {
            get { return mDelete; }
            set { mDelete = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_DelUnit; }
        }

        public void Execute(Unit unit)
        {
            if (mDelete)
            {
                unit.IsDeleted = true;
            }
            else
            {
                // TO CLine: 或者做溶解特效消失
                unit.UObject.gameObject.SetActive(false);
            }
        }

        public void Deserialize(JsonData jd)
        {
            mDelete = JsonHelper.ReadBool(jd["Delete"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Delete", mDelete);

            return writer;
        }

        public EventData Clone()
        {
            EventDelUnit evt = new EventDelUnit();
            evt.mDelete = this.mDelete;

            return evt;
        }
    }

}
