/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\PingPongAnimatorProperty.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public class PingPongAnimatorProperty : LineAnimatorProperty
    {
        private int mPingPongTimes = 1;

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
