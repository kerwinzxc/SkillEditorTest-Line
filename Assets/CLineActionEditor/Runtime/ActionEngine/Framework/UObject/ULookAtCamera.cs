/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\ULookAtCamera.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-26      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public sealed class ULookAtCamera : MonoBehaviour
    {
        void LateUpdate()
        {
            if (Camera.main)
                transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        }
    }
}
