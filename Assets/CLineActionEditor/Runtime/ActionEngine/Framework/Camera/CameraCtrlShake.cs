/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\CameraCtrlShake.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-3      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class CameraCtrlShake : XObject, ICameraController, ICameraApplyer
    {
        private float mDuration = 0.2f;
        private float mAmplitude = 0.7f;
        private int mVibrato = 10;
        private bool mDisableX = true;
        private bool mDisableY = false;
        private bool mAttenuation = true;

        private float mVibratoTime = 0; // duration/vibrato
        private float mCurTime = 0;
        private int mCurVibrato = 0;
        private Vector3 mShakeOffset;

        private List<int> mMoveList = new List<int>();


        public bool Enable
        {
            get;
            set;
        }

        public ECameraCtrlType GetCtrlType()
        {
            return ECameraCtrlType.ECCT_Shake;
        }

        public CameraCtrlShake()
        {
            Enable = false;
        }

        public void Init(params object[] param)
        {
            mDuration = (float)param[0];
            mAmplitude = (float)param[1];
            mVibrato = (int)param[2];
            mDisableX = (bool)param[3];
            mDisableY = (bool)param[4];
            mAttenuation = (bool)param[5];

            mVibratoTime = mDuration / (2 * mVibrato);

            mCurTime = 0;
            mCurVibrato = 0;
            Enable = true;

            mShakeOffset = Helper.Vec3Zero;
            mMoveList.Clear();

            int rd = UnityEngine.Random.Range(0, 2);
            int direction = rd == 0 ? -1 : 1;
            for (int i = 0; i < 2 * mVibrato; i += 2)
            {
                mMoveList.Add(direction);
                mMoveList.Add(direction * -1);
            }
        }

        protected override void OnDispose()
        {
            mMoveList.Clear();
            Reset();
        }

        public void Reset()
        {
            mCurTime = 0;
            mCurVibrato = 0;
            Enable = false;
        }

        public void LateUpdate(CameraMgr mgr, float fTick)
        {

        }

        public void Apply(CameraMgr mgr, float fTick)
        {
            if (!Enable)
                return;

            if (mCurVibrato < 2 * mVibrato)
            {
                mCurTime += fTick;
                float t = Mathf.Clamp(mCurTime / mVibratoTime, 0, 1f);
                float b = mMoveList[mCurVibrato] * 1f;
                float percent = Mathf.Lerp(0, b, t);

                float intensity = mAmplitude;
                intensity *= percent;

                if (!mDisableX)
                {
                    mShakeOffset.x = intensity;
                }
                if (!mDisableY)
                {
                    mShakeOffset.y = intensity;
                }

                mgr.Camera.transform.position += mShakeOffset;

                if (t == 1)
                {
                    mCurTime -= mVibratoTime;
                    mCurVibrato++;
                    if (mAttenuation && mCurVibrato % 2 == 0)
                    {
                        mAmplitude *= 0.5f;
                    }
                }
            }
            else
            {
                Reset();
            }
        }


    }

}
