/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowTimeline.cs
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
    using System.Collections.Generic;

    [System.Serializable]
    public enum TimeFormat
    {
        Seconds,
        Frames
    }

    internal sealed partial class ActionWindow
    {
        public bool enableSnap = true;

        private float _length = 16f;
        public float length
        {
            get
            {
                if (actorTreeViewAction != null)
                {
                    return ToSecond(actorTreeViewAction.TotalTime);
                }
                return _length;
            }
            set
            {
                _length = Mathf.Max(value, 0.1f);
                if (actorTreeViewAction != null)
                {
                    actorTreeViewAction.TotalTime = ToMillisecond(_length);
                }
            }
        }

        private float _viewTimeMin = 0f;
        public float viewTimeMin
        {
            get { return _viewTimeMin; }
            set { _viewTimeMin = Mathf.Min(value, viewTimeMax - 0.25f); }
        }

        private float _viewTimeMax = 16f;
        public float viewTimeMax
        {
            get { return _viewTimeMax; }
            set { _viewTimeMax = Mathf.Max(value, viewTimeMin + 0.25f, 0); }
        }
        public float viewTime
        {
            get { return viewTimeMax - viewTimeMin; }
        }
        public float maxTime
        {
            get { return Mathf.Max(viewTimeMax, length); }
        }

        private float _currentTime = 0f;
        public float currentTime
        {
            get { return _currentTime; }
            set { _currentTime = Mathf.Clamp(value, 0, length); }
        }

        private TimeFormat _timeFormat;
        public TimeFormat timeFormat
        {
            get { return _timeFormat; }
            set
            {
                if (_timeFormat != value)
                {
                    _timeFormat = value;
                    frameRate = value == TimeFormat.Frames ? 60 : 100;
                }
            }
        }

        private int _frameRate;
        public int frameRate
        {
            get { return _frameRate; }
            set { if (_frameRate != value) { _frameRate = value; snapInterval = 1f / value; } }
        }

        private float _snapInterval;
        public float snapInterval
        {
            get { return _snapInterval; }
            set { if (_snapInterval != value) { _snapInterval = Mathf.Max(value, 0.001f); } }
        }
        private float _frameWidth;
        public float frameWidth
        {
            get { return _frameWidth; }
            set { _frameWidth = value; }
        }

        public float SnapTime(float time)
        {
            if (Event.current.shift || !enableSnap)
            {
                return time;
            }
            return SnapTime2(time);
        }

        public float SnapTime2(float time)
        {
            return Mathf.Round(time / snapInterval) * snapInterval;
        }

        public float SnapTime3(float time)
        {
            return enableSnap ? SnapTime2(time) : time;
        }

        public float Time2Pos(float time)
        {
            return (time - viewTimeMin) / viewTime * rectTimeArea.width;
        }
        public float Pos2Time(float pos)
        {
            return (pos - headerWidth - timelineOffsetX) / rectTimeArea.width * viewTime + viewTimeMin;
        }

        public Vector2 MousePos2ViewPos(Vector2 mousePosition)
        {
            Vector2 pos = mousePosition;
            pos.y -= rectClient.y;
            pos.y += scrollPosition.y;

            return pos;
        }

        [System.NonSerialized] private float timeInfoStart;
        [System.NonSerialized] private float timeInfoEnd;
        [System.NonSerialized] private float timeInfoInterval;

        private List<float> modulos = new List<float>()
        {
                0.0000001f, 0.0000005f, 0.000001f, 0.000005f, 0.00001f, 0.00005f, 0.0001f, 0.0005f,
                0.001f, 0.005f, 0.01f, 0.05f, 0.1f, 0.5f, 1, 5, 10, 50, 100, 500,
                1000, 5000, 10000, 50000, 100000, 500000, 1000000, 5000000, 10000000
        };

        private void DrawTimeline()
        {
            frameWidth = rectTimeArea.width / viewTime * snapInterval;

            //range bar
            var _timeMin = viewTimeMin;
            var _timeMax = viewTimeMax;
            rectRangebar = new Rect(headerWidth, position.height - horizontalScrollbarHeight - 2, rectContent.width - 2 * verticalScrollbarWidth, horizontalScrollbarHeight);
            EditorGUI.MinMaxSlider(rectRangebar, ref _timeMin, ref _timeMax, 0, maxTime);
            viewTimeMin = _timeMin;
            viewTimeMax = _timeMax;
            if (rectRangebar.Contains(Event.current.mousePosition) && Event.current.clickCount == 2)
            {
                viewTimeMin = 0;
                viewTimeMax = length;
            }

            //time build
            timeInfoInterval = 1000f;
            for (var i = 0; i < modulos.Count; i++)
            {
                var count = viewTime / modulos[i];
                if (rectTimeArea.width / count > 50)
                {
                    timeInfoInterval = modulos[i];
                    break;
                }
            }

            timeInfoInterval = Mathf.RoundToInt(timeInfoInterval / snapInterval) * snapInterval;
            timeInfoStart = (float)Mathf.FloorToInt(viewTimeMin / timeInfoInterval) * timeInfoInterval;
            timeInfoEnd = (float)Mathf.CeilToInt(viewTimeMax / timeInfoInterval) * timeInfoInterval;
            timeInfoStart = Mathf.Round(timeInfoStart * 10000) / 10000;
            timeInfoEnd = Mathf.Round(timeInfoEnd * 10000) / 10000;

            //time ruler step
            using (new GUIGroupScope(rectTimeRuler))
            {
                if (frameWidth > 10)
                {
                    for (var i = timeInfoStart; i <= timeInfoEnd; i += snapInterval)
                    {
                        var posX = Time2Pos(i);
                        Vector3 start = new Vector3(posX, timeRulerHeight, 0);
                        Vector3 end = new Vector3(start.x, timeRulerHeight - 5, 0);

                        Handles.color = editorResources.colorWhite;
                        Handles.DrawLine(start, end);
                    }
                }
            }

            //time ruler interval
            using (new GUIGroupScope(rectTimeline))
            {
                float timeInterval = GetTimeInterval(timeInfoInterval);
                for (var i = 0f; i <= timeInfoEnd; i += timeInterval)
                {
                    if (i < timeInfoStart)
                        continue;

                    var posX = Time2Pos(i);
                    var rounded = Mathf.Round(i * 10000) / 10000;

                    Vector3 p1 = new Vector3(posX, timeRulerHeight - 13, 0);
                    Vector3 p2 = new Vector3(p1.x, timeRulerHeight, 0);
                    Vector3 p3 = new Vector3(p1.x, rectTimeline.height, 0);

                    Handles.color = editorResources.colorWhite;
                    Handles.DrawLine(p1, p2);
                    Handles.color = editorResources.colorTimeline;
                    Handles.DrawLine(p2, p3);

                    var text = timeFormat == TimeFormat.Frames ? (rounded * frameRate).ToString("0") : rounded.ToString("0.00");
                    var size = editorResources.labelStyle.CalcSize(new GUIContent(text));
                    var stampRect = new Rect(posX + 2, 0, size.x, size.y);
                    GUI.Box(stampRect, text, editorResources.labelStyle);
                }
            }

        }

        public string FormatTime(float time)
        {
            var rounded = Mathf.Round(time * 10000) / 10000;
            var frame = Mathf.RoundToInt(time * frameRate);
            return timeFormat == TimeFormat.Frames ? frame.ToString("0") : rounded.ToString("0.00");
        }

        private float GetTimeInterval(float modulo)
        {
            float timeInterval = modulo;
            if (modulo <= 5 * snapInterval)
            {
                timeInterval = 5 * snapInterval;
            }
            else if (modulo <= 10 * snapInterval)
            {
                timeInterval = 10 * snapInterval;
            }
            else if (modulo <= 50 * snapInterval)
            {
                timeInterval = 50 * snapInterval;
            }
            else
            {
                timeInterval = 100 * snapInterval;
            }

            return timeInterval;
        }

        public void Pan(Event e)
        {
            var t = (Mathf.Abs(e.delta.x) / rectTimeArea.width) * viewTime;
            var move = e.delta.x > 0 ? -t : t;
            if (move < 0 && Mathf.Abs(move) > viewTimeMin)
            {
                move = -viewTimeMin;
            }
            viewTimeMax += move;
            viewTimeMin += move;
            scrollPosition.y -= e.delta.y;
        }

        public void Zoom(Event e)
        {
            var pointerTimeA = Pos2Time(e.mousePosition.x);
            var delta = e.alt ? -e.delta.x * 0.1f : e.delta.y;
            var t = (Mathf.Abs(delta * 25) / rectTimeArea.width) * viewTime;
            viewTimeMax += delta > 0 ? t : -t;
            var pointerTimeB = Pos2Time(e.mousePosition.x + e.delta.x);
            var diff = pointerTimeA - pointerTimeB;
            viewTimeMax += diff;
            viewTimeMax = Mathf.Clamp(viewTimeMax, 0.25f, 20f);
        }

        public int Round2Frame(float time)
        {
            return Mathf.RoundToInt(time * frameRate); ;
        }

        public int ToMillisecond(float time)
        {
            return Mathf.RoundToInt(time * 1000);
        }

        public float ToSecond(int time)
        {
            return time * 0.001f;
        }
    }

}