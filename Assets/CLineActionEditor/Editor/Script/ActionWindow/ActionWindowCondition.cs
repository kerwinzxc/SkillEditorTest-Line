/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowCondition.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-11-1      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using System.Linq;

    internal sealed partial class ActionWindow
    {
        [SerializeField] private ActorTreeProperty actorConditionTree = null;

        private void InitInspectorCondition()
        {
            actorConditionTree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorConditionTree.Init(null);
            actorConditionTree.AddManipulator(new ActorTreeConditionManipulator(actorConditionTree));
        }

        public void BuildConditionTree<T>(List<T> list)
        {
            actorConditionTree.children.Clear();
            using (var itr = list.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    ActorCondition ac = ScriptableObject.CreateInstance<ActorCondition>();
                    ac.property = itr.Current as IProperty;
                    ac.Init(actorConditionTree);
                }
            }
        }

        public void DrawCondition()
        {
            actorConditionTree.HandleManipulatorsEvents(this, Event.current);
            actorConditionTree.Draw();
        }

        public void ClearConditionTree()
        {
            actorConditionTree.RemoveAll();
        }

    }

}