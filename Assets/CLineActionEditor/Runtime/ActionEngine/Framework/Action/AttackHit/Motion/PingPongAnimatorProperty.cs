/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\PingPongAnimatorProperty.cs
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

    public class PingPongAnimatorProperty : LineAnimatorProperty
    {
        [SerializeField] private int mPingPongTimes = 1;

        #region property
        [EditorProperty("PingPong次数 ", EditorPropertyType.EEPT_Int)]
        public int PingPongTimes
        {
            get { return mPingPongTimes; }
            set { mPingPongTimes = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mPingPongTimes = JsonHelper.ReadInt(jd["PingPongTimes"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "PingPongTimes", mPingPongTimes);

            return writer;
        }
    }
}
