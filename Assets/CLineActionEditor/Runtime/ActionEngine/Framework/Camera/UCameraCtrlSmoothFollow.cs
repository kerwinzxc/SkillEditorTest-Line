/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Camera\UCameraCtrlSmoothFollow.cs
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

    public class UCameraCtrlSmoothFollow : MonoBehaviour
    {
        public float smoothXOZ;
        public float smoothY;
        public Vector3 offset;
        public Vector3 euler;
        public Transform follow;

        void Awake()
        {
            smoothXOZ = 0.1f;
            smoothY = 0.6f;
            offset = new Vector3(0, 3.5f, -6.5f);
            euler = new Vector3(20, 0, 0);
        }

    }
}
