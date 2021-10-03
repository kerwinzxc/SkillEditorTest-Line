/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActorEventAttackDef.cs
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
    using System.Collections.Generic;

    public class ActorEventAttackDef : Actor
    {
        private EHitFeedbackType mCurFeedbackType = EHitFeedbackType.EHT_None;
        private IProperty mCurFeedback = null;

        private bool mHasExecute;
        private ActionAttackDef mAttack;

        public bool HasExecute
        {
            get { return mHasExecute; }
            set { mHasExecute = value; }
        }
        
        public ActionAttackDef Attack
        {
            get { return mAttack; }
            set { mAttack = value; }
        }

        public ActorEventAttackDef()
        {
            mActiveColor = Color.red;
        }

        public void Init(Actor parent, float time)
        {
            Parent = parent;
            StartTime = time;

            mAttack = new ActionAttackDef();
            mAttack.TriggerTime = (int)(time * 1000);
            mAttack.ActorID = parent.ID.ToString();
        }
        public override EActorType GetActorType
        {
            get { return EActorType.EAT_EventAttack; }
        }

        public override Actor Clone()
        {
            ActorEventAttackDef aa = ActorEventAttackDef.CreateInstance<ActorEventAttackDef>();
            aa.Clone(this);
            aa.mHasExecute = this.mHasExecute;
            //aa.mAttack = this.mAttack.Clone();

            return aa;
        }

        [MenuItem("CONTEXT/Evt_Attack/Del Attack")]
        public static void DelAttack(MenuCommand cmd)
        {
            ActorEventAttackDef evt = (ActorEventAttackDef)cmd.context;

            Actor parent = evt.Parent;
            parent.ActorList.Remove(evt);

            EditorUtility.SetDirty(parent);
        }
        public override void OnRightMouseDown(Vector2 point)
        {
            EditorUtility.DisplayPopupMenu(new Rect(point.x, point.y, 0, 0), "CONTEXT/Evt_Attack/", new MenuCommand(this));
        }
        public override void OnLeftMouseDrag(Vector2 point)
        {
            mAttack.TriggerTime = (int)(ActionWindow.Instance.PosToWorkTime(point.x) * 1000);
        }
        public override void OnDraw(ref Rect rect, bool active)
        {
            if (active)
                GUI.color = mActiveColor;
            else
                GUI.color = mColor;

            mRegion = new Rect(ActionWindow.Instance.WorkTimeToPos(mAttack.TriggerTime * 0.001f) - 3f, rect.y + 2, 6, 16);
            GUI.Box(mRegion, "", ActionWindow.Instance.EditorSkin.GetStyle("Key"));
        }
        public override void OnDrawInspector()
        {
            if (mAttack != null)
                ActionWindow.DrawProperty(mAttack);

            if (mAttack.EmitProperty != null)
            {
                GUI.color = Color.cyan;
                GUILayout.Space(5);
                GUILayout.Label("[发射器]");
                GUILayout.Space(2);
                GUI.color = Color.white;
                ActionWindow.DrawProperty(mAttack.EmitProperty);
            }

            if (mAttack.EntityProperty != null)
            {
                GUI.color = Color.cyan;
                GUILayout.Space(5);
                GUILayout.Label("[攻击体]");
                GUILayout.Space(2);
                GUI.color = Color.white;
                ActionWindow.DrawProperty(mAttack.EntityProperty);
            }

            if (mAttack.MotionAnimatorProperty != null)
            {
                GUI.color = Color.cyan;
                GUILayout.Space(5);
                GUILayout.Label("[运动插值器]");
                GUILayout.Space(2);
                GUI.color = Color.white;
                ActionWindow.DrawProperty(mAttack.MotionAnimatorProperty);
            }

            DrawFeedback();
        }
        private void DrawFeedback()
        {
            GUILayout.Space(5);
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("[受击反馈]");
            GUI.color = Color.white;
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            mCurFeedbackType = (EHitFeedbackType)EditorGUILayout.EnumPopup(mCurFeedbackType);
            if (GUILayout.Button("New Feedback"))
            {
                NewFeedback();
            }
            if (GUILayout.Button("Del Feedback"))
            {
                DelFeedback();
            }
            if (GUILayout.Button("Del ALL"))
            {
                DelAllFeedback();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            using (List<IProperty>.Enumerator itr = mAttack.HitFeedbackList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(itr.Current.GetType().ToString().Replace("CAE.Core.", "")))
                    {
                        mCurFeedback = itr.Current;
                    }
                    if (mCurFeedback == itr.Current)
                    {
                        GUILayout.Label(ActionWindow.Instance.GetCachedTex(Color.red, 16));
                    }
                    GUILayout.EndHorizontal();

                    if (mCurFeedback == itr.Current)
                    {
                        GUILayout.Space(3);
                        ActionWindow.DrawProperty(itr.Current);
                        GUILayout.Space(3);
                    }
                }
            }
        }

        private void NewFeedback()
        {
            if (mCurFeedbackType == EHitFeedbackType.EHT_None || mCurFeedbackType == EHitFeedbackType.EHT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select Hit Feedback type.", "OK");
            }
            else
            {
                mCurFeedback = mAttack.Add(mCurFeedbackType);
            }
        }

        private void DelFeedback()
        {
            if (mCurFeedback != null)
            {
                mAttack.Del(mCurFeedback);
                mCurFeedback = null;
            }
        }

        private void DelAllFeedback()
        {
            mCurFeedback = null;
            mAttack.DelAll();
        }

    }
}
