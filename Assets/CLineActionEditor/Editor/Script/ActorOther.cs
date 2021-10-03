/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorOther.cs
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

    public class ActorOther : Actor
    {
        public ActorOther()
        {

        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_Other; }
        }

        public override Actor Clone()
        {
            ActorOther aa = ActorOther.CreateInstance<ActorOther>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/OtherEvt/Add Other")]
        public static void AddEvent(MenuCommand cmd)
        {
            ActorOther attack = cmd.context as ActorOther;

            ActorEvent evt = ActorEvent.CreateInstance<ActorEvent>();
            evt.Init(attack, ActionWindow.Instance.WorkTime);
            attack.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/OtherEvt/Del Other")]
        public static void DelAttack(MenuCommand cmd)
        {
            ActorOther attack = cmd.context as ActorOther;

            ActorGroupOther groupAttack = attack.Parent as ActorGroupOther;
            groupAttack.ActorList.Remove(attack);
            if (attack.ID == groupAttack.ID && groupAttack.ActorList.Count == 0)
                groupAttack.ID--;

            EditorUtility.SetDirty(groupAttack);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/OtherEvt/", new MenuCommand(this));
        }

        public override void OnDraw(ref Rect rect, bool active)
        {
            base.OnDraw(ref rect, active);

            GUI.color = new Color(0.4f, 0.45f, 0.6f, 1f);
            if (!active)
                GUI.color *= new Color(0.75f, 0.75f, 0.75f, 1f);

            mName = ID.ToString();
        }

    }
}
