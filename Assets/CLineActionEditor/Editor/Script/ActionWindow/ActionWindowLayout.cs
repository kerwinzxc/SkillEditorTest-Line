/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowLayout.cs
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

    // Layout
    // +--------+----------------+---------------------+
    // |                                               |
    // +--------+----------------+---------------------+
    // |        |                |                     |
    // |        |                |                     |
    // |        |                |                     |
    // | header |   time area    |      inspector      |
    // |        |                |                     |
    // |        |                |                     |
    // |        |                |                     |
    // +--------+----------------+----------+----------+

    internal sealed partial class ActionWindow
    {
        public float toobarHeight = 21f;
        public float timeRulerHeight = 22f;
        public float inspectorWidth = 600f;

        public float horizontalScrollbarHeight = 18f;
        public float verticalScrollbarWidth = 18f;
        public float timelineOffsetX = 6f;

        public float propertyHeight = 22f;

        // header
        private float minHeaderWidth = 195f;
        private float maxHeaderWidth = 500f;

        private float _headerWidth = 225f;
        public float headerWidth
        {
            get { return _headerWidth; }
            set { _headerWidth = Mathf.Clamp(value, minHeaderWidth, maxHeaderWidth); }
        }

        public Rect rectWindow;
        public Rect rectBody;
        public Rect rectTimeArea;
        public Rect rectTimeRuler;
        public Rect rectTimeline;
        public Rect rectContent;
        public Rect rectHeader;

        public Rect rectClient;
        public Rect rectClientView;
        public Rect rectRangebar;

        public Rect rectInspector;
        public Rect rectInspectorLeft;
        public Rect rectInspectorRight;
    }
}