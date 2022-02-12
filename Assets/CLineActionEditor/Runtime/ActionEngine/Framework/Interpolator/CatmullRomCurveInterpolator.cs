/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Interpolator\CatmullRomCurveInterpolator.cs
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
    using System.Collections.Generic;

    public sealed class CatmullRomCurveInterpolator : Interpolator
    {
        private float mSpeed;
        private List<Vector3> mPointList = new List<Vector3>();
        private Vector3 mCalcDir = Vector3.zero;
        private Vector3 mCalcPos = Vector3.zero;

        private int mPosIndex;

        public void Init(Vector3 startPos, Vector3 endPos, float speed, float heightCoefficient = 0.3f)
        {
            mPosIndex = 0;
            mSpeed = speed;
            mCalcPos = startPos;

            Vector3 dist = endPos - startPos;
            mCalcDir = dist.normalized;
            dist.y = 0; // TO CLine: just xoz plane, use 3d later.

            Vector3 midPos = startPos + dist.normalized * dist.magnitude * 0.5f;
            midPos.y += dist.magnitude * 0.5f * heightCoefficient;

            mPointList = CatmullRom.MakeSpline(new Vector3[] { startPos, midPos, endPos }, (int)Mathf.Ceil(dist.magnitude) * 2);

            Displace = startPos;
            Rotation = Helper.LookRotation(mCalcDir);
        }

        public EInterpolatorType InplType { get { return EInterpolatorType.EIT_CatmullRom; } }
        public Vector3 Displace{ get; set;}
        public Quaternion Rotation { get; set; }
        public float RemainDistance { get; set; }

        public void Interpolate(float fTick)
        {
            mCalcPos += mCalcDir * mSpeed * fTick;

            int idx = -1;
            for (int i = mPosIndex; i < mPointList.Count; ++i)
            {
                Vector3 dir = mPointList[i] - mCalcPos;
                if (Vector3.Dot(mCalcDir, dir.normalized) >= 0)
                {
                    idx = i;
                    break;
                }
            }

            mPosIndex = (idx != -1 ? idx : mPointList.Count - 1);

            if (mPosIndex > 0 && mPosIndex < mPointList.Count)
            {
                Vector3 p1 = mPointList[mPosIndex - 1];
                Vector3 p2 = mPointList[mPosIndex];

                Displace = p2;
                Rotation = Quaternion.LookRotation(p2 - p1);
            }
        }

        public bool IsFinished()
        {
            return mPosIndex == mPointList.Count - 1;
        }

        public void Reset()
        {
            mPosIndex = 0;
            mPointList.Clear();
        }
    }

}
