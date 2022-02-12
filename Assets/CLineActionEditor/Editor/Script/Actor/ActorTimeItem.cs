/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorTimeItem.cs
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

    internal class ActorTimeItem : Actor
    {
        private readonly GUIContent headerContent = new GUIContent();
        private readonly GUIStyle _style;
        private readonly Tooltip _tooltip;
        private Rect boundingRect;

        public Color headColor { get; set; }
        public Color lineColor { get; set; }
        public bool drawLine { get; set; }
        public bool drawHead { get; set; }
        public bool canMoveHead { get; set; }
        public string tooltip { get; set; }
        public Vector2 boundOffset { get; set; }
        public float widgetHeight { get { return _style.fixedHeight; } }
        public float widgetWidth { get { return _style.fixedWidth; } }

        public Rect bounds
        {
            get
            {
                Rect r = boundingRect;
                r.y = window.rectTimeRuler.yMax - widgetHeight;
                r.position += boundOffset;

                return r;
            }
        }

        public GUIStyle style
        {
            get { return _style; }
        }
        public bool showTooltip { get; set; }
        public bool firstDrag { get; private set; }

        public ActorTimeItem(GUIStyle style, System.Action<float> onDrag)
        {
            drawLine = true;
            drawHead = true;
            canMoveHead = false;
            tooltip = string.Empty;
            boundOffset = Vector2.zero;
            headColor = window.editorResources.colorWhite;
            lineColor = style.normal.textColor;
            _style = style;

            _tooltip = new Tooltip(window.editorResources.displayBackground, window.editorResources.tinyFont);

            var scrub = new ActorTimeItemManipulator(
                (evt) =>
                {
                    firstDrag = true;
                    var rc = window.rectTimeRuler;
                    rc.x -= widgetWidth * 0.5f;
                    return rc.Contains(evt.mousePosition) && bounds.Contains(evt.mousePosition);
                },
                (evt) =>
                {
                    firstDrag = false;
                    canMoveHead = true;

                    var t = window.Pos2Time(evt.mousePosition.x);
                    var snapT = window.SnapTime(t);
                    onDrag?.Invoke(snapT);
                },
                (evt) =>
                {
                    canMoveHead = false;
                    showTooltip = false;
                    firstDrag = false;

                    if (window.enableSnap)
                    {
                        var t = window.Pos2Time(evt.mousePosition.x);
                        var snapT = window.SnapTime2(t);
                        window.currentTime = Mathf.Max(0f, snapT);
                    }
                }
            );
            AddManipulator(scrub);
        }

        public void Draw(float time)
        {
            float posX = window.Time2Pos(time) + window.rectTimeArea.x;
            boundingRect = new Rect(posX - widgetWidth * 0.5f, window.rectBody.y, widgetWidth, widgetHeight);

            if (Event.current.type == EventType.Repaint)
            {
                if (boundingRect.xMax < window.rectTimeRuler.xMin)
                    return;
                if (boundingRect.xMin > window.rectTimeRuler.xMax)
                    return;
            }

            if (drawLine)
            {
                Rect lineRect = new Rect(posX - 0.5f, window.rectClient.y, 1, window.rectTimeArea.height);
                EditorGUI.DrawRect(lineRect, lineColor);
            }

            if (drawHead && Event.current.type == EventType.Repaint)
            {
                using (new GUIColorScope(headColor))
                {
                    style.Draw(boundingRect, headerContent, false, false, false, false);
                }

                if (canMoveHead)
                    EditorGUIUtility.AddCursorRect(bounds, MouseCursor.MoveArrow);
            }

            if (showTooltip)
            {
                _tooltip.text = window.FormatTime(time);

                Vector2 position = bounds.position;
                position.y = window.rectTimeRuler.y;
                position.y -= _tooltip.bounds.height;
                position.x -= Mathf.Abs(_tooltip.bounds.width - bounds.width) / 2.0f;

                Rect tooltipBounds = bounds;
                tooltipBounds.position = position;
                _tooltip.bounds = tooltipBounds;

                _tooltip.Draw();
            }

        }

    }

}