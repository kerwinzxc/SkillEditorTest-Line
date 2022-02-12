/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowDuration.cs
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

    internal sealed partial class ActionWindow
    {
        private ActorTimeItem timelineDuration;

        private void DrawDuration(EAreaType area, float duration)
        {
            bool headerMode = area == EAreaType.Header;

            if (timelineDuration == null || timelineDuration.style != editorResources.endmarker)
            {
                timelineDuration = new ActorTimeItem(editorResources.endmarker, OnkDragDuration)
                {
                    tooltip = L10n.Tr("End of sequence marker"),
                    boundOffset = new Vector2(0.0f, -5f)
                };
            }

            DrawDuration(headerMode, !headerMode, duration);
        }

        private void DrawDuration(bool drawhead, bool drawline, float duration)
        {
            Color lineColor = editorResources.colorEndmarker;
            Color headColor = Color.white;

            bool canMoveHead = !EditorApplication.isPlaying;
            if (canMoveHead)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (timelineDuration.bounds.Contains(Event.current.mousePosition))
                    {
                        if (playHead != null && playHead.bounds.Contains(Event.current.mousePosition))
                        {
                            canMoveHead = false;
                        }
                    }
                }
            }
            else
            {
                lineColor.a *= 0.66f;
                headColor = editorResources.colorDuration;
            }

            if (canMoveHead)
                timelineDuration.HandleManipulatorsEvents(this, Event.current);

            timelineDuration.lineColor = lineColor;
            timelineDuration.headColor = headColor;
            timelineDuration.drawHead = drawhead;
            timelineDuration.drawLine = drawline;
            timelineDuration.canMoveHead = canMoveHead;
            timelineDuration.Draw(duration);

            if (drawhead)
            {
                float width = Time2Pos(duration);
                using (new GUIGroupScope(rectTimeRuler))
                {
                    Rect rc = new Rect(0, timeRulerHeight - 5, width, 5);
                    EditorGUI.DrawRect(rc, editorResources.colorDurationLine);
                }
            }
        }

        private void OnkDragDuration(float newTime)
        {
            length = newTime;
            timelineDuration.showTooltip = true;
        }
    }
}