/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ActorTrackManipulator.cs
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
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    internal class ActorTrackManipulator : Manipulator
    {
        private readonly float buttonSize = 16f;
        private readonly float dragHeight = 3f;
        private ActorTreeItem owner = null;
        private ActorTreeItem dragItem = null;

        public ActorTrackManipulator(ActorTreeItem owner)
        {
            this.owner = owner;
        }

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (evt.alt || evt.button != 0)
                return false;

            var trackActorList = owner.BuildRows();
            var selectable = new List<ActorTreeItem>();
            using (var itr = trackActorList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var rect = itr.Current.manipulatorRect;
                    rect.x = 0;
                    rect.width = window.headerWidth - buttonSize;
                    //rect.width += window.rectContent.width;
                    var pos = window.MousePos2ViewPos(evt.mousePosition);
                    if (rect.Contains(pos))
                    {
                        selectable.Add(itr.Current);
                    }
                }
            }

            var item = selectable.OrderByDescending(x => x.depth).FirstOrDefault();
            if (item != null)
            {
                Actor curActor = window.GetActorTrack();
                if (evt.modifiers == Helper.actionModifier)
                {
                    if (curActor != null)
                    {
                        if (curActor != item)
                        {
                            if (window.HasSelect(item))
                            {
                                window.Deselect(item);
                            }
                            else if (curActor.GetActorType() == item.GetActorType())
                            {
                                window.SelectMore(item);
                            }
                        }
                        else
                        {
                            window.Deselect(item);
                        }
                    }
                    else
                    {
                        window.Select(item);
                    }
                }
                else
                {
                    if (!window.HasSelect(item))
                    {
                        window.Select(item);
                    }
                }
                return true;
            }

            window.DeselectAllTrack();
            window.Repaint();

            return false;
        }

        protected override bool MouseDrag(Event evt, ActionWindow window)
        {
            window.RemoveCaptured(this);

            if (evt.button != 0)
                return false;

            if (evt.mousePosition.x > window.headerWidth)
                return false;

            var trackActorList = owner.BuildRows();
            using (var itr = trackActorList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current is ActorTrack)
                    {
                        var rect = itr.Current.manipulatorRect;
                        rect.width += window.rectContent.width;
                        var pos = window.MousePos2ViewPos(evt.mousePosition);
                        if (rect.Contains(pos))
                        {
                            dragItem = itr.Current;
                            break;
                        }
                    }
                }
            }

            if (dragItem == null || window.HasSelect(dragItem))
            {
                dragItem = null;
                return false;
            }

            Actor curActor = window.GetActorTrack();
            if (dragItem.GetActorType() != curActor.GetActorType())
            {
                dragItem = null;
                return false;
            }

            window.AddCaptured(this);

            return true;
        }

        protected override bool MouseUp(Event evt, ActionWindow window)
        {
            if (dragItem != null)
            {
                ActorTreeItem parent = dragItem.GetParent();
                using (var itr = window.selectionList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        parent.children.Remove(itr.Current as ActorTreeItem);
                    }
                }
                int idx = parent.children.IndexOf(dragItem);
                using (var itr = window.selectionList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        parent.children.Insert(idx, itr.Current as ActorTreeItem);
                    }
                }

                dragItem = null;
                window.BuildTrackIndex(parent);
            }
            window.RemoveCaptured(this);

            return false;
        }

        public override void Overlay(Event evt, ActionWindow window)
        {
            if (dragItem != null)
            {
                var rc = dragItem.manipulatorRect;
                rc.y -= dragHeight;
                rc.y += (-window.scrollPosition.y + window.rectClient.y);
                rc.width += window.rectContent.width;
                rc.height = dragHeight;

                using (new GUIColorScope(window.editorResources.colorRed))
                {
                    GUI.DrawTexture(rc, EditorGUIUtility.whiteTexture);
                }
                EditorGUIUtility.AddCursorRect(window.rectHeader, MouseCursor.MoveArrow);
            }
        }
    }

}