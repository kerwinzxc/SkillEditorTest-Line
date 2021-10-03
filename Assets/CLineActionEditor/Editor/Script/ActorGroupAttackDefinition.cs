/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupAttackDefinition.cs
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

    public class ActorGroupAttackDefinition : Actor
    {
        public ActorGroupAttackDefinition()
        {
            mColor = new Color(0f, 0.9f, 0.9f, 1f);
            mName = "攻击定义组";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupAttackDefinition; }
        }

        public override Actor Clone()
        {
            ActorGroupAttackDefinition aa = ActorGroupAttackDefinition.CreateInstance<ActorGroupAttackDefinition>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Attack/New AttackDef")]
        public static void NewAttackDef(MenuCommand cmd)
        {
            ActorGroupAttackDefinition groupAttack = (ActorGroupAttackDefinition)cmd.context;

            ActorAttackDefinition attack = ActorAttackDefinition.CreateInstance<ActorAttackDefinition>();
            attack.Parent = groupAttack;
            attack.ID = groupAttack.GetActorID();
            groupAttack.mActorList.Add(attack);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Attack/", new MenuCommand(this));
        }
    }
}
