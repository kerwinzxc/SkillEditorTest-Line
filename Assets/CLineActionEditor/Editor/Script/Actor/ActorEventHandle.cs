/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorEventHandle.cs
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

    public enum EEventHandleType
    {
        Left = 0,
        Right,
    }

    internal class ActorEventHandle : Actor
    {
        [SerializeField] private ActorEvent _parent = null;
        public ActorEvent parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        [SerializeField] private float _time;
        public float time
        {
            get { return _time; }
            set { _time = value; }
        }
        [SerializeField] private EEventHandleType _handleType = EEventHandleType.Left;
        public EEventHandleType handleType
        {
            get { return _handleType; }
            set { _handleType = value; }
        }
        [SerializeField] private Color _color;
        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }

        public void Init(ActorEvent parent, float time, EEventHandleType handleType)
        {
            this.parent = parent;
            this.handleType = handleType;
            this.time = time;

            color = window.editorResources.colorEventHandle;
        }

        public void BuildRect()
        {
            var x = window.Time2Pos(time) + window.rectTimeArea.x;
            if (handleType == EEventHandleType.Right)
            {
                if (parent.duration == 0)
                {
                    x += Mathf.Max(window.frameWidth - 5, 0);
                }
                else
                {
                    x -= 5;
                }
            }
            rect = new Rect(x, parent.manipulatorRect.y, 5, parent.manipulatorRect.height);
        }

        public override string GetActorType()
        {
            return this.GetType().ToString();
        }

        public override void Draw()
        {
            BuildRect();

            using (new GUIColorScope(color))
            {
                GUI.Box(rect, "", window.editorResources.customEventKey);
            }

            if (GUIUtility.hotControl == 0)
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeLeftRight);
        }

        public void Move(Event evt)
        {
            var posX = Mathf.Max(window.rectTimeArea.x, evt.mousePosition.x);
            var t = window.SnapTime(window.Pos2Time(posX));

            switch (handleType)
            {
                case EEventHandleType.Left:
                    {
                        var limit = parent.end - window.snapInterval / 2;
                        var move = t <= limit ? parent.start - t : 0;
                        parent.start -= move;
                        parent.duration += move;
                        time = parent.start;
                    }
                    break;
                case EEventHandleType.Right:
                    {
                        var limit = parent.start + window.snapInterval / 2;
                        var move = t >= limit ? parent.end - t : 0;
                        parent.duration -= move;
                        time = parent.end;
                    }
                    break;
            }
        }

    }

}