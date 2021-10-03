/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\Actor.cs
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
    using System.Collections.Generic;

    public enum EActorType
    {
        EAT_GroupAnimation,
        EAT_GroupAttackDefinition,
        EAT_GroupCamera,
        EAT_GroupEffect,
        EAT_GroupInterrupt,
        EAT_GroupSound,
        EAT_GroupOther,

        EAT_Animation,
        EAT_AttackDefinition,
        EAT_Camera,
        EAT_Effect,
        EAT_Interrupt,
        EAT_Sound,
        EAT_Other,

        EAT_Event,
        EAT_EventAttack,
        EAT_EventInterrupt,
    }

    public abstract class Actor : ScriptableObject
    {
        private int mID = 0;
        protected string mName = string.Empty;
        protected float mStartTime;
        protected float mDuration;
        protected float mNameOffsetX = 0f;
        protected Color mColor = Color.white;
        protected Color mActiveColor = Color.white;
        protected Rect mRegion = new Rect(0, 0, 0, 0);
        protected Actor mParent;
        protected List<Actor> mActorList = new List<Actor>();

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        public float StartTime
        {
            get { return mStartTime; }
            set { mStartTime = value; }
        }

        public Actor Parent
        {
            get { return mParent; }
            set { mParent = value; }
        }

        public List<Actor> ActorList
        {
            get { return mActorList; }
        }

        public int ID
        {
            get { return mID; }
            set { mID = value; }
        }

        public abstract EActorType GetActorType { get; }
        public abstract Actor Clone();

        public virtual void OnLeftMouseDown(Vector2 point)
        {

        }

        public virtual void OnLeftMouseUp(Vector2 point)
        {

        }

        public virtual void OnLeftMouseDrag(Vector2 point)
        {

        }

        public virtual void OnLeftMouseDoubleClick(Vector2 point)
        {

        }

        public virtual void OnRightMouseDown(Vector2 point)
        {

        }

        public virtual void OnRightMouseUp(Vector2 point)
        {

        }

        public virtual void OnDraw(ref Rect rect, bool active)
        {
            if (active)
                GUI.color = mActiveColor;
            else
                GUI.color = mColor;

            mRegion = new Rect(0, rect.y, rect.width, rect.height);
            GUI.Box(mRegion, "");
        }

        public virtual void OnDrawName(ref Rect rect)
        {
            GUI.color = mColor;
            GUI.Label(new Rect(mNameOffsetX, rect.y + rect.height * 0.5f - 5f, rect.width - mNameOffsetX, rect.height), mName);
        }

        public virtual void OnDrawInspector()
        {

        }

        public bool IsInActor(Vector2 point)
        {
            return EditorUtil.IsPointInRect(ref point, ref mRegion);
        }

        public int GetActorID()
        {
            return ++mID;
        }

        public void Clone(Actor ac)
        {
            this.mID = ac.mID;
            this.mName = ac.mName;
            this.mStartTime = ac.mStartTime;
            this.mDuration = ac.mDuration;
            this.mNameOffsetX = ac.mNameOffsetX;
            this.mColor = ac.mColor;
            this.mActiveColor = ac.mActiveColor;
            this.mRegion = ac.mRegion;
            this.mParent = ac.mParent;

            using (List<Actor>.Enumerator itr = ac.mActorList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    Actor a = itr.Current.Clone();
                    a.Parent = this;
                    this.mActorList.Add(a);
                }
            }
        }

    }

}
