/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\CameraCtrlShake.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-3      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;

    public sealed class CameraCtrlShake : XObject, ICameraController, ICameraApplyer
    {
        private float mIntensity = 0.7f;
        private bool mAttenuation = true;

        private bool mDisableX;
        private bool mDisableY;
        private float mDuration;

        private float mCurTime;
        private bool mIsShaking;
        private Vector2 mShakeOffset;

        public Vector2 ShakeOffset
        {
            get { return mShakeOffset; }
        }

        public ECameraCtrlType GetCtrlType()
        {
            return ECameraCtrlType.ECCT_Shake;
        }

        public void Init(params object[] param)
        {
            mDuration = (float)param[0];
            mDisableX = (bool)param[1];
            mDisableY = (bool)param[2];

            Reset();
        }

        public void Reset()
        {
            mCurTime = 0f;
            mIsShaking = false;
            mShakeOffset = Vector3.zero;
        }

        public void LateUpdate(CameraMgr mgr, float fTick)
        {
            if (!mIsShaking) return;

            float factor = mIntensity;
            if (mAttenuation)
            {
                float t = mCurTime / mDuration;
                factor = Mathf.Lerp(mIntensity, 0f, t);
            }

            mShakeOffset = Random.insideUnitSphere * factor;

            mShakeOffset.x = (mDisableX ? 0f : mShakeOffset.x);
            mShakeOffset.y = (mDisableY ? 0f : mShakeOffset.y);

            if (fTick == 0f)
                mShakeOffset = Vector2.zero;

            if (mDuration > 0f)
            {
                mCurTime += fTick;
                if (mCurTime >= mDuration)
                {
                    StopShake();
                }
            }
        }

        public void StopShake()
        {
            Reset();
        }

        public void StartShake(float intensity)
        {
            mCurTime = 0f;
            mIsShaking = true;
            mShakeOffset = Vector3.zero;
            mIntensity = intensity;
        }

        protected override void OnDispose()
        {

        }

        public void Apply(CameraMgr mgr, float fTick)
        {
            this.LateUpdate(mgr, fTick);

            mgr.Camera.position += new Vector3(ShakeOffset.x, ShakeOffset.y, 0f);
        }
    }

}
