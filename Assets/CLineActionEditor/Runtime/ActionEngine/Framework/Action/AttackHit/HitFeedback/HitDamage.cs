/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitDamage.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-16      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using NumericalType = System.Double;

    public enum EDamageType
    {
        EDT_Normal, // default
    }

    public sealed class HitDamage : IProperty, IHitFeedback
    {
        // hit
        [SerializeField] private EDamageType mDamageType = EDamageType.EDT_Normal;

        #region property
        [EditorProperty("击中伤害类型", EditorPropertyType.EEPT_Enum)]
        public EDamageType DamageType
        {
            get { return mDamageType; }
            set { mDamageType = value; }
        }
        #endregion proerpty

        public string DebugName
        {
            get { return "HitDamage"; }
        }

        public void Deserialize(JsonData jd)
        {
            mDamageType = JsonHelper.ReadEnum<EDamageType>(jd["DamageType"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "DamageType", mDamageType.ToString());

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitDamage; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            ECombatResult result = (ECombatResult)param[0];
            NumericalType damage = (NumericalType)param[1];

            if (attackee.ActionStatus.CanHurt && !attackee.IsGod)
            {
                attackee.BeHurt(attacker, damage, result);
            }
        }
    }
}