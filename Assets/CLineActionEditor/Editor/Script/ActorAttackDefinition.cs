/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorAttackDefinition.cs
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

    public class ActorAttackDefinition : Actor
    {
        public ActorAttackDefinition()
        {

        }

        public override Actor Clone()
        {
            ActorAttackDefinition aa = ActorAttackDefinition.CreateInstance<ActorAttackDefinition>();
            aa.Clone(this);
            return aa;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_AttackDefinition; }
        }

        [MenuItem("CONTEXT/AttackEvt/Add AttackDef")]
        public static void AddEvent(MenuCommand cmd)
        {
            ActorAttackDefinition attack = (ActorAttackDefinition)cmd.context;

            ActorEventAttackDef evt = ActorEventAttackDef.CreateInstance<ActorEventAttackDef>();
            evt.Init(attack, ActionWindow.Instance.WorkTime);
            attack.ActorList.Add(evt);
        }

        [MenuItem("CONTEXT/AttackEvt/Del AttackDef")]
        public static void DelAttackDef(MenuCommand cmd)
        {
            ActorAttackDefinition attack = (ActorAttackDefinition)cmd.context;

            ActorGroupAttackDefinition groupAttack = attack.Parent as ActorGroupAttackDefinition;
            groupAttack.ActorList.Remove(attack);
            if (attack.ID == groupAttack.ID && groupAttack.ActorList.Count == 0)
                groupAttack.ID--;

            EditorUtility.SetDirty(groupAttack);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/AttackEvt/", new MenuCommand(this));
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
                ActorEventAttackDef aet = mActorList[0] as ActorEventAttackDef;
                if (aet.Attack != null && !string.IsNullOrEmpty(aet.Attack.Name))
                {
                    mName += "_" + aet.Attack.Name;
                }
            }
        }

    }
}
