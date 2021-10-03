/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupInterrupt.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-22      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    public class ActorGroupInterrupt : Actor
    {
        public ActorGroupInterrupt()
        {
            mColor = new Color(0f, 1f, 1f, 1f);
            mName = "";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupInterrupt; }
        }

        public override Actor Clone()
        {
            ActorGroupInterrupt aa = ActorGroupInterrupt.CreateInstance<ActorGroupInterrupt>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Interrupt/New Interrupt")]
        public static void NewInterrupt(MenuCommand cmd)
        {
            ActorGroupInterrupt groupInterrupt = (ActorGroupInterrupt)cmd.context;

            ActorInterrupt interrupt = ActorInterrupt.CreateInstance<ActorInterrupt>();
            interrupt.Parent = groupInterrupt;
            interrupt.ID = groupInterrupt.GetActorID();
            groupInterrupt.mActorList.Add(interrupt);
        }

        [MenuItem("CONTEXT/Interrupt/Copy Interrupt")]
        public static void CopyInterrupt(MenuCommand cmd)
        {
            ActorGroupInterrupt groupInterrupt = (ActorGroupInterrupt)cmd.context;
            ActionWindow.Instance.CopyInterruptGroup = groupInterrupt;
        }

        [MenuItem("CONTEXT/Interrupt/Paste Interrupt")]
        public static void PasteInterrupt(MenuCommand cmd)
        {
            ActorGroupInterrupt groupInterrupt = (ActorGroupInterrupt)cmd.context;
            groupInterrupt.Clone(ActionWindow.Instance.CopyInterruptGroup);
            EditorUtility.SetDirty(groupInterrupt);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Interrupt/", new MenuCommand(this));
        }
    }
}
