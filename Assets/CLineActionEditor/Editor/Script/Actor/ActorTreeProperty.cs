/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorTreeProperty.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-23      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System.Collections.Generic;
    using UnityEngine;

    internal class ActorTreeProperty : Actor
    {
        [SerializeField] protected ActorTreeProperty _parent = null;
        [SerializeField] protected List<ActorTreeProperty> _children = new List<ActorTreeProperty>();
        [SerializeField] protected string _itemName = string.Empty;

        public ActorTreeProperty parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public List<ActorTreeProperty> children
        {
            get { return _children; }
            set { _children = value; }
        }
        public string itemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public virtual void OnSelected()
        {

        }

        public virtual string GetPropertyType()
        {
            return string.Empty;
        }

        public virtual void Init(ActorTreeProperty parent)
        {
            if (parent != null)
            {
                this.parent = parent;
                parent.children.Add(this);
            }
        }

        public void AddChild(ActorTreeProperty child)
        {
            child.parent = this;
            children.Add(child);
        }

        public void RemoveChild(ActorTreeProperty child)
        {
            children.Remove(child);
        }

        public void RemoveAll()
        {
            children.Clear();
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