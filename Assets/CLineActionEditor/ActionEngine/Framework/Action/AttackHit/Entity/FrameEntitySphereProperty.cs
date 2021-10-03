/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\FrameEntitySphereProperty.cs
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

    public sealed class FrameEntitySphereProperty : AttackEntityProperty
    {
        private float mRadius;

        #region property
        [EditorProperty("球半径", EditorPropertyType.EEPT_Float)]
        public float Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mRadius = JsonHelper.ReadFloat(jd["Radius"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Radius", mRadius);

            return writer;
        }
    }

}
