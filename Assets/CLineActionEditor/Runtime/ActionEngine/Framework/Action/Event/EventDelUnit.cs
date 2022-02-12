/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventDelUnit.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-19      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public class EventDelUnit : IEventData, IProperty
    {
        [SerializeField] private bool mDelete = false;

        #region property
        [EditorProperty("是否真实删除", EditorPropertyType.EEPT_Bool)]
        public bool Delete
        {
            get { return mDelete; }
            set { mDelete = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_DelUnit; }
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

        public IEventData Clone()
        {
            EventDelUnit evt = new EventDelUnit();
            evt.mDelete = this.mDelete;

            return evt;
        }
    }

}
