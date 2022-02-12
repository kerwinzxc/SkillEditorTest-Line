/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Interpolator\CatmullRom.cs
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

    public static class CatmullRom
    {
        public static List<Vector3> MakeSpline(Vector3[] splinePoints, int segments = 7)
        {
            int pointsLength = splinePoints.Length;
            int numberOfPoints = pointsLength - 1;
            float add = 1.0f / segments * numberOfPoints;
            float i, start = 0.0f;
            int p0 = 0, p2 = 0, p3 = 0;

            List<Vector3> points = new List<Vector3>();

            for (int j = 0; j < numberOfPoints; j++)
            {
                p0 = j - 1;
                p2 = j + 1;
                p3 = j + 2;

                p0 = p0 < 0 ? 0 : p0;
                p3 = p3 > numberOfPoints - 1 ? numberOfPoints : p3;

                for (i = start; i <= 1.0f; i += add)
                {
                    points.Add(GetSplinePoint3D(ref splinePoints[p0], ref splinePoints[j], ref splinePoints[p2], ref splinePoints[p3], i));
                }

                start = i - 1.0f;
            }

            return points;
        }

        private static Vector3 GetSplinePoint3D(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, float t)
        {
            var px = Vector4.zero;
            var py = Vector4.zero;
            var pz = Vector4.zero;
            float dt0 = Mathf.Pow(VectorDistanceSquared(ref p0, ref p1), 0.25f);
            float dt1 = Mathf.Pow(VectorDistanceSquared(ref p1, ref p2), 0.25f);
            float dt2 = Mathf.Pow(VectorDistanceSquared(ref p2, ref p3), 0.25f);

            if (dt1 < 0.0001f) dt1 = 1.0f;
            if (dt0 < 0.0001f) dt0 = dt1;
            if (dt2 < 0.0001f) dt2 = dt1;

            InitNonuniformCatmullRom(p0.x, p1.x, p2.x, p3.x, dt0, dt1, dt2, ref px);
            InitNonuniformCatmullRom(p0.y, p1.y, p2.y, p3.y, dt0, dt1, dt2, ref py);
            InitNonuniformCatmullRom(p0.z, p1.z, p2.z, p3.z, dt0, dt1, dt2, ref pz);

            return new Vector3(EvalCubicPoly(ref px, t), EvalCubicPoly(ref py, t), EvalCubicPoly(ref pz, t));
        }

        private static float VectorDistanceSquared(ref Vector3 p, ref Vector3 q)
        {
            float dx = q.x - p.x;
            float dy = q.y - p.y;
            float dz = q.z - p.z;
            return dx * dx + dy * dy + dz * dz;
        }

        private static void InitNonuniformCatmullRom(float x0, float x1, float x2, float x3, float dt0, float dt1, float dt2, ref Vector4 p)
        {
            float t1 = ((x1 - x0) / dt0 - (x2 - x0) / (dt0 + dt1) + (x2 - x1) / dt1) * dt1;
            float t2 = ((x2 - x1) / dt1 - (x3 - x1) / (dt1 + dt2) + (x3 - x2) / dt2) * dt1;

            // Initialize cubic polygon
            p.x = x1;
            p.y = t1;
            p.z = -3 * x1 + 3 * x2 - 2 * t1 - t2;
            p.w = 2 * x1 - 2 * x2 + t1 + t2;
        }

        private static float EvalCubicPoly(ref Vector4 p, float t)
        {
            return p.x + p.y * t + p.z * (t * t) + p.w * (t * t * t);
        }
    }

}
