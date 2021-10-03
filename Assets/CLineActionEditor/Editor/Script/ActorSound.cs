/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorSound.cs
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

    public class ActorSound : Actor
    {
        public ActorSound()
        {

        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_Sound; }
        }

        public override Actor Clone()
        {
            ActorSound aa = ActorSound.CreateInstance<ActorSound>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/SoundEvt/Add Event")]
        public static void AddEvent(MenuCommand cmd)
        {
            ActorSound sound = cmd.context as ActorSound;

            ActorEvent evt = ActorEvent.CreateInstance<ActorEvent>();
            evt.Init(sound, ActionWindow.Instance.WorkTime);
            sound.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/SoundEvt/Del Sound")]
        public static void DelSound(MenuCommand cmd)
        {
            ActorSound sound = cmd.context as ActorSound;

            ActorGroupSound groupSound = sound.Parent as ActorGroupSound;
            groupSound.ActorList.Remove(sound);
            if (sound.ID == groupSound.ID && groupSound.ActorList.Count == 0)
                groupSound.ID--;

            EditorUtility.SetDirty(groupSound);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/SoundEvt/", new MenuCommand(this));
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
                EventPlaySound eps = ae.Event.EventData as EventPlaySound;

                if (eps != null && !string.IsNullOrEmpty(eps.SoundName))
                {
                    mName += "_" + eps.SoundName;
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
