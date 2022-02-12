/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorEvent.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using System.Linq;

    public enum EEventStyle
    {
        Signal = 0,
        Duration,
    }

    internal class ActorEvent : Actor
    {
        [SerializeField] private GUIContent text = new GUIContent();
        [SerializeField] private ActorEventHandle leftHandle = null;
        [SerializeField] private ActorEventHandle rightHandle = null;
        [SerializeField] private ActorTrack _parent = null;

        [SerializeField] public ActionEvent eventProperty = null;
        [SerializeField] private EHitFeedbackType feedbackType = EHitFeedbackType.EHT_None;

        [SerializeField] private int selectInterruptIndex = -1;

        [System.NonSerialized] private bool _hasExecute = false;

        public ActorTrack parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        [SerializeField] private EEventStyle _eventStyle = EEventStyle.Signal;
        public EEventStyle eventStyle
        {
            get
            {
                if (eventProperty != null)
                {
                    EEventStyle style = EEventStyle.Signal;
                    if (eventProperty.TriggerType == ETriggerType.Duration)
                    {
                        style = EEventStyle.Duration;
                    }
                    return style;
                }
                return _eventStyle;
            }
            set
            {
                _eventStyle = value;
                if (value == EEventStyle.Signal)
                {
                    duration = 0;
                    leftHandle = null;
                    rightHandle = null;

                    if (eventProperty != null)
                    {
                        eventProperty.TriggerType = ETriggerType.Signal;
                    }
                }
                else
                {
                    duration = window.snapInterval;
                    leftHandle = new ActorEventHandle();
                    leftHandle.Init(this, start, EEventHandleType.Left);
                    rightHandle = new ActorEventHandle();
                    rightHandle.Init(this, end, EEventHandleType.Right);

                    if (eventProperty != null)
                    {
                        eventProperty.TriggerType = ETriggerType.Duration;
                    }
                }
            }
        }

        [SerializeField] private float _start = 0f;
        public float start
        {
            get
            {
                if (eventProperty != null)
                {
                    return window.ToSecond(eventProperty.TriggerTime);
                }
                return _start;
            }
            set
            {
                _start = Mathf.Max(0, value);
                if (eventProperty != null)
                {
                    eventProperty.TriggerTime = window.ToMillisecond(_start);
                }
            }
        }

        [SerializeField] private float _duration = 0f;
        public float duration
        {
            get
            {
                if (eventProperty != null)
                {
                    return window.ToSecond(eventProperty.Duration);
                }
                return _duration;
            }
            set
            {
                if (eventStyle == EEventStyle.Signal)
                {
                    _duration = 0f;
                }
                else
                {
                    _duration = Mathf.Max(0, value);
                }
                if (eventProperty != null)
                {
                    eventProperty.Duration = window.ToMillisecond(_duration);
                }
            }
        }

        public float end
        {
            get
            {
                if (eventProperty != null)
                {
                    var s = window.ToSecond(eventProperty.TriggerTime);
                    var d = window.ToSecond(eventProperty.Duration);
                    return s + d;
                }
                return start + duration;
            }
        }

        public bool hasExecute
        {
            get { return _hasExecute; }
            set { _hasExecute = value; }
        }

        public override string GetActorType()
        {
            return this.GetType().ToString();
        }

        public void Init(ActorTrack parent, float posX, EEventStyle style, string eventTag, bool manual)
        {
            this.parent = parent;
            this.parent.AddEvent(this);

            eventProperty = new ActionEvent();
            eventProperty.TriggerTime = window.ToMillisecond(start);

            if  (eventTag == window.editorResources.trackAnimationType)
            {
                eventProperty.EventType = EEventDataType.EET_PlayAnim;
            }
            else if (eventTag == window.editorResources.trackEffectType)
            {
                eventProperty.EventType = EEventDataType.EET_PlayEffect;
            }
            else if (eventTag == window.editorResources.trackAudioType)
            {
                eventProperty.EventType = EEventDataType.EET_PlaySound;
            }
            else if (eventTag == window.editorResources.trackAttackType)
            {
                eventProperty.EventType = EEventDataType.EET_AttackDef;
            }
            else if (eventTag == window.editorResources.trackIntteruptType)
            {
                eventProperty.EventType = EEventDataType.EET_Interrupt;
            }

            if (manual)
            {
                if (eventTag == window.editorResources.trackAttackType)
                {
                    window.actorTreeViewAction.AttackList.Add(eventProperty);
                }
                else if (eventTag == window.editorResources.trackIntteruptType)
                {
                    window.actorTreeViewAction.InterruptList.Add(eventProperty);
                }
                else
                {
                    window.actorTreeViewAction.EventList.Add(eventProperty);
                }
            }

            start = window.SnapTime3(window.Pos2Time(posX));
            eventStyle = style;


        }

        public void Destroy()
        {
            if (window.actorTreeViewAction != null)
            {
                if (eventProperty.EventType == EEventDataType.EET_AttackDef)
                {
                    window.actorTreeViewAction.AttackList.Remove(eventProperty);
                }
                else if (eventProperty.EventType == EEventDataType.EET_Interrupt)
                {
                    window.actorTreeViewAction.InterruptList.Remove(eventProperty);
                }
                else
                {
                    window.actorTreeViewAction.EventList.Remove(eventProperty);
                }
            }
        }

        public void BuildRect()
        {
            var x = window.Time2Pos(start) + window.rectTimeArea.x;
            //var width = window.frameWidth;
            var width = Mathf.Max(window.frameWidth, 8);
            if (duration > 0)
            {
                width = window.Time2Pos(end) - window.Time2Pos(start);
            }
            rect = new Rect(x, parent.manipulatorRect.y, width, parent.height);
        }

        public void Move(Event evt, float offset)
        {
            start = window.SnapTime(window.Pos2Time(evt.mousePosition.x) - offset);
            if (eventStyle == EEventStyle.Duration)
            {
                leftHandle.time = start;
                rightHandle.time = end;
            }
        }

        public void OnSelected()
        {
            if (eventProperty != null && eventProperty.EventData != null)
            {
                switch (eventProperty.EventType)
                {
                    case EEventDataType.EET_Interrupt:
                        {
                            window.ClearConditionTree();
                            var interrupt = eventProperty.EventData as ActionInterrupt;
                            window.BuildConditionTree(interrupt.ConditionList);
                        }
                        break;
                    case EEventDataType.EET_AttackDef:
                        {
                            window.ClearConditionTree();
                            var atf = eventProperty.EventData as ActionAttackDef;
                            window.BuildConditionTree(atf.HitFeedbackList);
                        }
                        break;
                }
            }
        }

        public override void Draw()
        {
            if (start < window.viewTimeMin || start > window.viewTimeMax)
                return;

            //if (window.frameWidth <= 10)
            //    return;

            BuildRect();

            var selected = window.HasSelect(this);
            var color = selected ? window.editorResources.colorRed : eventStyle == EEventStyle.Signal ? window.editorResources.colorEventSignal : window.editorResources.colorEventDuration;
            using (new GUIColorScope(color))
            {
                GUI.Box(rect, "", window.editorResources.customEventKey);
            }

            if (eventProperty != null && eventProperty.EventData != null && eventProperty.EventData is EventPlayAnim epa)
            {
                if (!string.IsNullOrEmpty(epa.AnimName) && UnitWrapper.Instance.StateHash.ContainsKey(epa.AnimName))
                {
                    var state = UnitWrapper.Instance.StateHash[epa.AnimName];
                    duration = state.Length;
                }
            }

            switch (eventStyle)
            {
                case EEventStyle.Signal:
                    text.text = window.FormatTime(start);
                    break;
                case EEventStyle.Duration:
                    text.text = window.FormatTime(duration);
                    break;
            }
            using (new GUIColorScope(window.editorResources.colorBlack))
            {
                var rc = rect;
                var size = EditorStyles.label.CalcSize(text);
                var offset = (rc.width - size.x) / 2;
                offset = Mathf.Max(0, offset);
                rc.x += offset;
                GUI.Label(rc, text, EditorStyles.label);
            }

            if (eventStyle == EEventStyle.Duration)
            {
                leftHandle.Draw();
                rightHandle.Draw();
            }
        }

        public void BuildEventHandles(ref List<ActorEventHandle> list)
        {
            if (eventStyle == EEventStyle.Duration)
            {
                list.Add(leftHandle);
                list.Add(rightHandle);
            }
        }

        public override void DrawInspector()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(5);
                window.rightScrollPos = GUILayout.BeginScrollView(window.rightScrollPos, false, true);
                {
                    using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                    {
                        GUILayout.Label("Event");
                    }

                    GUILayout.Space(2);
                    window.DrawProperty(eventProperty);

                    GUILayout.Space(5);
                    using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                    {
                        GUILayout.Label("Event Data");
                    }

                    GUILayout.Space(2);
                    if (eventProperty.EventData != null)
                    {
                        switch (eventProperty.EventType)
                        {
                            case EEventDataType.EET_Interrupt:
                                DrawInspectorInterrupt();
                                break;
                            case EEventDataType.EET_AttackDef:
                                DrawInspectorAttackDef();
                                break;
                            default:
                                window.DrawProperty(eventProperty.EventData);
                                break;
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        //////////////////////////////////////////////////////////////////////////
        /// Event Interrupt
        public void DrawInspectorInterrupt()
        {
            using (new GUIColorScope(window.editorResources.colorInspectorLabel))
            {
                GUILayout.Label("快捷设置");
            }

            EditorGUI.BeginChangeCheck();
            var dict = window.InterruptList(eventProperty.EventData as ActionInterrupt, ref selectInterruptIndex);
            var displayedOptions = dict.Keys.ToArray();
            selectInterruptIndex = EditorGUILayout.Popup(selectInterruptIndex, displayedOptions);
            if (EditorGUI.EndChangeCheck())
            {
                var idName = displayedOptions[selectInterruptIndex];
                eventProperty.EventData = dict[idName].Clone();

                var interrupt = eventProperty.EventData as ActionInterrupt;
                window.BuildConditionTree(interrupt.ConditionList);
            }

            GUILayout.Space(5);
            using (new GUIColorScope(window.editorResources.colorInspectorLabel))
            {
                GUILayout.Label("Interrupt");
            }

            window.DrawActionInterrupt(eventProperty.EventData as ActionInterrupt);
        }

        //////////////////////////////////////////////////////////////////////////
        /// Event AttackDef
        public void DrawInspectorAttackDef()
        {
            window.DrawProperty(eventProperty.EventData);

            ActionAttackDef adf = eventProperty.EventData as ActionAttackDef;

            if (adf.EmitProperty != null)
            {
                using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                {
                    GUILayout.Space(5);
                    GUILayout.Label("[发射器]");
                }
                
                GUILayout.Space(2);
                window.DrawProperty(adf.EmitProperty);
            }

            if (adf.EntityProperty != null)
            {
                using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                {
                    GUILayout.Space(5);
                    GUILayout.Label("[攻击体]");
                }
                
                GUILayout.Space(2);
                window.DrawProperty(adf.EntityProperty);
            }

            if (adf.MotionAnimatorProperty != null)
            {
                using (new GUIColorScope(window.editorResources.colorInspectorLabel))
                {
                    GUILayout.Space(5);
                    GUILayout.Label("[运动插值器]");
                }
                
                GUILayout.Space(2);
                window.DrawProperty(adf.MotionAnimatorProperty);
            }

            DrawFeedback(adf);
        }

        //////////////////////////////////////////////////////////////////////////
        /// Feedback
        private void DrawFeedback(ActionAttackDef adf)
        {
            using (new GUIColorScope(window.editorResources.colorInspectorLabel))
            {
                GUILayout.Space(5);
                EditorGUILayout.LabelField("[受击反馈]");
            }

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            {
                feedbackType = (EHitFeedbackType)EditorGUILayout.EnumPopup(feedbackType);
                if (GUILayout.Button("New Feedback"))
                {
                    NewFeedback();
                    window.BuildConditionTree(adf.HitFeedbackList);
                }
                if (GUILayout.Button("Del Feedback"))
                {
                    DelFeedback();
                    window.BuildConditionTree(adf.HitFeedbackList);
                }
                if (GUILayout.Button("Del ALL"))
                {
                    DelAllFeedback();
                    window.BuildConditionTree(adf.HitFeedbackList);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            window.DrawCondition();
        }

        private void NewFeedback()
        {
            if (feedbackType == EHitFeedbackType.EHT_None || feedbackType == EHitFeedbackType.EHT_MAX)
            {
                EditorUtility.DisplayDialog("INFO", "Please select Hit Feedback type.", "OK");
            }
            else
            {
                ActionAttackDef adf = eventProperty.EventData as ActionAttackDef;
                adf.Add(feedbackType);
            }
        }

        private void DelFeedback()
        {
            var selectable = window.GetActorCondition();
            if (selectable != null)
            {
                ActionAttackDef adf = eventProperty.EventData as ActionAttackDef;
                adf.Del(selectable.property);
                window.DeselectAllCondition();
            }
        }

        private void DelAllFeedback()
        {
            ActionAttackDef adf = eventProperty.EventData as ActionAttackDef;
            adf.DelAll();
        }

    }

}