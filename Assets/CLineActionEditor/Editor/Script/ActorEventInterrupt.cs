/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorEventInterrupt.cs
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
    using System.Collections.Generic;

    public class ActorEventInterrupt : Actor
    {
        private ActionInterrupt mInterrupt;
        private int mShortcutIdx;
        private List<string> mShortcutNameList;
        private Dictionary<string, ActionInterrupt> mShortcutInterruptList;

        public override Actor Clone()
        {
            ActorEventInterrupt aa = ActorEventInterrupt.CreateInstance<ActorEventInterrupt>();
            aa.Clone(this);
            aa.mInterrupt = this.mInterrupt.Clone();
            aa.mShortcutIdx = this.mShortcutIdx;
            aa.mShortcutNameList = new List<string>();
            aa.mShortcutNameList.AddRange(this.mShortcutNameList);
            aa.mShortcutInterruptList = new Dictionary<string, ActionInterrupt>();
            using (Dictionary<string, ActionInterrupt>.Enumerator itr = this.mShortcutInterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    aa.mShortcutInterruptList.Add(itr.Current.Key, itr.Current.Value.Clone());
                }
            }

            return aa;
        }

        public ActionInterrupt Interrupt
        {
            get { return mInterrupt; }
            set { mInterrupt = value; }
        }

        public ActorEventInterrupt()
        {
            mActiveColor = Color.red;
        }

        public void Init(Actor parent, float time)
        {
            Parent = parent;
            StartTime = time;

            mInterrupt = new ActionInterrupt();
            mInterrupt.InterruptTime = (int)(time * 1000);
            mInterrupt.ActorID = parent.ID.ToString();

            mShortcutIdx = -1;
            mShortcutNameList = new List<string>();
            mShortcutInterruptList = new Dictionary<string, ActionInterrupt>();
            using (Dictionary<IProperty, ActionInterrupt>.Enumerator itr = ActionWindow.Instance.ActionInterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mShortcutNameList.Add(itr.Current.Value.InterruptName);
                    mShortcutInterruptList.Add(itr.Current.Value.InterruptName, itr.Current.Value);
                }
            }
        }

        public void DeserializeShortcutIndex()
        {
            mShortcutIdx = -1;
            mShortcutNameList = new List<string>();
            mShortcutInterruptList = new Dictionary<string, ActionInterrupt>();
            using (Dictionary<IProperty, ActionInterrupt>.Enumerator itr = ActionWindow.Instance.ActionInterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mShortcutNameList.Add(itr.Current.Value.InterruptName);
                    mShortcutInterruptList.Add(itr.Current.Value.InterruptName, itr.Current.Value);
                }
            }

            for (int i = 0; i < mShortcutNameList.Count; ++i)
            {
                if (mInterrupt.InterruptName == mShortcutNameList[i])
                {
                    mShortcutIdx = i;
                    break;
                }
            }
        }

        public override EActorType GetActorType
        {
            get { return EActorType.EAT_EventInterrupt; }
        }

        [MenuItem("CONTEXT/Evt_Interrupt/Del Interrupt")]
        public static void DelInterrupt(MenuCommand cmd)
        {
            ActorEventInterrupt evt = (ActorEventInterrupt)cmd.context;

            Actor parent = evt.Parent;
            parent.ActorList.Remove(evt);

            EditorUtility.SetDirty(parent);
        }

        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Evt_Interrupt/", new MenuCommand(this));
        }

        public override void OnLeftMouseDrag(Vector2 point)
        {
            mInterrupt.InterruptTime = (int)(ActionWindow.Instance.PosToWorkTime(point.x) * 1000);
        }

        public override void OnDraw(ref Rect rect, bool active)
        {
            if (active)
                GUI.color = mActiveColor;
            else
                GUI.color = mColor;

            mRegion = new Rect(ActionWindow.Instance.WorkTimeToPos(mInterrupt.InterruptTime * 0.001f) - 3f, rect.y + 2, 6, 16);
            GUI.Box(mRegion, "", ActionWindow.Instance.EditorSkin.GetStyle("Key"));
        }

        public override void OnDrawInspector()
        {
            GUI.color = Color.cyan;
            GUILayout.Label("快捷设置");
            GUI.color = Color.white;

            int oldIdx = mShortcutIdx;

            mShortcutIdx = EditorGUILayout.Popup(mShortcutIdx, mShortcutNameList.ToArray());
            if (mShortcutIdx != oldIdx && mShortcutIdx >= 0 && mShortcutIdx < mShortcutNameList.Count)
            {
                mInterrupt = mShortcutInterruptList[mShortcutNameList[mShortcutIdx]].Clone();
            }

            GUILayout.Space(5);
            GUI.color = Color.cyan;
            GUILayout.Label("Interrupt");
            GUI.color = Color.white;

            if (mInterrupt != null)
                ActionWindow.Instance.DrawActionInterrupt(mInterrupt);
        }

    }
}
