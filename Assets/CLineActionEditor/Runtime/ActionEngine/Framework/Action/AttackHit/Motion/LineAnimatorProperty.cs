/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Motion\LineAnimatorProperty.cs
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

    public class LineAnimatorProperty : MotionAnimatorProperty
    {
        [SerializeField] private float mSpeed = 0f;
        [SerializeField] private float mAcc = 0f;

        #region property
        [EditorProperty("直线插值速度", EditorPropertyType.EEPT_Float)]
        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }
        [EditorProperty("直线插值加速度", EditorPropertyType.EEPT_Float)]
        public float Acc
        {
            get { return mAcc; }
            set { mAcc = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mSpeed = JsonHelper.ReadFloat(jd["Speed"]);
            mAcc = JsonHelper.ReadFloat(jd["Acc"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "Speed", mSpeed);
            JsonHelper.WriteProperty(ref writer, "Acc", mAcc);

            return writer;
        }
    }

}
