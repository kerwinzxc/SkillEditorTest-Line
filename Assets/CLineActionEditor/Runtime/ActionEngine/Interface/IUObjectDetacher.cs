/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Interface\IUObjectDetacher.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : objectpool <===> XObject <===> UObject
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-10      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public interface IUObjectDetacher
    {
        void OnDetached(MonoBehaviour obj);
    }
}
