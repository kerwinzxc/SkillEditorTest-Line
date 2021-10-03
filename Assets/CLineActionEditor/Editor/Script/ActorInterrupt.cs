/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorInterrupt.cs
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
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public class ActorInterrupt : Actor
    {
        public ActorInterrupt()
        {

        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_Interrupt; }
        }

        public override Actor Clone()
        {
            ActorInterrupt aa = ActorInterrupt.CreateInstance<ActorInterrupt>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/InterruptEvt/Add Interrupt")]
        public static void AddInterrupt(MenuCommand cmd)
        {
            ActorInterrupt interrupt = cmd.context as ActorInterrupt;

            ActorEventInterrupt evt = ActorEventInterrupt.CreateInstance<ActorEventInterrupt>();
            evt.Init(interrupt, ActionWindow.Instance.WorkTime);
            interrupt.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/InterruptEvt/Del Interrupt")]
        public static void DelInterrupt(MenuCommand cmd)
        {
            ActorInterrupt interrupt = cmd.context as ActorInterrupt;

            ActorGroupInterrupt groupInterrupt = interrupt.Parent as ActorGroupInterrupt;
            groupInterrupt.ActorList.Remove(interrupt);
            if (interrupt.ID == groupInterrupt.ID && groupInterrupt.ActorList.Count == 0)
                groupInterrupt.ID--;

            EditorUtility.SetDirty(groupInterrupt);
        }

        [MenuItem("CONTEXT/InterruptEvt/Up")]
        public static void Up(MenuCommand cmd)
        {
            ActorInterrupt interrupt = cmd.context as ActorInterrupt;
            ActorGroupInterrupt groupInterrupt = interrupt.Parent as ActorGroupInterrupt;

            int idx = groupInterrupt.ActorList.FindIndex((p) => { return p == interrupt; });
            if (idx > 0)
            {
                Actor temp = groupInterrupt.ActorList[idx - 1];
                groupInterrupt.ActorList[idx - 1] = groupInterrupt.ActorList[idx];
                groupInterrupt.ActorList[idx] = temp;

                EditorUtility.SetDirty(groupInterrupt);
            }
        }

        [MenuItem("CONTEXT/InterruptEvt/Down")]
        public static void Down(MenuCommand cmd)
        {
            ActorInterrupt interrupt = (ActorInterrupt)cmd.context;
            ActorGroupInterrupt groupInterrupt = interrupt.Parent as ActorGroupInterrupt;

            int idx = groupInterrupt.ActorList.FindIndex((p) => { return p == interrupt; });
            if (idx < groupInterrupt.ActorList.Count - 1)
            {
                Actor temp = groupInterrupt.ActorList[idx + 1];
                groupInterrupt.ActorList[idx + 1] = groupInterrupt.ActorList[idx];
                groupInterrupt.ActorList[idx] = temp;

                EditorUtility.SetDirty(groupInterrupt);
            }
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/InterruptEvt/", new MenuCommand(this));
        }

        public override void OnDraw(ref Rect rect, bool active)
        {
            base.OnDraw(ref rect, active);

            GUI.color = new Color(0.4f, 0.45f, 0.6f, 1f);
            if (!active)
                GUI.color *= new Color(0.75f, 0.75f, 0.75f, 1f);

            mName = ID.ToString();
            if (mActorList.Count != 0)
            {
                ActorEventInterrupt aei = mActorList[0] as ActorEventInterrupt;
                if (aei.Interrupt != null && !string.IsNullOrEmpty(aei.Interrupt.DebugName))
                {
                    mName += "_" + aei.Interrupt.DebugName;
                }
            }
        }

    }
}
