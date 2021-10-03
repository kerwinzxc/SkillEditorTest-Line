/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\AttackDefWrapper.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-22      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Text;

    public class AttackDefWrapper : XObject
    {
        private GameObject mRec = null;
        private GameObject mArea = null;
        private ActionAttackDef mAttack = null;
        private float mSimulateTime = 0f;

        public AttackDefWrapper(ActionAttackDef aad)
        {
            mAttack = aad;
            switch (mAttack.EmitType)
            {
                case EEmitType.EET_Normal:
                    {
                        NormalEmit();
                    }
                    break;
                default:
                    LogMgr.Instance.Logf(ELogType.ELT_WARNING, "Action", "current attack def can not preview --{0}", mAttack.Name);
                    break;
            }
        }


        void NormalEmit()
        {
            if (mAttack.EmitProperty is NormalEmitProperty)
            {
                Vector3 StartPosition = CalcStartPositionForNormal();
                Vector3 StartDirection = CalcStartRotationForNormal();

                mRec = new GameObject("temp");

                UDrawTool tool;
                switch (mAttack.EntityType)
                {
                    case EEntityType.EET_FrameCub:
                        break;
                    case EEntityType.EET_FrameCylinder:
                        {
                            FrameEntityCylinderProperty p = mAttack.EntityProperty as FrameEntityCylinderProperty;
                            mRec.transform.localScale = Vector3.one;
                            mRec.transform.localPosition = StartPosition;
                            mRec.transform.localRotation = Quaternion.LookRotation(Vector3.forward);
                            tool = mRec.AddComponent<UDrawTool>();
                            tool.DrawCircleSolid(mRec.transform, StartPosition, p.Radius);
                            mArea = tool.go;
                        }
                        break;
                    case EEntityType.EET_FrameFan:
                        {
                            FrameEntityFanProperty p = mAttack.EntityProperty as FrameEntityFanProperty;
                            mRec.transform.localScale = Vector3.one;
                            mRec.transform.localPosition = StartPosition;
                            mRec.transform.localRotation = Quaternion.LookRotation(StartDirection);
                            tool = mRec.AddComponent<UDrawTool>();
                            tool.DrawSectorSolid(mRec.transform, StartPosition, p.Degree, p.Radius);
                            mArea = tool.go;
                        }
                        break;
                    default:
                        LogMgr.Instance.Logf(ELogType.ELT_WARNING, "Core", "current attack def can not preview --{0}", mAttack.Name);
                        break;
                }
            }
        }

        Vector3 CalcStartPositionForNormal()
        {
            Vector3 pos = Vector3.zero;
            Vector3 fward = Vector3.zero;
            NormalEmitProperty mProperty = mAttack.EmitProperty as NormalEmitProperty;
            Transform unitTransform = UnitWrapper.Instance.UnitWrapperUnit.transform;
            switch (mProperty.PosType)
            {
                case EEmitterPosType.EEPT_AttackerCurrentPosAndDir:
                    {
                        pos = unitTransform.position;
                        fward = unitTransform.forward;
                    }
                    break;
            }


            Vector2 offsetXZ = new Vector2(mProperty.EmitOffset.x, mProperty.EmitOffset.z);
            Vector3 directionXZ = new Vector2(fward.x, fward.z);
            offsetXZ = offsetXZ.magnitude * directionXZ;
            pos += new Vector3(offsetXZ.x, mProperty.EmitOffset.y, offsetXZ.y);

            return pos;
        }

        Vector3 CalcStartRotationForNormal()
        {
            Vector3 direction;
            NormalEmitProperty mProperty = mAttack.EmitProperty as NormalEmitProperty;
            Transform unitTransform = UnitWrapper.Instance.UnitWrapperUnit.transform;
            switch (mProperty.PosType)
            {
                case EEmitterPosType.EEPT_AttackerCurrentPosAndDir:
                default:
                    direction = unitTransform.forward;
                    break;
            }

            if (mProperty.EmitRotation != Vector3.zero)
            {
                Quaternion qat = Quaternion.LookRotation(direction);
                Vector3 euler = mProperty.EmitRotation + qat.eulerAngles;
                direction = Quaternion.Euler(euler) * Vector3.forward;
            }

            return direction;
        }

        public void Tick(float fTick)
        {
            mSimulateTime += fTick;


        }


        protected override void OnDispose()
        {
            if (mRec)
                GameObject.DestroyImmediate(mRec);
            if (mArea)
                GameObject.DestroyImmediate(mArea);
        }
    }
}
