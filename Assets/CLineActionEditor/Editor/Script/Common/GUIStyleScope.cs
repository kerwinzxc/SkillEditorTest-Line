/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\GUIStyleScope.cs
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
    using UnityEngine;
    using System;

    internal struct GUIStyleScope : IDisposable
    {
        readonly GUIStyle style;
        readonly Color oldColor;

        public GUIStyleScope(GUIStyle style, Color newColor)
        {
            this.style = style;
            oldColor = style.normal.textColor;
            style.normal.textColor = newColor;
        }

        public void Dispose()
        {
            style.normal.textColor = oldColor;
        }
    }

}