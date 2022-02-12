/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitEffect.cs
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

    public sealed class HitEffect : IProperty, IHitFeedback
    {
        [SerializeField] private string mEffect = string.Empty;
        [SerializeField] private string mEffectCritical = string.Empty;
        [SerializeField] private string mEffectBlock = string.Empty;
        [SerializeField] private Vector3 mEffectOffset = Vector3.zero;
        
        #region property
        [EditorProperty("受击特效", EditorPropertyType.EEPT_GameObject)]
        public string Effect
        {
            get { return mEffect; }
            set { mEffect = value; }
        }
        [EditorProperty("暴击特效", EditorPropertyType.EEPT_GameObject)]
        public string EffectCritical
        {
            get { return mEffectCritical; }
            set { mEffectCritical = value; }
        }
        [EditorProperty("格挡特效", EditorPropertyType.EEPT_GameObject)]
        public string EffectBlock
        {
            get { return mEffectBlock; }
            set { mEffectBlock = value; }
        }
        [EditorProperty("特效相对位置", EditorPropertyType.EEPT_Vector3)]
        public Vector3 EffectOffset
        {
            get { return mEffectOffset; }
            set { mEffectOffset = value; }
        }
        #endregion property

        public string DebugName
        {
            get { return "HitEffect"; }
        }

        public void Deserialize(JsonData jd)
        {
            mEffect = JsonHelper.ReadString(jd["Effect"]);
            mEffectCritical = JsonHelper.ReadString(jd["EffectCritical"]);
            mEffectBlock = JsonHelper.ReadString(jd["EffectBlock"]);
            mEffectOffset = JsonHelper.ReadVector3(jd["EffectOffset"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Effect", mEffect);
            JsonHelper.WriteProperty(ref writer, "EffectCritical", mEffectCritical);
            JsonHelper.WriteProperty(ref writer, "EffectBlock", mEffectBlock);
            JsonHelper.WriteProperty(ref writer, "EffectOffset", mEffectOffset);

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitEffect; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            ECombatResult result = (ECombatResult)param[0];

            string effectName = mEffect;
            switch (result)
            {
                case ECombatResult.ECR_Critical:
                    effectName = mEffectCritical;
                    break;
                case ECombatResult.ECR_Block:
                    effectName = mEffectBlock;
                    break;
            }

            if (!string.IsNullOrEmpty(effectName))
            {
                EffectMgr.Instance.PlayEffect(effectName, attackee.HitPosition == Vector3.zero ? attackee.CenterPosition : attackee.HitPosition, Quaternion.identity);
            }
        }
    }
}