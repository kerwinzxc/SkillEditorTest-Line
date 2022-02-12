/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ActorEventHandleManipulator.cs
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
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    internal class ActorEventHandleManipulator : Manipulator
    {
        private ActorTreeItem owner = null;
        private ActorEventHandle dragHandle = null;

        public ActorEventHandleManipulator(ActorTreeItem owner)
        {
            this.owner = owner;
        }

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (evt.alt || evt.button != 0)
                return false;

            if (evt.mousePosition.x <= window.rectTimeArea.x ||
                evt.mousePosition.x >= window.rectClient.width - 2 * window.verticalScrollbarWidth ||
                evt.mousePosition.y <= window.rectClient.y ||
                evt.mousePosition.y >= window.rectWindow.height - window.horizontalScrollbarHeight)
                return false;

            var eventHandleActorList = new List<ActorEventHandle>();
            owner.BuildEventHandles(ref eventHandleActorList);
            using (var itr = eventHandleActorList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var pos = window.MousePos2ViewPos(evt.mousePosition);
                    if (itr.Current.manipulatorRect.Contains(pos))
                    {
                        dragHandle = itr.Current;
                        break;
                    }
                }
            }

            if (dragHandle != null)
            {
                dragHandle.color = window.editorResources.colorEventHandleSelected;
                window.AddCaptured(this);

                return true;
            }

            return false;
        }

        protected override bool MouseDrag(Event evt, ActionWindow window)
        {
            if (dragHandle == null)
                return false;

            dragHandle.Move(evt);

            return true;
        }

        protected override bool MouseUp(Event evt, ActionWindow window)
        {
            if (dragHandle == null)
                return false;

            dragHandle.color = window.editorResources.colorEventHandle;
            dragHandle = null;

            window.RemoveCaptured(this);

            return true;
        }

        public override void Overlay(Event evt, ActionWindow window)
        {
            if (dragHandle != null)
            {
                EditorGUIUtility.AddCursorRect(window.rectTimeArea, MouseCursor.SplitResizeLeftRight);
                using (new GUIGroupScope(window.rectTimeline))
                {
                    TimeIndicator.Draw(window, dragHandle.time);
                }
            }
        }
    }

}