/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupAnimation.cs
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

    public class ActorGroupAnimation : Actor
    {
        public ActorGroupAnimation()
        {
            mColor = new Color(0f, 0.8f, 0f, 1f);
            mName = "动作组";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupAnimation; }
        }

        public override Actor Clone()
        {
            ActorGroupAnimation aa = ActorGroupAnimation.CreateInstance<ActorGroupAnimation>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Anim/New Animation")]
        public static void NewAnimation(MenuCommand cmd)
        {
            ActorGroupAnimation groupAnim = (ActorGroupAnimation)cmd.context;

            ActorAnimation anim = ActorAnimation.CreateInstance<ActorAnimation>();
            anim.Parent = groupAnim;
            anim.ID = groupAnim.GetActorID();
            groupAnim.mActorList.Add(anim);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Anim/", new MenuCommand(this));
        }
    }
}
