/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\DragBoxSelectManipulator.cs
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

    internal class DragBoxSelectManipulator : Manipulator
    {
        private ActorTreeItem owner = null;
        private bool dragBox = false;
        private Vector2 startPos = Vector2.zero;

        public DragBoxSelectManipulator(ActorTreeItem owner)
        {
            this.owner = owner;
        }

        protected override bool MouseDown(Event evt, ActionWindow window)
        {
            if (evt.alt || evt.button != 0)
                return false;

            if (evt.mousePosition.x <= window.headerWidth ||
                evt.mousePosition.x >= window.rectClient.width - 2 * window.verticalScrollbarWidth ||
                evt.mousePosition.y <= window.rectClient.y ||
                evt.mousePosition.y >= window.rectWindow.height - window.horizontalScrollbarHeight)
                return false;

            return true;
        }

        protected override bool MouseDrag(Event evt, ActionWindow window)
        {
            if (evt.button != 0)
                return false;

            if (evt.mousePosition.x <= window.headerWidth)
                return false;

            if (!dragBox)
            {
                dragBox = true;
                startPos = evt.mousePosition;
                window.AddCaptured(this);
                return true;
            }

            return false;
        }

        protected override bool MouseUp(Event evt, ActionWindow window)
        {
            if (dragBox)
            {
                dragBox = false;
                window.RemoveCaptured(this);
                return true;
            }

            return false;
        }

        public override void Overlay(Event evt, ActionWindow window)
        {
            if (dragBox)
            {
                Rect boxRect = new Rect();
                boxRect.xMin = Mathf.Max(Mathf.Min(startPos.x, evt.mousePosition.x), 0);
                boxRect.xMax = Mathf.Min(Mathf.Max(startPos.x, evt.mousePosition.x), Screen.width / EditorGUIUtility.pixelsPerPoint);
                boxRect.yMin = Mathf.Min(startPos.y, evt.mousePosition.y);
                boxRect.yMax = Mathf.Max(startPos.y, evt.mousePosition.y);

                using (new GUIColorScope(window.editorResources.colorWhite))
                {
                    GUI.Box(boxRect, string.Empty, window.editorResources.customHollowFrame);
                    GUI.color = window.editorResources.colorWhite.WithAlpha(0.05f);
                    GUI.DrawTexture(boxRect, EditorGUIUtility.whiteTexture);
                }

                window.DeselectAll();
                var eventActorList = owner.BuildEvents();
                using (var itr = eventActorList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        var rect = itr.Current.manipulatorRect;
                        var pos = window.MousePos2ViewPos(evt.mousePosition);
                        if (boxRect.Overlaps(rect))
                        {
                            window.SelectMore(itr.Current);
                        }
                    }
                }

            }
        }

    }

}