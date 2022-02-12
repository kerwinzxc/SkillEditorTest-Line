/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckWeaponType.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-18      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckWeaponType : InterruptCondition, IProperty
    {
        [SerializeField] private WeaponProperty.EWeaponType mWeaponType;

        #region property
        [EditorProperty("武器类型比较值", EditorPropertyType.EEPT_Enum)]
        public WeaponProperty.EWeaponType WeaponType
        {
            get { return mWeaponType; }
            set { mWeaponType = value; }
        }
        #endregion

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckWeaponType;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
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