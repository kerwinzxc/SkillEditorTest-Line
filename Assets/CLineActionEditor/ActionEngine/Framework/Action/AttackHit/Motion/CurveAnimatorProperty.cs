/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\CurveAnimatorProperty.cs
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

    public sealed class CurveAnimatorProperty : MotionAnimatorProperty
    {
        private bool mUseTargetDist = true;
        private float mSpeed = 0f;
        private float mCurveHeightCoeff = 0.3f;

        [EditorProperty("是否使用目标距离", EditorPropertyType.EEPT_Bool)]
        public bool UseTargetDist
        {
            get { return mUseTargetDist; }
            set { mUseTargetDist = value; }
        }
        [EditorProperty("曲线插值速度", EditorPropertyType.EEPT_Float)]
        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }
        [EditorProperty("曲线曲度", EditorPropertyType.EEPT_Float)]
        public float CurveHeightCoeff
        {
            get { return mCurveHeightCoeff; }
            set { mCurveHeightCoeff = value; }
        }

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mUseTargetDist = JsonHelper.ReadBool(jd["UseTargetDist"]);
            mSpeed = JsonHelper.ReadFloat(jd["Speed"]);
            mCurveHeightCoeff = JsonHelper.ReadFloat(jd["CurveHeightCoeff"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "UseTargetDist", mUseTargetDist);
            JsonHelper.WriteProperty(ref writer, "Speed", mSpeed);
            JsonHelper.WriteProperty(ref writer, "CurveHeightCoeff", mCurveHeightCoeff);

            return writer;
        }
    }

}
