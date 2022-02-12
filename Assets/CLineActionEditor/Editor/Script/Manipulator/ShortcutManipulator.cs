/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ShortcutManipulator.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-11-10      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEngine;

    internal sealed class ShortcutManipulator : Manipulator
    {
        protected override bool KeyDown(Event evt, ActionWindow window)
        {
            bool keydown = true;
            switch (evt.keyCode)
            {
                case KeyCode.Alpha1:
                    window.PreviewPlay();
                    break;
                case KeyCode.Alpha2:
                    window.PreviewStop();
                    break;
                case KeyCode.Alpha3:
                    window.PreviewStepBack();
                    break;
                case KeyCode.Alpha4:
                    window.PreviewStepForward();
                    break;
                default:
                    keydown = false;
                    break;
            }

            return keydown;
        }
    }
}