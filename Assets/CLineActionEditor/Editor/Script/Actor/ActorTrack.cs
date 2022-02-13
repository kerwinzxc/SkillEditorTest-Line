/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorTrack.cs
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
    using UnityEngine;
    using UnityEditor;

    internal class ActorTrack : ActorTreeItem
    {
        [SerializeField] private List<ActorEvent> _eventList = new List<ActorEvent>();
        public List<ActorEvent> eventList
        {
            get { return _eventList; }
        }

        [SerializeField] private string _type = string.Empty;
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        [SerializeField] private bool _nameReadOnly = true;
        public bool nameReadOnly
        {
            get { return _nameReadOnly; }
            set { _nameReadOnly = value; }
        }

        [SerializeField] private Color mColorSwatch;
        public Color colorSwatch
        {
            get { return mColorSwatch; }
            set { mColorSwatch = value; }
        }

        [SerializeField] private GUIContent mIcon;
        public GUIContent icon
        {
            get { return mIcon; }
            set { mIcon = value; }
        }

        public override void Init(ActorTreeItem parent)
        {
            base.Init(parent);

            parent.AddChild(this);
            nameReadOnly = false;
        }

        //CreateEventSignal
        //CreateEventDuration
        public void AddEvent(ActorEvent evt)
        {
            _eventList.Add(evt);
        }

        public void RemoveEvent(ActorEvent evt)
        {
            evt.Destroy();
            _eventList.Remove(evt);
        }

        public void RemoveAllEvent()
        {
            using (var itr = eventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Destroy();
                }
            }
            eventList.Clear();
        }

        public override string GetActorType()
        {
            return type;
        }

        public override void BuildEventHandles(ref List<ActorEventHandle> list)
        {
            using (var itr = _eventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.BuildEventHandles(ref list);
                }
            }
        }

        public override List<ActorEvent> BuildEvents()
        {
            return _eventList;
        }

        public override List<ActorTreeItem> BuildRows()
        {
            List<ActorTreeItem> list = new List<ActorTreeItem>();
            list.Add(this);
            return list;
        }

        public override void BuildRect(float h)
        {
            rect.x = depth * indent;
            rect.y = h + padding;
            rect.width = window.headerWidth - rect.x - 3;// headerSplitterWidth/2
            rect.height = height;
        }

        public override void Draw()
        {
            var selected = window.HasSelect(this);

            // track color kind - swatch
            using (new GUIColorScope(colorSwatch))
            {
                var swatchRect = rect;
                swatchRect.x += indent;
                swatchRect.width = window.editorResources.trackSwatchStyle.fixedWidth;
                GUI.Label(swatchRect, GUIContent.none, window.editorResources.trackSwatchStyle);
            }

            // header background
            var backgroundColor = selected ? window.editorResources.colorSelection : window.editorResources.colorTrackHeaderBackground;
            var backgroundRect = rect;
            backgroundRect.x += (indent + window.editorResources.trackSwatchStyle.fixedWidth);
            backgroundRect.width -= (indent + window.editorResources.trackSwatchStyle.fixedWidth);
            EditorGUI.DrawRect(backgroundRect, backgroundColor);

            // track icon
            const float buttonSize = 16f;
            var iconRect = rect;
            iconRect.width = buttonSize;
            iconRect.x += (indent + window.editorResources.trackSwatchStyle.fixedWidth + padding);
            iconRect.y += (iconRect.height - iconRect.width) * 0.5f;
            GUI.Box(iconRect, icon, GUIStyle.none);

            // track name
            var labelRect = rect;
            labelRect.x += (indent + window.editorResources.trackSwatchStyle.fixedWidth + padding + buttonSize + padding);
            var size = window.editorResources.groupFont.CalcSize(new GUIContent(itemName));
            labelRect.width = Mathf.Max(50, Mathf.Min(size.x, window.headerWidth-2*buttonSize- labelRect.x));
            if (nameReadOnly)
            {
                EditorGUI.LabelField(labelRect, itemName, window.editorResources.groupFont);
            }
            else
            {
                var textColor = selected ? window.editorResources.colorWhite : window.editorResources.groupFont.normal.textColor;
                string newName;
                EditorGUI.BeginChangeCheck();
                using (new GUIStyleScope(window.editorResources.groupFont, textColor))
                {
                    newName = EditorGUI.DelayedTextField(labelRect, itemName, window.editorResources.groupFont);
                }

                if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(newName))
                {
                    itemName = newName;
                    foreach (var actorEvent in _eventList)
                    {
                        actorEvent.eventProperty.TrackName = itemName;
                    }
                }
            }

            // track context menu
            var buttonRect = rect;
            buttonRect.x = window.headerWidth - buttonSize - padding;
            buttonRect.y += (minHeight - buttonSize) * 0.5f;
            buttonRect.width = buttonSize;
            buttonRect.height = buttonSize;
            if (GUI.Button(buttonRect, GUIContent.none, window.editorResources.trackOptions))
            {
                window.ShowContextMenu(this, null);
            }

            // track content
            var colorContent = selected ? window.editorResources.colorTrackBackgroundSelected : window.editorResources.colorTrackBackground;
            var contentRect = rect;
            contentRect.x = window.headerWidth;
            contentRect.width = window.rectContent.width;
            EditorGUI.DrawRect(contentRect, colorContent);

            // child
            using (var itr = children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Draw();
                }
            }

            // event
            using (var itr = _eventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Draw();
                }
            }
        }


    }

}