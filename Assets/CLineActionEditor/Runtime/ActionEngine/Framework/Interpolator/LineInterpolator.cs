/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Interpolator\LineInterpolator.cs
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

    public enum EInterpolatorSpace
    {
        EIS_3D,
        EIS_XOZ,
    }

    public sealed class LineInterpolator : Interpolator
    {
        private float mSpeed;
        private float mAcc;
        private float mTime;
        private Vector3 mDirection;
        private bool mInitialize;


        public void Init(float dist, Vector3 dir, float speed, float acc)
        {
            if (acc == 0f)
            {
                mTime = dist / speed;
            }
            else
            {
                mTime = (Mathf.Sqrt(speed * speed + 2 * acc * dist) - speed) / acc;
            }

            mSpeed = speed;
            mAcc = acc;
            mDirection = dir.normalized;
            mInitialize = true;

            Rotation = Helper.LookRotation(mDirection);
        }

        public void Init(float dist, Vector3 dir, float time, bool uniformSpeed)
        {
            if (uniformSpeed)
            {
                mSpeed = dist / time;
                mAcc = 0f;
            }
            else
            {
                mSpeed = 0f;
                mAcc = 2 * dist / Helper.DistanceSqr(time);
            }

            mDirection = dir.normalized;
            mTime = time;
            mInitialize = true;

            Rotation = Helper.LookRotation(mDirection);
        }

        public void Init(Vector3 begin, Vector3 end, float speed, float acc)
        {
            Vector3 p = end - begin;

            if (!p.Equals(Vector3.zero))
                Init(p.magnitude, p.normalized, speed, acc);
        }

        public void Init(Vector3 begin, Vector3 end, float time, bool uniformSpeed)
        {
            Vector3 p = end - begin;

            if (!p.Equals(Vector3.zero))
                Init(p.magnitude, p.normalized, time, uniformSpeed);
        }

        public EInterpolatorType InplType { get { return EInterpolatorType.EIT_Line; } }
        public Quaternion Rotation { get; set; }
        public Vector3 Displace { get; set; }
        public float RemainDistance { get; set; }
        public void Interpolate(float fTick)
        {
            if (!mInitialize || mDirection.Equals(Vector3.zero)) return;

            if (mTime == 0f)
            {
                Displace = Vector3.zero;
                return;
            }

            if (mTime >= fTick)
            {
                mTime -= fTick;
            }
            else
            {
                fTick = mTime;
                mTime = 0f;
            }

            if (mAcc != 0f)
            {
                float dv = mAcc * fTick;
                Displace = (mSpeed + 0.5f * dv) * fTick * mDirection;
                mSpeed += dv;
            }
            else
            {
                Displace = mSpeed * fTick * mDirection;
            }
        }
        public bool IsFinished()
        {
            return (mTime == 0f);
        }
        public void Reset()
        {
            mInitialize = false;
        }

    }

}
