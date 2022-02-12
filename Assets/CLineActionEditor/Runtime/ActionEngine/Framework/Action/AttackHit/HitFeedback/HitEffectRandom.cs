/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitEffectRandom.cs
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
    using System.Collections.Generic;

    public sealed class HitEffectRandom : IProperty, IHitFeedback
    {
        [SerializeField] private List<string> mEffectList = new List<string>();

        #region property
        [EditorProperty("随机特效", EditorPropertyType.EEPT_GameObjectList)]
        public List<string> EffectList
        {
            get { return mEffectList; }
            set { mEffectList = value; }
        }
        #endregion property

        public string DebugName
        {
            get { return "HitEffectRandom"; }
        }

        public void Deserialize(JsonData jd)
        {
            mEffectList = JsonHelper.ReadListString(jd["EffectList"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "EffectList", mEffectList);

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitEffectRandom; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            if (mEffectList.Count > 0)
            {
                int index = Random.Range(0, mEffectList.Count);
                EffectMgr.Instance.PlayEffect(mEffectList[index], attackee.HitPosition == Vector3.zero ? attackee.CenterPosition : attackee.HitPosition, Quaternion.identity);
            }
        }

    }
}