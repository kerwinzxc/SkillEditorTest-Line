/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventMove.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-20      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using LitJson;

    public enum EMoveDirectionType
    {
        EMDT_MoveToTarget = 0,
        EMDT_LeaveAwayTarget,
        EMDT_Custom,
        EMDT_FrontFace,
    }

    public enum EMoveSpeedType
    {
        EMST_UseAnim = 0,
        EMST_Custom,
    }

    public class EventMove : IEventData, IProperty
    {
        [SerializeField] private bool mIgnorMove = false;
        [SerializeField] private bool mResetVelocity = true;
        [SerializeField] private EMoveDirectionType mMoveDirType = EMoveDirectionType.EMDT_Custom;
        [SerializeField] private Vector3 mCustomDirection = Vector3.zero;
        [SerializeField] private EMoveSpeedType mMoveSpeedType = EMoveSpeedType.EMST_Custom;
        [SerializeField] private float mCustomSpeed = 0f;
        [SerializeField] private float mAnimDist = 0f;
        [SerializeField] private float mAnimTime = 0f;
        [SerializeField] private bool mResetAcc = true;
        [SerializeField] private Vector3 mAcc = Vector3.zero;

        #region property
        [EditorProperty("是否忽略移动", EditorPropertyType.EEPT_Bool)]
        public bool IgnorMove
        {
            get { return mIgnorMove; }
            set { mIgnorMove = value; }
        }
        [EditorProperty("是否重置速度", EditorPropertyType.EEPT_Bool)]
        public bool ResetVelocity
        {
            get { return mResetVelocity; }
            set { mResetVelocity = value; }
        }
        [EditorProperty("移动方向类型", EditorPropertyType.EEPT_Enum)]
        public EMoveDirectionType MoveDirType
        {
            get { return mMoveDirType; }
            set { mMoveDirType = value; }
        }
        [EditorProperty("自定义移动方向", EditorPropertyType.EEPT_Vector3)]
        public Vector3 CustomDirection
        {
            get { return mCustomDirection; }
            set { mCustomDirection = value; }
        }
        [EditorProperty("移动速率类型", EditorPropertyType.EEPT_Enum)]
        public EMoveSpeedType MoveSpeedType
        {
            get { return mMoveSpeedType; }
            set { mMoveSpeedType = value; }
        }
        [EditorProperty("自定义速率", EditorPropertyType.EEPT_Float)]
        public float CustomSpeed
        {
            get { return mCustomSpeed; }
            set { mCustomSpeed = value; }
        }
        [EditorProperty("动画移动距离", EditorPropertyType.EEPT_Float)]
        public float AnimDist
        {
            get { return mAnimDist; }
            set { mAnimDist = value; }
        }
        [EditorProperty("动画移动时间", EditorPropertyType.EEPT_Float)]
        public float AnimTime
        {
            get { return mAnimTime; }
            set { mAnimTime = value; }
        }
        [EditorProperty("是否重置加速度", EditorPropertyType.EEPT_Bool)]
        public bool ResetAcc
        {
            get { return mResetAcc; }
            set { mResetAcc = value; }
        }
        [EditorProperty("自定义加速度", EditorPropertyType.EEPT_Vector3)]
        public Vector3 Acc
        {
            get { return mAcc; }
            set { mAcc = value; }
        }
        #endregion

        public EEventDataType EventType
        {
            get { return EEventDataType.EET_Move; }
        }

        public string DebugName
        {
            get { return GetType().ToString(); }
        }
        public void Enter(Unit unit)
        {

        }
        public void Update(Unit unit, int deltaTime)
        {

        }
        public void Exit(Unit unit)
        {

        }

        public void Execute(Unit unit)
        {
            unit.ActionStatus.IgnoreMove = mIgnorMove;

            Vector3 dir = Vector3.zero;
            switch (mMoveDirType)
            {
                case EMoveDirectionType.EMDT_MoveToTarget:
                    if (unit.Target != null && !unit.Target.IsDead && !unit.Target.IsDeleted)
                    {
                        dir = unit.Target.Position - unit.Position;
                        dir.Normalize();
                    }
                    break;
                case EMoveDirectionType.EMDT_LeaveAwayTarget:
                    if (unit.Target != null && !unit.Target.IsDead && !unit.Target.IsDeleted)
                    {
                        dir = unit.Position - unit.Target.Position;
                        dir.Normalize();
                    }
                    break;
                case EMoveDirectionType.EMDT_Custom:
                    {
                        dir = mCustomDirection;
                        dir.Normalize();
                    }
                    break;
                case EMoveDirectionType.EMDT_FrontFace:
                    {
                        dir = unit.UObject.forward;
                        dir.Normalize();
                    }
                    break;
            }

            float speed = 0f;
            switch (mMoveSpeedType)
            {
                case EMoveSpeedType.EMST_UseAnim:
                    {
                        speed = mAnimDist / mAnimTime;
                    }
                    break;
                case EMoveSpeedType.EMST_Custom:
                    {
                        speed = mCustomSpeed;
                    }
                    break;
            }

            if (mResetVelocity)
                unit.ActionStatus.SetVelocity(dir * speed);
            else
                unit.ActionStatus.ApplyVelocity(dir * speed);

            if (mResetAcc)
                unit.ActionStatus.SetAccelerate(mAcc);
            else
                unit.ActionStatus.ApplyAccelerate(mAcc);
        }

        public void Deserialize(JsonData jd)
        {
            mIgnorMove = JsonHelper.ReadBool(jd["IgnorMove"]);
            mResetVelocity = JsonHelper.ReadBool(jd["ResetVelocity"]);
            mMoveDirType = JsonHelper.ReadEnum<EMoveDirectionType>(jd["MoveDirType"]);
            mCustomDirection = JsonHelper.ReadVector3(jd["CustomDirection"]);
            mMoveSpeedType = JsonHelper.ReadEnum<EMoveSpeedType>(jd["MoveSpeedType"]);
            mCustomSpeed = JsonHelper.ReadFloat(jd["CustomSpeed"]);
            mAnimDist = JsonHelper.ReadFloat(jd["AnimDist"]);
            mAnimTime = JsonHelper.ReadFloat(jd["AnimTime"]);
            mResetAcc = JsonHelper.ReadBool(jd["ResetAcc"]);
            mAcc = JsonHelper.ReadVector3(jd["Acc"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "IgnorMove", mIgnorMove);
            JsonHelper.WriteProperty(ref writer, "ResetVelocity", mResetVelocity);
            JsonHelper.WriteProperty(ref writer, "MoveDirType", mMoveDirType.ToString());
            JsonHelper.WriteProperty(ref writer, "CustomDirection", mCustomDirection);
            JsonHelper.WriteProperty(ref writer, "MoveSpeedType", mMoveSpeedType.ToString());
            JsonHelper.WriteProperty(ref writer, "CustomSpeed", mCustomSpeed);
            JsonHelper.WriteProperty(ref writer, "AnimDist", mAnimDist);
            JsonHelper.WriteProperty(ref writer, "AnimTime", mAnimTime);
            JsonHelper.WriteProperty(ref writer, "ResetAcc", mResetAcc);
            JsonHelper.WriteProperty(ref writer, "Acc", mAcc);

            return writer;
        }

        public IEventData Clone()
        {
            EventMove evt = new EventMove();
            evt.mIgnorMove = this.mIgnorMove;
            evt.mResetVelocity = this.mResetVelocity;
            evt.mMoveDirType = this.mMoveDirType;
            evt.mCustomDirection = this.mCustomDirection;
            evt.mMoveSpeedType = this.mMoveSpeedType;
            evt.mCustomSpeed = this.mCustomSpeed;
            evt.mAnimDist = this.mAnimDist;
            evt.mAnimTime = this.mAnimTime;
            evt.mResetAcc = this.mResetAcc;
            evt.mAcc = this.mAcc;

            return evt;
        }

    }

}
