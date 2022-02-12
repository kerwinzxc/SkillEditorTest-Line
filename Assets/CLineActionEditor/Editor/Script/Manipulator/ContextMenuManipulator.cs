/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Manipulator\ContextMenuManipulator.cs
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
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;

    internal class ContextMenuManipulator : Manipulator
    {
        private ActorTreeItem owner = null;

        public ContextMenuManipulator(ActorTreeItem owner)
        {
            this.owner = owner;
        }

        protected override bool ContextClick(Event evt, ActionWindow window)
        {
            if (evt.alt)
                return false;

            if (evt.mousePosition.x < window.headerWidth)
                return false;

            var list = owner.BuildRows();
            var selectable = new List<ActorTreeItem>();
            using (var itr = list.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    var rect = itr.Current.manipulatorRect;
                    rect.width += window.rectContent.width;
                    var pos = window.MousePos2ViewPos(evt.mousePosition);
                    if (rect.Contains(pos))
                    {
                        selectable.Add(itr.Current);
                    }
                }
            }

            var item = selectable.OrderByDescending(x => x.depth).FirstOrDefault();
            window.ShowContextMenu(item, evt);

            return true;
        }
    }

}
