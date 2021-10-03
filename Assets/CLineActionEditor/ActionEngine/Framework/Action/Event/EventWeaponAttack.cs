/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventWeaponAttack.cs
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
    public sealed class EventWeaponAttack : EventData
    {
        private bool mIsLeftDummy = false;

        #region property
        [EditorProperty("左手挂点", EditorPropertyType.EEPT_Bool)]
        public bool IsLeftDummy
        {
            get { return mIsLeftDummy; }
            set { mIsLeftDummy = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_WeaponAttack; }
        }

        public void Execute(Unit unit)
        {
            unit.EquipWeapon.DoAttack(mIsLeftDummy);
        }

        public void Deserialize(LitJson.JsonData jd)
        {
            mIsLeftDummy = JsonHelper.ReadBool(jd["IsLeftDummy"]);
        }

        public LitJson.JsonWriter Serialize(LitJson.JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "IsLeftDummy", mIsLeftDummy);

            return writer;
        }

        public EventData Clone()
        {
            EventWeaponAttack evt = new EventWeaponAttack();
            evt.mIsLeftDummy = this.mIsLeftDummy;

            return evt;
        }
    }
}