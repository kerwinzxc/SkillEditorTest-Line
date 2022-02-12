/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\Manipulator.cs
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

    internal abstract class Manipulator
    {
        private int controlId = 0;

        protected virtual bool MouseDown(Event evt, ActionWindow window) { return false; }
        protected virtual bool MouseDrag(Event evt, ActionWindow window) { return false; }
        protected virtual bool MouseUp(Event evt, ActionWindow window) { return false; }
        protected virtual bool MouseWheel(Event evt, ActionWindow window) { return false; }
        protected virtual bool DoubleClick(Event evt, ActionWindow window) { return false; }
        protected virtual bool KeyDown(Event evt, ActionWindow window) { return false; }
        protected virtual bool KeyUp(Event evt, ActionWindow window) { return false; }
        protected virtual bool ContextClick(Event evt, ActionWindow window) { return false; }
        protected virtual bool ValidateCommand(Event evt, ActionWindow window) { return false; }
        protected virtual bool ExecuteCommand(Event evt, ActionWindow window) { return false; }
        public virtual void Overlay(Event evt, ActionWindow window) { }
        public bool HandleEvent(Event evt, ActionWindow window)
        {
            controlId = EditorGUIUtility.GetControlID(FocusType.Passive);

            bool isHandled = false;

            switch (evt.type)
            {
                case EventType.ScrollWheel:
                    isHandled = MouseWheel(evt, window);
                    break;

                case EventType.MouseUp:
                    {
                        if (EditorGUIUtility.hotControl == controlId)
                        {
                            isHandled = MouseUp(evt, window);

                            GUIUtility.hotControl = 0;
                            evt.Use();
                        }
                    }
                    break;

                case EventType.MouseDown:
                    {
                        isHandled = evt.clickCount < 2 ? MouseDown(evt, window) : DoubleClick(evt, window);

                        if (isHandled)
                            GUIUtility.hotControl = controlId;
                    }
                    break;

                case EventType.MouseDrag:
                    {
                        if (GUIUtility.hotControl == controlId)
                            isHandled = MouseDrag(evt, window);
                    }
                    break;

                case EventType.KeyDown:
                    isHandled = KeyDown(evt, window);
                    break;

                case EventType.KeyUp:
                    isHandled = KeyUp(evt, window);
                    break;

                case EventType.ContextClick:
                    isHandled = ContextClick(evt, window);
                    break;

                case EventType.ValidateCommand:
                    isHandled = ValidateCommand(evt, window);
                    break;

                case EventType.ExecuteCommand:
                    isHandled = ExecuteCommand(evt, window);
                    break;
            }

            if (isHandled)
            {
                evt.Use();
            }

            return isHandled;
        }
    }
}