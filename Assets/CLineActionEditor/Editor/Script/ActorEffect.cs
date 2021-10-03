/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorEffect.cs
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

    public class ActorEffect : Actor
    {
        public ActorEffect()
        {

        }

        public override Actor Clone()
        {
            ActorEffect aa = ActorEffect.CreateInstance<ActorEffect>();
            aa.Clone(this);
            return aa;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_Effect; }
        }

        [MenuItem("CONTEXT/EffectEvt/Add Event")]
        public static void AddEvent(MenuCommand cmd)
        {
            ActorEffect effect = (ActorEffect)cmd.context;

            ActorEvent evt = ActorEvent.CreateInstance<ActorEvent>();
            evt.Init(effect, ActionWindow.Instance.WorkTime);
            effect.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/EffectEvt/Del Effect")]
        public static void DelCamera(MenuCommand cmd)
        {
            ActorEffect effect = (ActorEffect)cmd.context;

            ActorGroupEffect groupEffect = effect.Parent as ActorGroupEffect;
            groupEffect.ActorList.Remove(effect);
            if (effect.ID == groupEffect.ID && groupEffect.ActorList.Count == 0)
                groupEffect.ID--;

            EditorUtility.SetDirty(groupEffect);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/EffectEvt/", new MenuCommand(this));
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
                EventPlayEffect epe = ae.Event.EventData as EventPlayEffect;

                if (epe != null && !string.IsNullOrEmpty(epe.EffectName) && mName != epe.EffectName)
                {
                    mName += "_" + epe.EffectName;
                    //mDuration = ActionWindow.Instance.EffectWrapperHash[mName].GetEffectTime();
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
