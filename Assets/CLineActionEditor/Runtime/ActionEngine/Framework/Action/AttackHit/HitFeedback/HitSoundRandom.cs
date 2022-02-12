/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitSoundRandom.cs
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

    public sealed class HitSoundRandom : IProperty, IHitFeedback
    {
        [SerializeField] private List<string> mSoundList = new List<string>();

        #region property
        [EditorProperty("随机音效", EditorPropertyType.EEPT_GameObjectList)]
        public List<string> SoundList
        {
            get { return mSoundList; }
            set { mSoundList = value; }
        }
        #endregion property

        public string DebugName
        {
            get { return "HitSoundRandom"; }
        }

        public void Deserialize(JsonData jd)
        {
            mSoundList = JsonHelper.ReadListString(jd["SoundList"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "SoundList", mSoundList);

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitSoundRandom; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            if (mSoundList.Count > 0)
            {
                int index = Random.Range(0, mSoundList.Count);
                AudioMgr.Instance.PlaySound(mSoundList[index]);
            }
        }

    }
}