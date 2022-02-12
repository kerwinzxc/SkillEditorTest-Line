/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ActorSplitterManipulator.cs
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

    internal class ActorSplitterManipulator : Manipulator
    {
        private bool captured = false;
        private Actor owner = null;

        public ActorSplitterManipulator(Actor owner)
        {
            this.owner = owner;
        }

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (owner.manipulatorRect.Contains(evt.mousePosition))
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

            window.headerWidth = evt.mousePosition.x;

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

        public override void Overlay(Event evt, ActionWindow window)
        {
            Rect rect = window.rectWindow;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeLeftRight);
        }
    }

}