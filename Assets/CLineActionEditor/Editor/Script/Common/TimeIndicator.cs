/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\TimeIndicator.cs
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
    using UnityEditor;

    internal static class TimeIndicator
    {
        private static readonly Tooltip tooltip = new Tooltip(ActionWindow.instance.editorResources.displayBackground, ActionWindow.instance.editorResources.tinyFont);
        private static float width = 11f;

        public static void Draw(ActionWindow window, float time)
        {
            tooltip.text = window.FormatTime(time);

            float posX = window.Time2Pos(time);
            var bounds = new Rect(posX, 0, width, width);

            var tooltipBounds = tooltip.bounds;
            tooltipBounds.xMin = bounds.xMin - (tooltipBounds.width / 2.0f);
            tooltipBounds.y = bounds.y;
            tooltip.bounds = tooltipBounds;

            if (time >= 0)
            {
                tooltip.Draw();
                DrawLineAtTime(window, time, Color.black, true);
            }
        }

        public static void Draw(ActionWindow window, float start, float end)
        {
            Draw(window, start);
            Draw(window, end);
        }

        public static void DrawLineAtTime(ActionWindow window, float time, Color color, bool dotted = false)
        {
            var posX = window.Time2Pos(time);

            var p0 = new Vector3(posX, 0);
            var p1 = new Vector3(posX, window.rectTimeArea.height);

            if (dotted)
                DrawDottedLine(p0, p1, color);
            else
                DrawLine(p0, p1, color);
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            var c = Handles.color;
            Handles.color = color;
            Handles.DrawLine(p1, p2);
            Handles.color = c;
        }

        public static void DrawDottedLine(Vector3 p1, Vector3 p2, Color color)
        {
            var c = Handles.color;
            Handles.color = color;
            Handles.DrawDottedLine(p1, p2, 2);
            Handles.color = c;
        }

    }
}