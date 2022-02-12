/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorGroup.cs
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

    internal class ActorGroup : ActorTreeItem
    {
        public override void Init(ActorTreeItem parent)
        {
            base.Init(parent);

            parent.AddChild(this);
            expand = true;
        }

        public override string GetActorType()
        {
            return this.GetType().ToString();
        }

        public override void BuildEventHandles(ref List<ActorEventHandle> list)
        {
            if (expand)
            {
                using (var itr = children.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        itr.Current.BuildEventHandles(ref list);
                    }
                }
            }
        }

        public override List<ActorEvent> BuildEvents()
        {
            List<ActorEvent> list = new List<ActorEvent>();

            if (expand)
            {
                using (var itr = children.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        var l = itr.Current.BuildEvents();
                        list.AddRange(l);
                    }
                }
            }
            return list;
        }

        public override List<ActorTreeItem> BuildRows()
        {
            List<ActorTreeItem> list = new List<ActorTreeItem>();
            list.Add(this);

            if (expand)
            {
                using (var itr = children.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        var l = itr.Current.BuildRows();
                        list.AddRange(l);
                    }
                }
            }

            return list;
        }

        public override void BuildRect(float h)
        {
            rect.x = depth * indent;
            rect.y = h + padding;
            rect.width = window.headerWidth - rect.x - 3;// headerSplitterWidth/2
            rect.height = height;
            if (expand)
            {
                using (var itr = children.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        rect.height += itr.Current.totalHeight;
                    }
                }
            }
        }

        public override void Draw()
        {
            var selected = window.HasSelect(this);
            var foldoutRect = rect;
            foldoutRect.width = indent;
            foldoutRect.height = indent;
            expand = EditorGUI.Foldout(foldoutRect, expand, GUIContent.none, EditorStyles.foldout);

            // header
            var headerRect = rect;
            headerRect.x += indent;
            headerRect.width -= indent;

            var color = selected ? window.editorResources.colorSelection : window.editorResources.colorGroup;
            using (new GUIColorScope(color))
            {
                GUI.Box(headerRect, GUIContent.none, window.editorResources.groupBackground);
            }

            // group name
            const float buttonSize = 16f;
            var labelRect = rect;
            labelRect.x += indent;
            var size = EditorStyles.whiteLargeLabel.CalcSize(new GUIContent(itemName));
            labelRect.y += (minHeight - size.y) * 0.5f;
            labelRect.width = window.headerWidth - labelRect.x - buttonSize;
            EditorGUI.LabelField(labelRect, itemName, EditorStyles.whiteLargeLabel);

            // button create
            var buttonRect = rect;
            buttonRect.x = window.headerWidth - buttonSize - padding;
            buttonRect.y += (minHeight - buttonSize) * 0.5f;
            buttonRect.width = buttonSize;
            buttonRect.height = buttonSize;
            if (GUI.Button(buttonRect, window.editorResources.createAddNew, window.editorResources.trackGroupAddButton))
            {
                window.ShowContextMenu(this, null);
            }

            // content
            var contentRect = rect;
            contentRect.x = window.headerWidth;
            contentRect.width = window.rectContent.width;

            color = selected ? window.editorResources.colorTrackBackgroundSelected : window.editorResources.colorGroupTrackBackground;
            EditorGUI.DrawRect(contentRect, color);
            if (!expand && children != null && children.Count > 0)
            {
                contentRect.y += padding;
                contentRect.height -= padding * 2;
                EditorGUI.DrawRect(contentRect, window.editorResources.colorClipUnion);
            }

            // child
            if (expand)
            {
                using (var itr = children.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        itr.Current.Draw();
                    }
                }
            }
        }

    }
}