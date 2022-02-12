/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\FrameEntityRingProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-16      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;

    public sealed class FrameEntityRingProperty : AttackEntityProperty
    {
        private float mInnerRadius;
        private float mOuterRadius;
        private float mHeight;

        #region property
        [EditorProperty("圆环内半径", EditorPropertyType.EEPT_Float)]
        public float InnerRadius
        {
            get { return mInnerRadius; }
            set { mInnerRadius = value; }
        }
        [EditorProperty("圆环外半径", EditorPropertyType.EEPT_Float)]
        public float OuterRadius
        {
            get { return mOuterRadius; }
            set { mOuterRadius = value; }
        }
        [EditorProperty("圆环高度", EditorPropertyType.EEPT_Float)]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mInnerRadius = JsonHelper.ReadFloat(jd["InnerRadius"]);
            mOuterRadius = JsonHelper.ReadFloat(jd["OuterRadius"]);
            mHeight = JsonHelper.ReadFloat(jd["Height"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "InnerRadius", mInnerRadius);
            JsonHelper.WriteProperty(ref writer, "OuterRadius", mOuterRadius);
            JsonHelper.WriteProperty(ref writer, "Height", mHeight);

            return writer;
        }
    }

}
