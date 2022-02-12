/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowState.cs
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
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed partial class ActionWindow
    {
        [SerializeReference] private List<Manipulator> captureList = new List<Manipulator>();
        [System.NonSerialized] public List<Actor> selectionList = new List<Actor>();
        [System.NonSerialized] public List<Actor> propertyList = new List<Actor>();
        [System.NonSerialized] public List<Actor> conditionList = new List<Actor>();

        public void Select(Actor actor)
        {
            selectionList.Clear();
            selectionList.Add(actor);
        }

        public void SelectMore(Actor actor)
        {
            if (!selectionList.Contains(actor))
                selectionList.Add(actor);
        }

        public void Deselect(Actor actor)
        {
            selectionList.Remove(actor);
        }

        public void DeselectAll()
        {
            selectionList.Clear();
        }

        public void DeselectAllTrack()
        {
            selectionList = selectionList.Where(x => x is ActorEvent).ToList();
        }

        public void DeselectAllEvent()
        {
            selectionList = selectionList.Where(x => x is ActorTreeItem).ToList();
        }

        public bool HasSelect(Actor actor)
        {
            return selectionList.Contains(actor);
        }

        public Actor GetActorTrack()
        {
            return selectionList.Count > 0 ? selectionList[0] : null;
        }

        public List<Actor> GetAllActorEvent()
        {
            return selectionList.Where(x => x is ActorEvent).ToList();
        }

        public ActorEvent GetActorEvent()
        {
            var list = selectionList.Where(x => x is ActorEvent).ToList();
            return list.Count == 1 ? (ActorEvent)list[0] : null;
        }

        public void SelectProperty(Actor actor)
        {
            propertyList.Clear();
            propertyList.Add(actor);
        }

        public void DeselectAllProperty()
        {
            propertyList.Clear();
        }

        public ActorTreeProperty GetActorProperty()
        {
            var list = propertyList.Where(x => x is ActorTreeProperty).ToList();
            return list.Count > 0 ? (ActorTreeProperty)list[0] : null;
        }

        public bool HasSelectProperty(Actor actor)
        {
            return propertyList.Contains(actor);
        }

        public void SelectCondition(Actor actor)
        {
            conditionList.Clear();
            conditionList.Add(actor);
        }

        public void DeselectAllCondition()
        {
            conditionList.Clear();
        }

        public ActorCondition GetActorCondition()
        {
            var list = conditionList.Where(x => x is ActorCondition).ToList();
            return list.Count > 0 ? (ActorCondition)list[0] : null;
        }

        public bool HasSelectCondition(Actor actor)
        {
            return conditionList.Contains(actor);
        }

        public void AddCaptured(Manipulator manipulator)
        {
            if (!captureList.Contains(manipulator))
            {
                captureList.Add(manipulator);
            }
        }

        public void RemoveCaptured(Manipulator manipulator)
        {
            captureList.Remove(manipulator);
        }

        public void DrawOverlay()
        {
            if (captureList.Count > 0)
            {
                using (var itr = captureList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        itr.Current.Overlay(Event.current, this);
                    }
                }
                Repaint();
            }
        }

        public void HandlePreManipulator()
        {
            actorSplitter.HandleManipulatorsEvents(this, Event.current);

            if (actorTreeViewAction != null)
            {
                actorTreeView.HandleManipulatorsEvents(this, Event.current);
            }
        }

        public void HandlePostManipulator()
        {
        }

        public void ShowContextMenu(Actor item, Event evt)
        {
            if (actorTreeViewAction != null && item != null)
            {
                GenericMenu menu = new GenericMenu();
                if (item is ActorGroup group)
                {
                    if (group.itemName == editorResources.groupAnimationName)
                    {
                        menu.AddItem(editorResources.contextNewAnimation, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackAnimationType, editorResources.colorAnimation, editorResources.animationTrackIcon);
                        });
                    }
                    else if (group.itemName == editorResources.groupEffectName)
                    {
                        menu.AddItem(editorResources.contextNewEffect, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackEffectType, editorResources.colorEffect, editorResources.effectTrackIcon);
                        });
                    }
                    else if (group.itemName == editorResources.groupAudioName)
                    {
                        menu.AddItem(editorResources.contextNewAudio, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackAudioType, editorResources.colorAudio, editorResources.audioTrackIcon);
                        });
                    }
                    else if (group.itemName == editorResources.groupCameraName)
                    {
                        menu.AddItem(editorResources.contextNewCamera, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackCameraType, editorResources.colorCamera, editorResources.cameraTrackIcon);
                        });
                    }
                    else if (group.itemName == editorResources.groupMiscName)
                    {
                        menu.AddItem(editorResources.contextNewMisc, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackMiscType, editorResources.colorWhite, editorResources.otherTrackIcon);
                        });
                    }
                    else if (group.itemName == editorResources.groupAttackName)
                    {
                        menu.AddItem(editorResources.contextNewAttack, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackAttackType, editorResources.colorRed, editorResources.attackTrackIcon);
                        });
                    }
                    else if (group.itemName == editorResources.groupIntteruptName)
                    {
                        menu.AddItem(editorResources.contextNewInterrupt, false, () =>
                        {
                            var track = CreateActorTrack(group, editorResources.trackIntteruptType, editorResources.colorInterrupt, editorResources.interruptTrackIcon);
                        });
                    }
                }
                else if (item is ActorTrack track)
                {
                    if (evt != null)
                    {
                        float posX = evt.mousePosition.x;
                        bool enableSignal = true;
                        bool enableDuration = false;
                        string actorType = track.GetActorType();
                        if (actorType == editorResources.trackAnimationType)
                        {
                            enableSignal = false;
                            enableDuration = true;
                        }
                        else if (actorType == editorResources.trackMiscType)
                        {
                            enableDuration = true;
                        }

                        if (enableSignal)
                        {
                            menu.AddItem(editorResources.contextAddEventSignal, false, () =>
                            {
                                CreateActorEvent(track, posX, EEventStyle.Signal, actorType);
                            });
                        }
                        if (enableDuration)
                        {
                            menu.AddItem(editorResources.contextAddEventDuration, false, () =>
                            {
                                CreateActorEvent(track, posX, EEventStyle.Duration, actorType);
                            });
                        }
                    }
                    menu.AddItem(editorResources.contextDelTrack, false, () =>
                    {
                        if (EditorUtility.DisplayDialog("Delete Track", "Are you sure?", "YES", "NO!"))
                        {
                            var parent = track.GetParent();

                            Helper.PushUndo(new Object[] { parent, track }, track.itemName);
                            Helper.PushDestroyUndo(parent, track);

                            track.RemoveAllEvent();
                            parent.RemoveChild(track);

                            BuildTrackIndex(parent);
                        }
                    });
                }
                else if (item is ActorEvent e)
                {
                    if (e.eventProperty != null && e.eventProperty.EventData != null && e.eventProperty.EventData is EventPlayAnim epa)
                    {
                        menu.AddItem(editorResources.contextResetTotalTime, false, () =>
                        {
                            if (actorTreeViewAction != null)
                            {
                                UnityEditor.Animations.AnimatorState state = UnitWrapper.Instance.StateHash[epa.AnimName];
                                length = state.motion.averageDuration;
                            }
                        });
                    }

                    menu.AddItem(editorResources.contextDelEvent, false, () =>
                    {
                        if (EditorUtility.DisplayDialog("Delete Event", "Are you sure?", "YES", "NO!"))
                        {
                            var parent = e.parent;

                            Helper.PushUndo(new Object[] { parent, e }, editorResources.contextDelEvent.text);
                            Helper.PushDestroyUndo(parent, e);

                            parent.RemoveEvent(e);
                        }
                    });
                }
                menu.ShowAsContext();
            }
        }

        public ActorGroup CreateActorGroup(ActorTreeItem parent, string name)
        {
            var group = ScriptableObject.CreateInstance<ActorGroup>();
            group.Init(parent);
            group.itemName = name;

            return group;
        }

        public ActorTrack CreateActorTrack(ActorGroup group, string type, Color color, GUIContent icon, bool forceUndo = true)
        {
            var track = ScriptableObject.CreateInstance<ActorTrack>();
            string operation = string.Format("{0} {1}", type, group.childCount);
            if (forceUndo)
            {
                Helper.RegisterCreatedObjectUndo(track, operation);
                Helper.PushUndo(new Object[] { group, track }, operation);
            }

            track.Init(group);
            track.type = type;
            track.itemName = operation;
            track.colorSwatch = color;
            track.icon = icon;

            return track;
        }

        public ActorEvent CreateActorEvent(ActorTrack parent, float posX, EEventStyle style, string eventTag, bool forceUndo = true)
        {
            var evt = ScriptableObject.CreateInstance<ActorEvent>();

            if (forceUndo)
            {
                string operation = string.Format("{0} Event {1}", parent.GetActorType(), parent.eventList.Count);
                Helper.RegisterCreatedObjectUndo(evt, operation);
                Helper.PushUndo(new Object[] { parent, evt }, operation);
            }

            evt.Init(parent, posX, style, eventTag, forceUndo);
            RefreshTrackIndex(parent);

            return evt;
        }

        public void RefreshTrackIndex(ActorTrack track)
        {
            var group = track.GetParent();
            var index = group.children.IndexOf(track);

            using (var itr = track.eventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.eventProperty.TrackIndex = index;
                }
            }
        }

        public void BuildTrackIndex(ActorTreeItem group)
        {
            using (var itr = group.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    RefreshTrackIndex(itr.Current as ActorTrack);
                }
            }
            if (actorTreeViewAction != null)
            {
                actorTreeViewAction.ForceSort();
            }
        }

    }
}