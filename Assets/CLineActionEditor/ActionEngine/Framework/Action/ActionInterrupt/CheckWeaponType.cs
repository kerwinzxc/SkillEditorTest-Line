/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckWeaponType.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-18      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class CheckWeaponType : InterruptCondition
    {
        private WeaponProperty.EWeaponType mWeaponType;

        [EditorProperty("武器类型比较值", EditorPropertyType.EEPT_Enum)]
        public WeaponProperty.EWeaponType WeaponType
        {
            get { return mWeaponType; }
            set { mWeaponType = value; }
        }

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckWeaponType;
            }
        }

        public bool CheckInterrupt(Unit unit)
        {
            return unit.EquipWeapon.Property.WeaponType == WeaponType;
        }

        public void Deserialize(JsonData jd)
        {
            mWeaponType = JsonHelper.ReadEnum<WeaponProperty.EWeaponType>(jd["WeaponType"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "WeaponType", mWeaponType.ToString());

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckWeaponType obj = new CheckWeaponType();
            obj.WeaponType = this.WeaponType;

            return obj;
        }
    }
}