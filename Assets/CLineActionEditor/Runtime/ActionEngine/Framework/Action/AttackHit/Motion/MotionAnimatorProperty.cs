/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\MotionAnimatorProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public enum EMotionAnimatorType
    {
        EMAT_None,
        EMAT_Line,
        EMAT_Curve,
        EMAT_PingPong,
    }

    public class MotionAnimatorProperty : IProperty
    {
        [SerializeField] private bool mUseWeaponAttackDist = false;
        [SerializeField] private float mDistance = 10f;

        #region property
        [EditorProperty("是否使用武器攻击距离", EditorPropertyType.EEPT_Bool)]
        public bool UseWeaponAttackDist
        {
            get { return mUseWeaponAttackDist; }
            set { mUseWeaponAttackDist = value; }
        }
        [EditorProperty("插值距离", EditorPropertyType.EEPT_Float)]
        public float Distance
        {
            get { return mDistance; }
            set { mDistance = value; }
        }
        #endregion

        public string DebugName
        {
            get { return string.Empty; }
        }

        public virtual void Deserialize(JsonData jd)
        {
            mUseWeaponAttackDist = JsonHelper.ReadBool(jd["UseWeaponAttackDist"]);
            mDistance = JsonHelper.ReadFloat(jd["Distance"]);
        }

        public virtual JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "UseWeaponAttackDist", mUseWeaponAttackDist);
            JsonHelper.WriteProperty(ref writer, "Distance", mDistance);

            return writer;
        }
    }

}
