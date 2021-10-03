/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorAnimation.cs
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
    using UnityEditor.Animations;

    public class ActorAnimation : Actor
    {
        public override EActorType GetActorType
        {
            get
            {
                return EActorType.EAT_Animation;
            }
        }

        public override Actor Clone()
        {
            ActorAnimation aa = ActorAnimation.CreateInstance<ActorAnimation>();
            aa.Clone(this);
            return aa;
        }

        [MenuItem("CONTEXT/AnimEvt/Add Event")]
        public static void AddEvent(MenuCommand cmd)
        {
            ActorAnimation anim = cmd.context as ActorAnimation;

            ActorEvent evt = ActorEvent.CreateInstance<ActorEvent>();
            evt.Init(anim, ActionWindow.Instance.WorkTime);
            anim.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/AnimEvt/Reset TotalTime")]
        public static void ResetTotalTime(MenuCommand cmd)
        {
            ActorAnimation anim = cmd.context as ActorAnimation;

            ActionWindow.Instance.TotalTime = anim.mDuration;
            Action action = ActionWindow.Instance.Property as Action;
            if (action != null)
            {
                action.TotalTime = (int)(anim.mDuration * EditorUtil.TimeToMillisecond);
            }
        }

        [MenuItem("CONTEXT/AniEvt/Del Animation")]
        public static void DelAnimation(MenuCommand cmd)
        {
            ActorAnimation anim = cmd.context as ActorAnimation;

            ActorGroupAnimation groupAnim = anim.Parent as ActorGroupAnimation;
            groupAnim.ActorList.Remove(anim);
            if (anim.ID == groupAnim.ID && groupAnim.ActorList.Count == 0)
                groupAnim.ID--;

            EditorUtility.SetDirty(groupAnim);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/AnimEvt/", new MenuCommand(this));
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
                ActorEvent ae = mActorList[0] as ActorEvent;
                EventPlayAnim epa = ae.Event.EventData as EventPlayAnim;

                if (epa != null && !string.IsNullOrEmpty(epa.AnimName) && mName != epa.AnimName)
                {
                    if (UnitWrapper.Instance.StateHash.ContainsKey(epa.AnimName))
                    {
                        AnimatorState state = UnitWrapper.Instance.StateHash[epa.AnimName];
                        mName += "_" + epa.AnimName;
                        mDuration = state.motion.averageDuration;
                    }
                }

                mStartTime = ae.Event.TriggerTime * 0.001f;

                Rect rc = rect;
                rc.x = ActionWindow.Instance.WorkTimeToPos(mStartTime);
                rc.width = ActionWindow.Instance.WorkTimeToPos(mDuration);

                GUI.Box(rc, "", ActionWindow.Instance.EditorSkin.GetStyle("Actor"));
                GUI.color = Color.cyan;
                GUI.Label(rc, mDuration.ToString());
            }
        }

    }
}
