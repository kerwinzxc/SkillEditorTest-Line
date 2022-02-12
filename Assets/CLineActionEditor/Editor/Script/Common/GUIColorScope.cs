/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\GUIColorScope.cs
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

    internal struct GUIColorScope : IDisposable
    {
        private readonly Color color;
        public GUIColorScope(Color color)
        {
            this.color = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = this.color;
        }
    }

}