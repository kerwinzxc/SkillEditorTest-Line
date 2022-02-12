/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowPreview.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-31      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using LitJson;
    using System.IO;
    using System.Text;

    internal sealed partial class ActionWindow
    {
        private float lastTime = 0f;
        private float deltaTime = 1f / 60;

        private List<EffectWrapper> mEffectWrapperList = new List<EffectWrapper>();
        private List<AttackDefWrapper> mAttackDefWrapperList = new List<AttackDefWrapper>();

        private static string FilePath
        {
            get { return Application.dataPath + @"/CLineActionEditor/Demo/Resource/GameData/"; }
        }

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;

            GameConfig.Instance.Init();
            LogMgr.Instance.Init();

            CameraMgr.Instance.Init();
            CameraMgr.Instance.BindCamera();

            ResourceMgr.Instance.Init();
            ObjectPoolMgr.Instance.Init();
            AudioMgr.Instance.Init();

            Deserialize();
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;

            CameraMgr.Instance.Destroy();
            AudioMgr.Instance.Destroy();
            LogMgr.Instance.Destroy();
            GameConfig.Instance.Destroy();
        }

        private void OnEditorUpdate()
        {
            if (!Application.isPlaying && toolState == EToolState.Play)
            {
                Preview(Time.realtimeSinceStartup - lastTime);
                Repaint();
            }

            lastTime = Time.realtimeSinceStartup;
        }

        private void Preview(float fTick)
        {
            fTick *= playbackSpeed;

            Tick(fTick);

            currentTime += fTick;
            if (currentTime >= length)
            {
                currentTime = 0f;
                toolState = EToolState.Stop;
                ResetPreview(fTick);
            }
        }

        private void Tick(float fTick)
        {
            if (actorTreeViewAction == null)
            {
                return;
            }

            //tick event
            using (var itrGroup = actorTreeView.children.GetEnumerator())
            {
                while (itrGroup.MoveNext())
                {
                    using (var itrTrack = itrGroup.Current.children.GetEnumerator())
                    {
                        while (itrTrack.MoveNext())
                        {
                            var track = itrTrack.Current as ActorTrack;
                            using (var itrEvent = track.eventList.GetEnumerator())
                            {
                                while (itrEvent.MoveNext())
                                {
                                    var ae = itrEvent.Current;
                                    if (!ae.hasExecute && ToMillisecond(currentTime) >= ae.eventProperty.TriggerTime)
                                    {
                                        ExecuteEvent(ae, fTick);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //tick animation
            UnitWrapper.Instance.Tick(fTick);

            //tick effect
            using (var itr = mEffectWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Tick(fTick);
                }
            }

            //tick attack def
            using (var itr = mAttackDefWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Tick(fTick);
                }
            }

            //tick audio
            AudioMgr.Instance.Update(fTick);

            //tick camera
            CameraMgr.Instance.LateUpdate(fTick);
        }

        private void ExecuteEvent(ActorEvent evt, float fTick)
        {
            evt.hasExecute = true;
            switch (evt.eventProperty.EventType)
            {
                case EEventDataType.EET_PlayAnim:
                    {
                        var epa = evt.eventProperty.EventData as EventPlayAnim;
                        UnitWrapper.Instance.PlayAnimation(epa.AnimName);
                    }
                    break;
                case EEventDataType.EET_PlayEffect:
                    {
                        var epe = evt.eventProperty.EventData as EventPlayEffect;
                        var ew = new EffectWrapper(epe);
                        mEffectWrapperList.Add(ew);
                    }
                    break;
                case EEventDataType.EET_PlaySound:
                    {
                        var eps = evt.eventProperty.EventData as EventPlaySound;
                        AudioMgr.Instance.PlaySound(eps.SoundName, eps.InstanceCount);
                    }
                    break;
                case EEventDataType.EET_CameraShake:
                    {
                        var ecs = evt.eventProperty.EventData as EventCameraShake;
                        ecs.Execute(null);
                    }
                    break;
                case EEventDataType.EET_CameraEffect:
                    {
                        var ece = evt.eventProperty.EventData as EventCameraEffect;
                        ece.Execute(null);
                    }
                    break;
                case EEventDataType.EET_ShowTrail:
                    {
#if USE_PLUGINWEAPONTRAIL
                        Transform trail = ActionEngine.Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, "WeaponTrail", true);
                        if (trail)
                        {
                            var wt = trail.GetComponent<PluginWeaponTrail.XWeaponTrail>();
                            wt.Activate();
                        }
#endif
                    }
                    break;
                case EEventDataType.EET_HideTrail:
                    {
#if USE_PLUGINWEAPONTRAIL
                        var eht = evt.eventProperty.EventData as EventHideTrail;
                        Transform trail = ActionEngine.Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, "WeaponTrail", true);
                        if (trail)
                        {
                            var wt = trail.GetComponent<PluginWeaponTrail.XWeaponTrail>();
                            wt.StopSmoothly(eht.StopSmoothlyFadeTime);
                        }
#endif
                    }
                    break;
                case EEventDataType.EET_WeaponAttack:
                    {
                        var ewa = evt.eventProperty.EventData as EventWeaponAttack;
                        UnitWrapper.Instance.DoWeaponAttack(ewa.IsLeftDummy);
                    }
                    break;
                case EEventDataType.EET_WeaponIdle:
                    {
                        UnitWrapper.Instance.DoWeaponIdle();
                    }
                    break;
                case EEventDataType.EET_AttackDef:
                    {
                        var adf = evt.eventProperty.EventData as ActionAttackDef;
                        var adw = new AttackDefWrapper(adf);
                        mAttackDefWrapperList.Add(adw);
                    }
                    break;
            }
        }

        private void ResetPreview(float fTick)
        {
            if (actorTreeViewAction == null)
            {
                return;
            }

            // reset event
            using (var itrGroup = actorTreeView.children.GetEnumerator())
            {
                while (itrGroup.MoveNext())
                {
                    using (var itrTrack = itrGroup.Current.children.GetEnumerator())
                    {
                        while (itrTrack.MoveNext())
                        {
                            var track = itrTrack.Current as ActorTrack;
                            using (var itrEvent = track.eventList.GetEnumerator())
                            {
                                while (itrEvent.MoveNext())
                                {
                                    itrEvent.Current.hasExecute = false;
                                }
                            }
                        }
                    }
                }
            }

            //reset effect
            using (List<EffectWrapper>.Enumerator itr = mEffectWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mEffectWrapperList.Clear();

            //reset attack def
            using (List<AttackDefWrapper>.Enumerator itr = mAttackDefWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mAttackDefWrapperList.Clear();

            //reset audio
            AudioMgr.Instance.Destroy();

            //reset camera
            //CameraMgr.Instance.BindCtrl(ECameraCtrlType.None);

            //reset unit position and animation
            UnitWrapper.Instance.UnitWrapperUnit.transform.position = ActionEngine.Helper.Vec3Zero;
            UnitWrapper.Instance.Anim.Update(fTick);
        }

        
    }

}