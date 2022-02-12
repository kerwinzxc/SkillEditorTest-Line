/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Entity\FrameEntityCubProperty.cs
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
    using UnityEngine;

    public sealed class FrameEntityCubProperty : AttackEntityProperty
    {
        [SerializeField] private float mLength;
        [SerializeField] private float mWidth;
        [SerializeField] private float mHeight;

        #region property
        [EditorProperty("立方体长", EditorPropertyType.EEPT_Float)]
        public float Length
        {
            get { return mLength; }
            set { mLength = value; }
        }
        [EditorProperty("立方体宽", EditorPropertyType.EEPT_Float)]
        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }
        [EditorProperty("立方体高", EditorPropertyType.EEPT_Float)]
        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mLength = JsonHelper.ReadFloat(jd["Length"]);
            mWidth = JsonHelper.ReadFloat(jd["Width"]);
            mHeight = JsonHelper.ReadFloat(jd["Height"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Length", mLength);
            JsonHelper.WriteProperty(ref writer, "Width", mWidth);
            JsonHelper.WriteProperty(ref writer, "Height", mHeight);

            return writer;
        }

    }
}
