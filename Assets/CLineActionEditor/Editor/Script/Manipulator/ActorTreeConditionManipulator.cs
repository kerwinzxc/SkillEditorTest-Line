/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ActorTreeConditionManipulator.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-11-2      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    internal class ActorTreeConditionManipulator : Manipulator
    {
        private readonly float dragHeight = 3f;

        private ActorTreeProperty owner = null;
        private ActorTreeProperty selectable = null;
        private ActorTreeProperty dragable = null;

        public ActorTreeConditionManipulator(ActorTreeProperty owner)
        {
            this.owner = owner;
        }

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (evt.alt || evt.control || evt.shift || evt.button != 0)
                return false;

            var pos = window.MousePos2InspectorPos(evt.mousePosition);
            using (var itr = owner.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.manipulatorRect.Contains(pos))
                    {
                        selectable = itr.Current;
                        break;
                    }
                }
            }

            if (selectable != null)
            {
                window.SelectCondition(selectable);
                window.AddCaptured(this);

                return true;
            }

            return false;
        }

        protected override bool MouseDrag(Event evt, ActionWindow window)
        {
            if (selectable == null)
                return false;

            var pos = window.MousePos2InspectorPos(evt.mousePosition);
            using (var itr = owner.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.manipulatorRect.Contains(pos))
                    {
                        dragable = itr.Current;
                        break;
                    }
                }
            }

            if (dragable == null || dragable == selectable || dragable.parent != selectable.parent)
            {
                dragable = null;
                return false;
            }

            return true;
        }

        protected override bool MouseUp(Event evt, ActionWindow window)
        {
            if (dragable != null)
            {
                int selectIdx = owner.children.IndexOf(selectable);
                owner.children.Remove(selectable);

                int idx = owner.children.IndexOf(dragable);
                if (idx != -1)
                {
                    owner.children.Insert(idx, selectable);
                }
                else
                {
                    owner.children.Insert(selectIdx, selectable);
                }
            }

            dragable = null;
            selectable = null;
            window.RemoveCaptured(this);

            return false;
        }

        public override void Overlay(Event evt, ActionWindow window)
        {
            if (dragable != null)
            {
                var rc = dragable.manipulatorRect;
                rc.x += window.rectInspectorRight.x;
                rc.y += (window.toobarHeight + window.timeRulerHeight - window.rightScrollPos.y + 28);
                rc.y -= (dragHeight + window.propertyHeight);
                rc.height = dragHeight;

                using (new GUIColorScope(window.editorResources.colorRed))
                {
                    GUI.DrawTexture(rc, EditorGUIUtility.whiteTexture);
                }
                EditorGUIUtility.AddCursorRect(owner.manipulatorRect, MouseCursor.MoveArrow);
            }
        }

    }

}