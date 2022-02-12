/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventWeaponAttack.cs
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
    using UnityEngine;

    public sealed class EventWeaponAttack : IEventData, IProperty
    {
        [SerializeField] private bool mIsLeftDummy = false;

        #region property
        [EditorProperty("左手挂点", EditorPropertyType.EEPT_Bool)]
        public bool IsLeftDummy
        {
            get { return mIsLeftDummy; }
            set { mIsLeftDummy = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_WeaponAttack; }
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
            unit.EquipWeapon.DoAttack(mIsLeftDummy);
        }

        public void Deserialize(JsonData jd)
        {
            mIsLeftDummy = JsonHelper.ReadBool(jd["IsLeftDummy"]);
        }

        public LitJson.JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "IsLeftDummy", mIsLeftDummy);

            return writer;
        }

        public IEventData Clone()
        {
            EventWeaponAttack evt = new EventWeaponAttack();
            evt.mIsLeftDummy = this.mIsLeftDummy;

            return evt;
        }
    }
}