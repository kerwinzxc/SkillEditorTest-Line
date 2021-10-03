/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupSound.cs
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

    public class ActorGroupSound : Actor
    {
        public ActorGroupSound()
        {
            mColor = new Color(0f, 1f, 0f, 1f);
            mName = "音效组";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupSound; }
        }

        public override Actor Clone()
        {
            ActorGroupSound aa = ActorGroupSound.CreateInstance<ActorGroupSound>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Sound/New Sound")]
        public static void NewSound(MenuCommand cmd)
        {
            ActorGroupSound groupSound = (ActorGroupSound)cmd.context;

            ActorSound sound = ActorSound.CreateInstance<ActorSound>();
            sound.Parent = groupSound;
            sound.ID = groupSound.GetActorID();
            groupSound.mActorList.Add(sound);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Sound/", new MenuCommand(this));
        }
    }
}
