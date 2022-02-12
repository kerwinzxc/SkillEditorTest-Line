/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Emit\ArcEmitProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using LitJson;
    using System;

    public enum EArcEmitType
    {
        EAE_Circle,
        EAE_Sector,
        EAE_Sector_RelativeSelfDir,
        EAE_Circle_RelativeSelfDir
    }

    public class ArcEmitProperty : EmitProperty
    {
        [SerializeField] private EEmitterPosType mPosType = EEmitterPosType.EEPT_None;
        [SerializeField] private string mDummy = string.Empty;
        [SerializeField] private EArcEmitType mEmitType = EArcEmitType.EAE_Circle;
        [SerializeField] private int mIntervalDegree = 30;
        [SerializeField] private float mRadiusOffset = 0.0f;
        [SerializeField] private Vector3 mPosOffset = Vector3.zero;

        #region property
        [EditorProperty("发射初始位置类型", EditorPropertyType.EEPT_Enum)]
        public EEmitterPosType PosType
        {
            get { return mPosType; }
            set { mPosType = value; }
        }
        [EditorProperty("Dummy点", EditorPropertyType.EEPT_String)]
        public string Dummy
        {
            get { return mDummy; }
            set { mDummy = value; }
        }
        [EditorProperty("发射方式", EditorPropertyType.EEPT_Enum)]
        public EArcEmitType EmitType
        {
            get { return mEmitType; }
            set { mEmitType = value; }
        }
        [EditorProperty("间隔度数", EditorPropertyType.EEPT_Int)]
        public int IntervalDegree
        {
            get { return mIntervalDegree; }
            set { mIntervalDegree = value; }
        }
        [EditorProperty("偏移半径", EditorPropertyType.EEPT_Float)]
        public float RadiusOffset
        {
            get { return mRadiusOffset; }
            set { mRadiusOffset = value; }
        }
        [EditorProperty("偏移位置", EditorPropertyType.EEPT_Vector3)]
        public Vector3 PosOffset
        {
            get { return mPosOffset; }
            set { mPosOffset = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mPosType = JsonHelper.ReadEnum<EEmitterPosType>(jd["PosType"]);
            mDummy = JsonHelper.ReadString(jd["Dummy"]);
            mIntervalDegree = JsonHelper.ReadInt(jd["IntervalDegree"]);
            mRadiusOffset = JsonHelper.ReadFloat(jd["RadiusOffset"]);
            mPosOffset = JsonHelper.ReadVector3(jd["PosOffset"]);
            mEmitType = JsonHelper.ReadEnum<EArcEmitType>(jd["EmitType"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);
            JsonHelper.WriteProperty(ref writer, "PosType", mPosType.ToString());
            JsonHelper.WriteProperty(ref writer, "Dummy", mDummy);
            JsonHelper.WriteProperty(ref writer, "IntervalDegree", mIntervalDegree);
            JsonHelper.WriteProperty(ref writer, "RadiusOffset", mRadiusOffset);
            JsonHelper.WriteProperty(ref writer, "PosOffset", mPosOffset);
            JsonHelper.WriteProperty(ref writer, "EmitType", mEmitType.ToString());

            return writer;
        }
    }

}
