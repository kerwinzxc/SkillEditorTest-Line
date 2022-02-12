/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Interpolator\Interpolator.cs
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

    public enum EInterpolatorType
    {
        EIT_None,
        EIT_Line,
        EIT_CatmullRom,
        EIT_PingPong,
    }

    public interface Interpolator
    {
        EInterpolatorType InplType { get; }
        Vector3 Displace { get; set; }
        Quaternion Rotation { get; set; }
        float RemainDistance { get; set; }
        void Interpolate(float fTick);
        bool IsFinished();
        void Reset();
    }

}
