/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\Emit\NormalEmitProperty.cs
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
    using System;

    public enum EEmitterPosType
    {
        EEPT_None,
        EEPT_AttackerCurrentPosAndDir,
        EEPT_AttackerCurrentPosAndTargetDir
    }

    public class NormalEmitProperty : EmitProperty
    {
        [SerializeField] protected EEmitterPosType mPosType = EEmitterPosType.EEPT_None;
        [SerializeField] protected string mDummy = string.Empty;
        [SerializeField] protected Vector3 mEmitOffset = Vector3.zero;
        [SerializeField] protected Vector3 mEmitRotation = Vector3.zero;

        #region property
        [EditorProperty("发射初始位置类型", EditorPropertyType.EEPT_Enum)]
        public EEmitterPosType PosType
        {
            get { return mPosType; }
            set { mPosType = value; }
        }
        [EditorProperty("发射初始位置Dummy点", EditorPropertyType.EEPT_String, Description = "角色或者武器上的Dummy点")]
        public string Dummy
        {
            get { return mDummy; }
            set { mDummy = value; }
        }
        [EditorProperty("相对位置", EditorPropertyType.EEPT_Vector3)]
        public UnityEngine.Vector3 EmitOffset
        {
            get { return mEmitOffset; }
            set { mEmitOffset = value; }
        }
        [EditorProperty("相对朝向", EditorPropertyType.EEPT_Vector3, Description = "相对角色正朝向或者角色和目标的连线")]
        public UnityEngine.Vector3 EmitRotation
        {
            get { return mEmitRotation; }
            set { mEmitRotation = value; }
        }
        #endregion

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mPosType = JsonHelper.ReadEnum<EEmitterPosType>(jd["PosType"]);
            mDummy = JsonHelper.ReadString(jd["Dummy"]);
            mEmitOffset = JsonHelper.ReadVector3(jd["EmitOffset"]);
            mEmitRotation = JsonHelper.ReadVector3(jd["EmitRotation"]);
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "PosType", mPosType.ToString());
            JsonHelper.WriteProperty(ref writer, "Dummy", mDummy);
            JsonHelper.WriteProperty(ref writer, "EmitOffset", mEmitOffset);
            JsonHelper.WriteProperty(ref writer, "EmitRotation", mEmitRotation);

            return writer;
        }
    }
}
