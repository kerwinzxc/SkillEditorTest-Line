/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorTreeItem.cs
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

    internal class ActorTreeItem : Actor
    {
        [SerializeField] protected bool expand = false;
        [SerializeField] protected float indent = 15f;
        [SerializeField] protected float padding = 5f;
        [SerializeField] protected float itemHeight = 32f;

        [SerializeField] protected ActorTreeItem parent = null;
        [SerializeField] protected List<ActorTreeItem> _children = new List<ActorTreeItem>();
        public List<ActorTreeItem> children
        {
            get { return _children; }
            set { _children = value; }
        }

        [SerializeField] protected string _itemName = string.Empty;
        public string itemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        [SerializeField] protected int _depth = -1;
        public int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public float minHeight
        {
            get { return 32f; }
        }

        public float maxHeight
        {
            get { return 128f; }
        }

        public float height
        {
            get { return itemHeight; }
            set { itemHeight = Mathf.Clamp(value, minHeight, maxHeight); }
        }

        public float totalHeight
        {
            get { return height + padding; }
        }

        public int childCount
        {
            get { return children.Count; }
        }

        public virtual void Init(ActorTreeItem parent)
        {
            this.parent = parent;

            depth = parent != null ? parent.depth + 1 : -1;
        }

        public ActorTreeItem GetParent()
        {
            return parent;
        }

        public void AddChild(ActorTreeItem actor)
        {
            children.Add(actor);
        }

        public void RemoveChild(ActorTreeItem actor)
        {
            actor.parent = null;
            children.Remove(actor);
        }

        public virtual void BuildEventHandles(ref List<ActorEventHandle> list)
        {
            using (var itr = children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.BuildEventHandles(ref list);
                }
            }
        }

        public virtual List<ActorEvent> BuildEvents()
        {
            List<ActorEvent> list = new List<ActorEvent>();
            using (var itr = children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var l = itr.Current.BuildEvents();
                    list.AddRange(l);
                }
            }
            return list;
        }

        public virtual List<ActorTreeItem> BuildRows()
        {
            List<ActorTreeItem> list = new List<ActorTreeItem>();
            using (var itr = children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var l = itr.Current.BuildRows();
                    list.AddRange(l);
                }
            }
            return list;
        }

        public virtual void BuildRect(float h)
        {

        }

        public override void Draw()
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