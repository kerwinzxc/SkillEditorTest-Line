/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\Actor.cs
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
    using System.Collections.Generic;

    internal class Actor : ScriptableObject
    {
        [SerializeField] private List<Manipulator> manipulatorList = new List<Manipulator>();

        [SerializeField] protected Rect rect = Rect.zero;

        public string ActorID;
        public int ActorIndex;

        public Rect manipulatorRect
        {
            get { return rect; }
        }

        public ActionWindow window
        {
            get { return ActionWindow.instance; }
        }

        public virtual string GetActorType()
        { return string.Empty; }
        public virtual void Draw()
        { }
        public virtual void DrawInspector()
        { }

        public void AddManipulator(Manipulator m)
        {
            manipulatorList.Add(m);
        }
        public bool HandleManipulatorsEvents(ActionWindow window, Event evt)
        {
            var isHandled = false;
            using (var itr = manipulatorList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    isHandled = itr.Current.HandleEvent(evt, window);
                    if (isHandled)
                        break;
                }
            }

            return isHandled;
        }

    }
}