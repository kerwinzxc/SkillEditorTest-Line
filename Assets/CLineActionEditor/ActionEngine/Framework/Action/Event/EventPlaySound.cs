/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventPlaySound.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-19      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using System.Collections.Generic;

    public class EventPlaySound : EventData
    {
        private string mSoundName = string.Empty;
        private int mInstanceCount = 3;
        private float mDelayTime = 0f;
        private bool mUseRandom = false;
        private List<string> mRandomSoundList = new List<string>();

        #region property
        [EditorProperty("声音名称", EditorPropertyType.EEPT_GameObjectToString)]
        public string SoundName
        {
            get { return mSoundName; }
            set { mSoundName = value; }
        }
        [EditorProperty("实例个数", EditorPropertyType.EEPT_Int)]
        public int InstanceCount
        {
            get { return mInstanceCount; }
            set { mInstanceCount = value; }
        }
        [EditorProperty("延迟时间", EditorPropertyType.EEPT_Float)]
        public float DelayTime
        {
            get { return mDelayTime; }
            set { mDelayTime = value; }
        }
        [EditorProperty("是否随机音效", EditorPropertyType.EEPT_Bool)]
        public bool UseRandom
        {
            get { return mUseRandom; }
            set { mUseRandom = value; }
        }
        [EditorProperty("随机音效列表", EditorPropertyType.EEPT_GameObjectToStringList)]
        public List<string> RandomSoundList
        {
            get { return mRandomSoundList; }
            set { mRandomSoundList = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_PlaySound; }
        }

        public void Execute(Unit unit)
        {
            if (mDelayTime > 0.0001f)
                TimerMgr.Instance.AddTimer(mDelayTime, false, 0, PlaySound);
            else
                PlaySound();
        }

        public void Deserialize(JsonData jd)
        {
            mSoundName = JsonHelper.ReadString(jd["SoundName"]);         
            mInstanceCount = JsonHelper.ReadInt(jd["InstanceCount"]);
            mDelayTime = JsonHelper.ReadFloat(jd["DelayTime"]);
            mUseRandom = JsonHelper.ReadBool(jd["UseRandom"]);
            mRandomSoundList = JsonHelper.ReadListString(jd["RandomSoundList"]);
        }
        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "SoundName", mSoundName);
            JsonHelper.WriteProperty(ref writer, "InstanceCount", mInstanceCount);
            JsonHelper.WriteProperty(ref writer, "DelayTime", mDelayTime);
            JsonHelper.WriteProperty(ref writer, "UseRandom", mUseRandom);
            JsonHelper.WriteProperty(ref writer, "RandomSoundList", mRandomSoundList);

            return writer;
        }

        private void PlaySound(params object[] obj)
        {
            string sound = string.Empty;
            if (mUseRandom)
            {
                if (mRandomSoundList.Count > 0)
                {
                    int idx = UnityEngine.Random.Range(0, mRandomSoundList.Count);
                    sound = mRandomSoundList[idx];
                }
            }
            else
            {
                sound = mSoundName;
            }

            if (!string.IsNullOrEmpty(sound))
                AudioMgr.Instance.PlaySound(sound, mInstanceCount);
        }

        public EventData Clone()
        {
            EventPlaySound evt = new EventPlaySound();
            evt.mSoundName = this.mSoundName;
            evt.mInstanceCount = this.mInstanceCount;
            evt.mDelayTime = this.mDelayTime;
            evt.mUseRandom = this.mUseRandom;
            evt.mRandomSoundList.AddRange(this.mRandomSoundList);

            return evt;
        }

    }

}
