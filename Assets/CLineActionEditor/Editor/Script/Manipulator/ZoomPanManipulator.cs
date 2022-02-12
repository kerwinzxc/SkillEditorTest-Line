/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ZoomPanManipulator.cs
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
    using UnityEditor;
    using UnityEngine;

    internal class ZoomPanManipulator : Manipulator
    {
        private bool captured = false;

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (!window.rectTimeArea.Contains(evt.mousePosition))
            {
                return false;
            }

            if ((evt.button == 2 && evt.modifiers == EventModifiers.None) ||
                (evt.button == 0 && evt.modifiers == EventModifiers.Alt))
            {
                captured = true;
                window.AddCaptured(this);
                return true;
            }

            return false;
        }

        protected override bool MouseDrag(Event evt, ActionWindow window)
        {
            if (!captured)
                return false;

            window.Pan(evt);

            return true;
        }

        protected override bool MouseUp(Event evt, ActionWindow window)
        {
            if (!captured)
                return false;

            window.RemoveCaptured(this);
            captured = false;

            return true;
        }

        protected override bool MouseWheel(Event evt, ActionWindow window)
        {
            if (window.rectTimeArea.Contains(evt.mousePosition))
            {
                window.Zoom(evt);
                return true;
            }

            return false;
        }

        public override void Overlay(Event evt, ActionWindow window)
        {
            if (captured)
            {
                EditorGUIUtility.AddCursorRect(window.rectTimeArea, MouseCursor.Pan);
            }
        }

    }

}