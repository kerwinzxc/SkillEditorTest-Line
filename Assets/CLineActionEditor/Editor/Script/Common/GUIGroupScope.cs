/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\GUIGroupScope.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System;
    using UnityEngine;

    internal struct GUIGroupScope : IDisposable
    {
        public GUIGroupScope(Rect rect)
        {
            GUI.BeginGroup(rect);
        }

        public void Dispose()
        {
            GUI.EndGroup();
        }
    }
}