/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Common\Tooltip.cs
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

    public class Tooltip
    {
        public GUIStyle style
        {
            get;
            set;
        }
        public string text
        {
            get;
            set;
        }

        private GUIStyle _font;
        public GUIStyle font
        {
            get
            {
                if (_font != null)
                    return _font;

                if (style != null)
                    return style;

                _font = new GUIStyle();
                _font.font = EditorStyles.label.font;

                return _font;
            }
            set { _font = value; }
        }

        private float _pad = 4.0f;
        public float pad
        {
            get { return _pad; }
            set { _pad = value; }
        }

        private GUIContent _textContent;
        GUIContent textContent
        {
            get
            {
                if (_textContent == null)
                    _textContent = new GUIContent();

                _textContent.text = text;

                return _textContent;
            }
        }

        private Color _foreColor = Color.white;
        public Color foreColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        private Rect _bounds;
        public Rect bounds
        {
            get
            {
                var size = font.CalcSize(textContent);
                _bounds.width = size.x + (2.0f * pad);
                _bounds.height = size.y + 2.0f;

                return _bounds;
            }

            set { _bounds = value; }
        }

        public Tooltip(GUIStyle theStyle, GUIStyle font)
        {
            style = theStyle;
            _font = font;
        }

        public Tooltip()
        {
            style = null;
            _font = null;
        }

        public void Draw()
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (style != null)
            {
                using (new GUIColorScope(ActionWindow.instance.editorResources.colorTooltipBackground))
                    GUI.Label(bounds, GUIContent.none, style);
            }

            var textBounds = bounds;
            textBounds.x += pad;
            textBounds.width -= pad;

            using (new GUIColorScope(foreColor))
                GUI.Label(textBounds, textContent, font);
        }
    }

}