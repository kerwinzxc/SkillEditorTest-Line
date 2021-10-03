/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupOther.cs
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

    public class ActorGroupOther : Actor
    {
        public ActorGroupOther()
        {
            mColor = new Color(0.5f, 0.9f, 0.9f, 1f);
            mName = "其他组";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupOther; }
        }

        public override Actor Clone()
        {
            ActorGroupOther aa = ActorGroupOther.CreateInstance<ActorGroupOther>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Other/New Other")]
        public static void NewOther(MenuCommand cmd)
        {
            ActorGroupOther groupOther = (ActorGroupOther)cmd.context;

            ActorOther other = ActorOther.CreateInstance<ActorOther>();
            other.Parent = groupOther;
            other.ID = groupOther.GetActorID();
            groupOther.mActorList.Add(other);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Other/", new MenuCommand(this));
        }
    }
}
