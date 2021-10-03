/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorEvent.cs
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
    using System;
    using UnityEditor;
    using UnityEngine;

    public class ActorEvent : Actor
    {
        private Event mEvent = null;
        private bool mHasExecute = false;

        public Event Event
        {
            get { return mEvent; }
            set { mEvent = value; }
        }

        public bool HasExecute
        {
            get { return mHasExecute; }
            set { mHasExecute = value; }
        }

        public ActorEvent()
        {
            mActiveColor = Color.red;
            mHasExecute = false;
        }

        public void Init(Actor parent, float time)
        {
            Parent = parent;
            StartTime = time;

            mEvent = new Event();
            mEvent.TriggerTime = (int)(time * EditorUtil.TimeToMillisecond);
            mEvent.ActorID = parent.ID.ToString();
        }

        public override EActorType GetActorType
        {
            get
            {
                return EActorType.EAT_Event;
            }
        }

        public override Actor Clone()
        {
            ActorEvent aa = ActorEvent.CreateInstance<ActorEvent>();
            aa.Clone(this);
            aa.mEvent = this.mEvent.Clone();
            aa.mHasExecute = this.mHasExecute;
            return aa;
        }

        [MenuItem("CONTEXT/Event/Del Event")]
        public static void DelEvent(MenuCommand cmd)
        {
            ActorEvent evt = cmd.context as ActorEvent;

            Actor parent = evt.Parent;
            parent.ActorList.Remove(evt);

            EditorUtility.SetDirty(parent);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Event", new MenuCommand(this));
        }

        public override void OnLeftMouseDrag(Vector2 point)
        {
            mEvent.TriggerTime = (int)(ActionWindow.Instance.PosToWorkTime(point.x) * 1000);
        }

        public override void OnDraw(ref Rect rect, bool active)
        {
            if (active)
                GUI.color = mActiveColor;
            else
                GUI.color = mColor;

            mRegion = new Rect(ActionWindow.Instance.WorkTimeToPos(mEvent.TriggerTime * 0.001f) - 3f, rect.y + 2, 6, 16);
            GUI.Box(mRegion, "", ActionWindow.Instance.EditorSkin.GetStyle("Key"));
        }

        public override void OnDrawInspector()
        {
            GUI.color = Color.cyan;
            GUILayout.Label("Event");
            GUI.color = Color.white;

            if (mEvent != null)
                ActionWindow.DrawProperty(mEvent);

            GUILayout.Space(5);
            GUI.color = Color.cyan;
            GUILayout.Label("Event Data");
            GUI.color = Color.white;

            if (mEvent != null && mEvent.EventData != null)
                ActionWindow.DrawProperty(mEvent.EventData);
        }

    }
}
