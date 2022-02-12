/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowGUI.cs
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

using UnityEngine.Serialization;

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEngine;
    using UnityEditor;

    public enum ELanguageType
    {
        CN = 0,
        EN,
    }

    public enum EToolState
    {
        Stop = 0,
        Play,
        Pause,
    }

    internal sealed partial class ActionWindow
    {
        [SerializeField] private ELanguageType language = ELanguageType.CN;
        [SerializeField] private float playbackSpeed = 1f;
        [SerializeField] private bool prettyPrint = true;
        [SerializeField] private string animatorTypeName = "SuperCLine.ActionEngine.UnitUnityAnimator";
        [SerializeReference] private ActorSplitter actorSplitter = null;
        [SerializeReference] private ActorTreeItem actorTreeView = null;
        [System.NonSerialized] private EToolState toolState = EToolState.Stop;
        [System.NonSerialized] public Vector2 scrollPosition;
        [System.NonSerialized] public Action actorTreeViewAction = null;

        private void OnGUI()
        {
            InitGUI();

            if (PerformUndo())
                return;

            UpdateGUI();
            DrawDuration(EAreaType.Header, length);
            DrawToolbar();
            HandlePreManipulator();
            DrawTreeView();
            DrawSplitter();
            DrawTimeline();
            DrawDuration(EAreaType.Lines, length);
            DrawTimeCursor(EAreaType.Lines);
            DrawTimelineRange();
            DrawTimeCursor(EAreaType.Header);
            DrawInspector();
            DrawOverlay();
            HandlePostManipulator();
        }

        private void InitGUI()
        {
            rectWindow = position;
            rectBody = new Rect(0, toobarHeight, position.width - inspectorWidth, position.height - toobarHeight);
            rectClient = new Rect(0, rectBody.y + timeRulerHeight, rectBody.width, rectBody.height - timeRulerHeight);
            rectHeader = new Rect(rectBody.x, rectBody.y + timeRulerHeight, headerWidth, rectBody.height);
            rectTimeRuler = new Rect(rectBody.x + headerWidth + timelineOffsetX, rectBody.y, position.width - headerWidth - inspectorWidth - timelineOffsetX, timeRulerHeight);
            rectTimeline = new Rect(rectTimeRuler.x, rectTimeRuler.y, rectTimeRuler.width, rectBody.height);
            rectTimeArea = new Rect(rectTimeline.x, rectTimeline.y + timeRulerHeight, rectTimeline.width, rectTimeline.height - timeRulerHeight);
            rectContent = new Rect(rectBody.x + headerWidth, rectBody.y + timeRulerHeight, position.width - headerWidth - inspectorWidth, rectBody.height - timeRulerHeight);
            rectClientView = new Rect(0, 0, rectClient.width, rectClient.height);

            if (actorSplitter == null)
            {
                actorSplitter = ScriptableObject.CreateInstance<ActorSplitter>();
                actorSplitter.Init();

                timeFormat = TimeFormat.Frames;
            }

            if (actorTreeView == null)
            {
                actorTreeView = ScriptableObject.CreateInstance<ActorTreeItem>();
                actorTreeView.Init(null);
                actorTreeView.AddManipulator(new ShortcutManipulator());
                actorTreeView.AddManipulator(new ActorEventHandleManipulator(actorTreeView));
                actorTreeView.AddManipulator(new ActorEventManipulator(actorTreeView));
                actorTreeView.AddManipulator(new ActorTrackManipulator(actorTreeView));
                actorTreeView.AddManipulator(new ContextMenuManipulator(actorTreeView));
                actorTreeView.AddManipulator(new ZoomPanManipulator());
                actorTreeView.AddManipulator(new DragBoxSelectManipulator(actorTreeView));

                CreateActorGroup(actorTreeView, editorResources.groupAnimationName);
                CreateActorGroup(actorTreeView, editorResources.groupEffectName);
                CreateActorGroup(actorTreeView, editorResources.groupAudioName);
                CreateActorGroup(actorTreeView, editorResources.groupCameraName);
                CreateActorGroup(actorTreeView, editorResources.groupMiscName);
                CreateActorGroup(actorTreeView, editorResources.groupAttackName);
                CreateActorGroup(actorTreeView, editorResources.groupIntteruptName);
            }

        }

        private void UpdateGUI()
        {
            var height = 0f;
            var list = actorTreeView.BuildRows();
            for (int i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                item.BuildRect(height);
                height += item.totalHeight;
            }

            horizontalScrollbarHeight =
                GUI.skin.horizontalScrollbar.fixedHeight + GUI.skin.horizontalScrollbar.margin.top;

            verticalScrollbarWidth = (height + 19 >= rectClient.height)
                ? GUI.skin.verticalScrollbar.fixedWidth + GUI.skin.verticalScrollbar.margin.left
                : 0;

            rectClient.height -= horizontalScrollbarHeight;
            rectClient.width -= verticalScrollbarWidth;

            rectClientView.height = height + 5;
            rectClientView.width -= verticalScrollbarWidth;

            rectTimeRuler.width -= 2 * verticalScrollbarWidth;

            rectTimeline.width -= 2 * verticalScrollbarWidth;
            rectTimeline.height -= horizontalScrollbarHeight;

            rectTimeArea.width -= 2 * verticalScrollbarWidth;
            rectTimeArea.height -= horizontalScrollbarHeight;
        }

        public void PreviewPlay()
        {
            toolState = (toolState != EToolState.Play) ? EToolState.Play : EToolState.Pause;
        }

        public void PreviewStop()
        {
            toolState = EToolState.Stop;
            ResetPreview(length - currentTime);
            currentTime = 0f;
        }

        public void PreviewStepBack()
        {
            Preview(-deltaTime);
            Repaint();
        }

        public void PreviewStepForward()
        {
            Preview(deltaTime);
            Repaint();
        }

        private void Save()
        {
            switch (inspectorID)
            {
                case 0:
                    SaveRole();
                    break;
                case 1:
                    SaveWeapon();
                    break;
                case 2:
                    SaveAction();
                    break;
                case 3:
                    SaveAI();
                    break;
                case 4:
                    SaveBuff();
                    break;
            }
        }

        private void DrawToolbar()
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    if (GUILayout.Button(editorResources.stepBack, EditorStyles.toolbarButton))
                    {
                        PreviewStepBack();
                    }
                    GUIContent toolContent = (toolState == EToolState.Play) ? editorResources.pause : editorResources.play;
                    if (GUILayout.Button(toolContent, EditorStyles.toolbarButton))
                    {
                        PreviewPlay();
                    }
                    if (GUILayout.Button(editorResources.stepForward, EditorStyles.toolbarButton))
                    {
                        PreviewStepForward();
                    }
                    if (GUILayout.Button(editorResources.stop, EditorStyles.toolbarButton))
                    {
                        PreviewStop();
                    }
                    if (GUILayout.Button(editorResources.save, EditorStyles.toolbarButton))
                    {
                        Save();
                    }
                    if (GUILayout.Button(editorResources.help, EditorStyles.toolbarButton))
                    {
                        Application.OpenURL("https://www.supercline.com/");
                    }
                    GUILayout.Space(20);
                    playbackSpeed = GUILayout.HorizontalSlider(playbackSpeed, 0.1f, 2f, GUILayout.Width(150));
                    GUILayout.Label(playbackSpeed.ToString("0.00x"));
                    GUILayout.FlexibleSpace();
                    if (EditorGUILayout.DropdownButton(editorResources.options, FocusType.Keyboard, EditorStyles.toolbarDropDown))
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Frames"), timeFormat == TimeFormat.Frames, () =>
                        {
                            frameRate = 60; timeFormat = TimeFormat.Frames;
                        });
                        menu.AddItem(new GUIContent("Seconds"), timeFormat == TimeFormat.Seconds, () =>
                        {
                            frameRate = 100; timeFormat = TimeFormat.Seconds;
                        });
                        if (timeFormat == TimeFormat.Frames)
                        {
                            menu.AddSeparator("");
                            menu.AddDisabledItem(new GUIContent("Frame rate"));
                            menu.AddItem(new GUIContent("60"), frameRate.Equals(60), () =>
                            {
                                frameRate = 60; timeFormat = TimeFormat.Frames;
                            });
                            menu.AddItem(new GUIContent("50"), frameRate.Equals(50), () =>
                            {
                                frameRate = 50; timeFormat = TimeFormat.Frames;
                            });
                            menu.AddItem(new GUIContent("30"), frameRate.Equals(30), () =>
                            {
                                frameRate = 30; timeFormat = TimeFormat.Frames;
                            });
                        }
                        else
                        {
                            menu.AddSeparator("");
                            menu.AddDisabledItem(new GUIContent("Second"));
                            menu.AddItem(new GUIContent("0.1"), frameRate.Equals(10), () =>
                            {
                                frameRate = 10; timeFormat = TimeFormat.Seconds;
                            });
                            menu.AddItem(new GUIContent("0.01"), frameRate.Equals(100), () =>
                            {
                                frameRate = 100; timeFormat = TimeFormat.Seconds;
                            });
                            menu.AddItem(new GUIContent("0.001"), frameRate.Equals(1000), () =>
                            {
                                frameRate = 1000; timeFormat = TimeFormat.Seconds;
                            });
                        }
                        menu.AddSeparator("");
                        menu.AddDisabledItem(new GUIContent("Custom"));
                        menu.AddItem(new GUIContent("Enable Snap"), enableSnap, () =>
                        {
                            enableSnap = !enableSnap;
                        });
                        menu.AddItem(new GUIContent("Enable Pretty Print"), prettyPrint, () =>
                        {
                            prettyPrint = !prettyPrint;
                        });

                        menu.AddItem(new GUIContent("Playback Speed/0.1x"), playbackSpeed.Equals(0.1f), () => 
                        {
                            playbackSpeed = 0.1f;
                        });
                        menu.AddItem(new GUIContent("Playback Speed/0.5x"), playbackSpeed.Equals(0.5f), () =>
                        {
                            playbackSpeed = 0.5f;
                        });
                        menu.AddItem(new GUIContent("Playback Speed/1x"), playbackSpeed.Equals(1f), () =>
                        {
                            playbackSpeed = 1f;
                        });
                        menu.AddItem(new GUIContent("Playback Speed/1.5x"), playbackSpeed.Equals(1.5f), () =>
                        {
                            playbackSpeed = 1.5f;
                        });
                        menu.AddItem(new GUIContent("Playback Speed/2x"), playbackSpeed.Equals(2f), () =>
                        {
                            playbackSpeed = 2f;
                        });

                        menu.AddItem(new GUIContent("Language/中文"), language == ELanguageType.CN, () =>
                        {
                            language = ELanguageType.CN;
                        });
                        menu.AddItem(new GUIContent("Language/English"), language == ELanguageType.EN, () =>
                        {
                            language = ELanguageType.EN;
                        });

                        foreach (var typeName in Utility.Type.GetRuntimeTypeNames(typeof(IUnitAnimator)))
                        {
                            menu.AddItem(new GUIContent("Animator/"+typeName), animatorTypeName == typeName, () =>
                            {
                                animatorTypeName = typeName;
                            });
                        }

                        menu.ShowAsContext();
                    }
                }
            }
            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Width(headerWidth)))
                {
                    GUILayout.FlexibleSpace();
                    string timeStr = FormatTime(currentTime);
                    GUILayout.Label(timeStr);
                }
            }
        }
        
        

        private void DrawTreeView()
        {
            EditorGUI.DrawRect(rectClient, editorResources.colorSequenceBackground);
            scrollPosition = GUI.BeginScrollView(rectClient, scrollPosition, rectClientView, GUIStyle.none, GUI.skin.verticalScrollbar);
            {
                actorTreeView.Draw();
            }
            GUI.EndScrollView();
        }

        private void DrawSplitter()
        {
            actorSplitter.Draw();
        }

        private bool PerformUndo()
        {
            if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")
            {
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl = 0;
                Event.current.Use();
                return true;
            }

            return false;
        }

        private void DrawTimelineRange()
        {
            var subTimelineOverlayColor = editorResources.colorSubSequenceOverlay;

            if (viewTimeMin < 0.18f)
            {
                Rect before = new Rect(headerWidth - 1, rectClient.y, 6, rectTimeArea.height);
                EditorGUI.DrawRect(before, subTimelineOverlayColor);
            }

            if (viewTimeMax > length)
            {
                float s = Time2Pos(length);
                float e = Time2Pos(viewTimeMax);
                float x = Mathf.Clamp(rectTimeArea.x + s, rectTimeArea.x, float.MaxValue);
                Rect after = new Rect(x, rectClient.y, e - Mathf.Clamp(s, 0, float.MaxValue), rectTimeArea.height);
                EditorGUI.DrawRect(after, subTimelineOverlayColor);
            }
        }


        private void BuildActorEventTree(ActionEvent evt, string groupName, string trackType, Color trackColor, GUIContent trackIcon)
        {
            ActorGroup group = null;
            using (var itr = actorTreeView.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.itemName == groupName)
                    {
                        group = itr.Current as ActorGroup;
                        break;
                    }
                }
            }

            ActorTrack track = null;
            using (var itr = group.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.ActorIndex == evt.TrackIndex)
                    {
                        track = itr.Current as ActorTrack;
                        break;
                    }
                }
            }

            if  (track == null)
            {
                track = CreateActorTrack(group, trackType, trackColor, trackIcon, false);
                track.ActorIndex = evt.TrackIndex;
                track.itemName = evt.TrackName;
            }

            float posX = Time2Pos(ToSecond(evt.TriggerTime));
            var style = evt.TriggerType == ETriggerType.Signal ? EEventStyle.Signal : EEventStyle.Duration;
            var ae = CreateActorEvent(track, posX, style, trackType, false);
            ae.eventProperty = evt;
        }

        public void BuildTreeView(Action ac)
        {
            actorTreeViewAction = ac;

            ac.ForceSort();
            using (var itr = ac.EventList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    switch (itr.Current.EventType)
                    {
                        case EEventDataType.EET_CameraEffect:
                        case EEventDataType.EET_CameraShake:
                            {
                                BuildActorEventTree(itr.Current, editorResources.groupCameraName, editorResources.trackCameraType, editorResources.colorCamera, editorResources.cameraTrackIcon);
                            }
                            break;
                        case EEventDataType.EET_PlayAnim:
                            {
                                BuildActorEventTree(itr.Current, editorResources.groupAnimationName, editorResources.trackAnimationType, editorResources.colorAnimation, editorResources.animationTrackIcon);
                            }
                            break;
                        case EEventDataType.EET_PlayEffect:
                            {
                                BuildActorEventTree(itr.Current, editorResources.groupEffectName, editorResources.trackEffectType, editorResources.colorEffect, editorResources.effectTrackIcon);
                            }
                            break;
                        case EEventDataType.EET_PlaySound:
                            {
                                BuildActorEventTree(itr.Current, editorResources.groupAudioName, editorResources.trackAudioType, editorResources.colorAudio, editorResources.audioTrackIcon);
                            }
                            break;
                        default:
                            {
                                BuildActorEventTree(itr.Current, editorResources.groupMiscName, editorResources.trackMiscType, editorResources.colorWhite, editorResources.otherTrackIcon);
                            }
                            break;
                    }
                }
            }
            using (var itr = ac.InterruptList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    BuildActorEventTree(itr.Current, editorResources.groupIntteruptName, editorResources.trackIntteruptType, editorResources.colorInterrupt, editorResources.interruptTrackIcon);
                }
            }
            using (var itr = ac.AttackList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    BuildActorEventTree(itr.Current, editorResources.groupAttackName, editorResources.trackAttackType, editorResources.colorRed, editorResources.attackTrackIcon);
                }
            }

        }

        public void ClearTreeView()
        {
            actorTreeViewAction = null;

            using (var itr = actorTreeView.children.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.children.Clear();
                }
            }
        }

    }

}