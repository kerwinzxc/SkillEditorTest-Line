/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ActorTimeItemManipulator.cs
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

    internal class ActorTimeItemManipulator : Manipulator
    {
        private bool captured;
        private Func<Event, bool> onMouseDown;
        private System.Action<Event> onMouseDrag;
        private System.Action<Event> onMouseUp;

        public ActorTimeItemManipulator(Func<Event, bool> onMouseDown, System.Action<Event> onMouseDrag, System.Action<Event> onMouseUp)
        {
            this.onMouseDown = onMouseDown;
            this.onMouseDrag = onMouseDrag;
            this.onMouseUp = onMouseUp;
        }

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (evt.button != 0)
                return false;

            if (!onMouseDown(evt))
                return false;

            window.AddCaptured(this);
            captured = true;

            return true;
        }

        protected override bool MouseUp(Event evt, ActionWindow window)
        {
            if (!captured)
                return false;

            captured = false;
            window.RemoveCaptured(this);

            onMouseUp(evt);

            return true;
        }

        protected override bool MouseDrag(Event evt, ActionWindow window)
        {
            if (!captured)
                return false;

            onMouseDrag(evt);

            return true;
        }
    }

}