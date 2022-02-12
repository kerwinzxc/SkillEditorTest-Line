/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitSound.cs
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

    public sealed class HitSound : IProperty, IHitFeedback
    {
        [SerializeField] private string mSound = string.Empty;
        [SerializeField] private int mSoundCount = 1;
        [SerializeField] private string mSoundCritical = string.Empty;
        [SerializeField] private int mSoundCriticalCount = 1;
        [SerializeField] private string mSoundBlock = string.Empty;
        [SerializeField] private int mSoundBlockCount = 1;
        
        #region property
        [EditorProperty("受击音效", EditorPropertyType.EEPT_GameObject)]
        public string Sound
        {
            get { return mSound; }
            set { mSound = value; }
        }
        [EditorProperty("受击音效实例数", EditorPropertyType.EEPT_Int)]
        public int SoundCount
        {
            get { return mSoundCount; }
            set { mSoundCount = value; }
        }
        [EditorProperty("暴击音效", EditorPropertyType.EEPT_GameObject)]
        public string SoundCritical
        {
            get { return mSoundCritical; }
            set { mSoundCritical = value; }
        }
        [EditorProperty("暴击音效实例数", EditorPropertyType.EEPT_Int)]
        public int SoundCriticalCount
        {
            get { return mSoundCriticalCount; }
            set { mSoundCriticalCount = value; }
        }
        [EditorProperty("格挡音效", EditorPropertyType.EEPT_GameObject)]
        public string SoundBlock
        {
            get { return mSoundBlock; }
            set { mSoundBlock = value; }
        }
        [EditorProperty("格挡音效实例数", EditorPropertyType.EEPT_Int)]
        public int SoundBlockCount
        {
            get { return mSoundBlockCount; }
            set { mSoundBlockCount = value; }
        }
        #endregion property

        public string DebugName
        {
            get { return "HitSound"; }
        }

        public void Deserialize(JsonData jd)
        {
            mSound = JsonHelper.ReadString(jd["Sound"]);
            mSoundCount = JsonHelper.ReadInt(jd["SoundCount"]);
            mSoundCritical = JsonHelper.ReadString(jd["SoundCritical"]);
            mSoundCriticalCount = JsonHelper.ReadInt(jd["SoundCriticalCount"]);
            mSoundBlock = JsonHelper.ReadString(jd["SoundBlock"]);
            mSoundBlockCount = JsonHelper.ReadInt(jd["SoundBlockCount"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Sound", mSound);
            JsonHelper.WriteProperty(ref writer, "SoundCount", mSoundCount);
            JsonHelper.WriteProperty(ref writer, "SoundCritical", mSoundCritical);
            JsonHelper.WriteProperty(ref writer, "SoundCriticalCount", mSoundCriticalCount);
            JsonHelper.WriteProperty(ref writer, "SoundBlock", mSoundBlock);
            JsonHelper.WriteProperty(ref writer, "SoundBlockCount", mSoundBlockCount);

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitSound; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            ECombatResult result = (ECombatResult)param[0];

            string soundName = mSound;
            int soundInstance = mSoundCount;
            switch (result)
            {
                case ECombatResult.ECR_Critical:
                    soundName = mSoundCritical;
                    soundInstance = mSoundCriticalCount;
                    break;
                case ECombatResult.ECR_Block:
                    soundName = mSoundBlock;
                    soundInstance = mSoundBlockCount;
                    break;
            }

            if (!string.IsNullOrEmpty(soundName))
            {
                AudioMgr.Instance.PlaySound(soundName, soundInstance);
            }
        }

    }
}