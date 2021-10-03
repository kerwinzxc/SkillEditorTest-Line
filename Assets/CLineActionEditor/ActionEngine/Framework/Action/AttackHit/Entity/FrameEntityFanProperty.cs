/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\FrameEntityFanProperty.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-16      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;

    public sealed class FrameEntityFanProperty : AttackEntityProperty
    {
        private float mRadius;
        private float mHeight;
        private float mDegree;

        #region property
        [EditorProperty("扇形半径", EditorPropertyType.EEPT_Float)]
        public float Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }
        [EditorProperty("扇形高", EditorPropertyType.EEPT_Float)]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        [EditorProperty("扇形角度", EditorPropertyType.EEPT_Float)]
        public float Degree
        {
            get { return mDegree; }
            set { mDegree = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mRadius = JsonHelper.ReadFloat(jd["Radius"]);
            mHeight = JsonHelper.ReadFloat(jd["Height"]);
            mDegree = JsonHelper.ReadFloat(jd["Degree"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Radius", mRadius);
            JsonHelper.WriteProperty(ref writer, "Height", mHeight);
            JsonHelper.WriteProperty(ref writer, "Degree", mDegree);

            return writer;
        }
    }

}
