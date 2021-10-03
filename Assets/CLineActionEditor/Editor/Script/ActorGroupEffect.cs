/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorGroupEffect.cs
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

    public class ActorGroupEffect : Actor
    {
        public ActorGroupEffect()
        {
            mColor = new Color(0f, 0.9f, 0f, 1f);
            mName = "特效组";
            mNameOffsetX = 30f;
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_GroupEffect; }
        }

        public override Actor Clone()
        {
            ActorGroupEffect aa = ActorGroupEffect.CreateInstance<ActorGroupEffect>();
            aa.Clone(this);

            return aa;
        }

        [MenuItem("CONTEXT/Effect/New Effect")]
        public static void NewEffect(MenuCommand cmd)
        {
            ActorGroupEffect groupEffect = (ActorGroupEffect)cmd.context;

            ActorEffect effect = ActorEffect.CreateInstance<ActorEffect>();
            effect.Parent = groupEffect;
            effect.ID = groupEffect.GetActorID();
            groupEffect.mActorList.Add(effect);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Effect/", new MenuCommand(this));
        }
    }
}
