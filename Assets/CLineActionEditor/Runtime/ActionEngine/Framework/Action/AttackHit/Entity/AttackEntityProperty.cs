/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\AttackEntityProperty.cs
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
    using UnityEngine;
    using LitJson;
    using System.Collections.Generic;

    public enum EEntityType
    {
        EET_None,
        EET_FrameCub,
        EET_FrameCylinder,
        EET_FrameFan,
        EET_FrameRing,
        EET_FrameSphere,

        EET_Physical,       // 碰撞Trigger
        EET_Bounce,         // 雷电反弹
        EET_Hook,           // 回旋勾
    }

    public class AttackEntityProperty : IProperty
    {
        [SerializeField] private string mEffect = string.Empty;
        [SerializeField] private string mSound = string.Empty;
        [SerializeField] private int mSoundCount = 1;
        [SerializeField] private float mDelay = 0f;

        #region property
        [EditorProperty("攻击体延迟生效时间", EditorPropertyType.EEPT_Float)]
        public float Delay
        {
            get { return mDelay; }
            set { mDelay = value; }
        }
        [EditorProperty("攻击体特效", EditorPropertyType.EEPT_GameObject)]
        public string Effect
        {
            get { return mEffect; }
            set { mEffect = value; }
        }
        [EditorProperty("攻击体音效", EditorPropertyType.EEPT_GameObject)]
        public string Sound
        {
            get { return mSound; }
            set { mSound = value; }
        }
        [EditorProperty("攻击体音效实例数", EditorPropertyType.EEPT_Int)]
        public int SoundCount
        {
            get { return mSoundCount; }
            set { mSoundCount = value; }
        }
        #endregion

        public string DebugName
        {
            get { return string.Empty; }
        }

        public virtual void Deserialize(JsonData jd)
        {
            mEffect = JsonHelper.ReadString(jd["Effect"]);
            mSound = JsonHelper.ReadString(jd["Sound"]);
            mSoundCount = JsonHelper.ReadInt(jd["SoundCount"]);
            mDelay = JsonHelper.ReadFloat(jd["Delay"]);
        }

        public virtual JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Effect", mEffect);
            JsonHelper.WriteProperty(ref writer, "Sound", mSound);
            JsonHelper.WriteProperty(ref writer, "SoundCount", mSoundCount);
            JsonHelper.WriteProperty(ref writer, "Delay", mDelay);
            return writer;
        }

    }

}
