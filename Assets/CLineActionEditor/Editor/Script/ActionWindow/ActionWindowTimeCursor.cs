/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowTimeCursor.cs
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

    internal enum EAreaType
    {
        Header,
        Lines
    }

    internal sealed partial class ActionWindow
    {
        private ActorTimeItem playHead;

        private void DrawTimeCursor(EAreaType area)
        {
            if (playHead == null || playHead.style != editorResources.timeCursor)
            {
                playHead = new ActorTimeItem(editorResources.timeCursor, OnDragTimelineHead);
            }

            var headerMode = area == EAreaType.Header;
            DrawTimeCursor(headerMode, !headerMode);
        }

        private void DrawTimeCursor(bool drawHead, bool drawline)
        {
            playHead.HandleManipulatorsEvents(this, Event.current);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (rectTimeRuler.Contains(Event.current.mousePosition))
                {
                    playHead.HandleManipulatorsEvents(this, Event.current);
                    currentTime = Mathf.Max(0f, SnapTime(Pos2Time(Event.current.mousePosition.x)));
                }
            }

            playHead.drawHead = drawHead;
            playHead.drawLine = drawline;
            playHead.Draw(currentTime);
        }

        private void OnDragTimelineHead(float newTime)
        {
            var t = Mathf.Max(0f, newTime);
            var delta = t - currentTime;
            Preview(delta);
            currentTime = t;
            playHead.showTooltip = true;
        }

    }

}