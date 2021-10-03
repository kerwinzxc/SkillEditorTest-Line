/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\ActionWindowPreview.cs
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
    using System.IO;
    using LitJson;

    public partial class ActionWindow : EditorWindow
    {
        private Transform mCamera;
        private Vector3 mCameraOrignalPos;

        private List<EffectWrapper> mEffectWrapperList = new List<EffectWrapper>();
        private List<AttackDefWrapper> mAttackDefWrapperList = new List<AttackDefWrapper>();

        private void OnInitialize()
        {
            GameConfig.Instance.Init();
            LogMgr.Instance.Init();

            //gameobject2stringDic.Clear();
            CameraMgr.Instance.Destroy();
            CameraMgr.Instance.Init();

            mCamera = GameObject.FindWithTag("MainCamera").transform;
            CameraMgr.Instance.BindCamera(mCamera);
            CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_None);

            ResourceMgr.Instance.Init();
            ObjectPoolMgr.Instance.Init();
            AudioMgr.Instance.Init();

            Deserialize();
        }

        private void OnDestroy()
        {
            CameraMgr.Instance.BindCamera(null);
            CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_None);
            CameraMgr.Instance.Destroy();

            AudioMgr.Instance.Destroy();
            ObjectPoolMgr.Instance.Destroy();
            LogMgr.Instance.Destroy();
            GameConfig.Instance.Destroy();
        }

        private void Tick(float fTick)
        {
            if (mCurActionWrapper == null) return;

            // tick event
            using (Dictionary<EActorType, Actor>.Enumerator itr1 = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                            {
                                while (itr3.MoveNext())
                                {
                                    if (itr3.Current is ActorEvent)
                                    {
                                        ActorEvent ae = itr3.Current as ActorEvent;

                                        if (!ae.HasExecute && (mTime * 1000) >= ae.Event.TriggerTime)
                                        {
                                            ExecuteEvent(ae, fTick);
                                        }
                                    }
                                    else if (itr3.Current is ActorEventAttackDef)
                                    {
                                        ActorEventAttackDef aead = itr3.Current as ActorEventAttackDef;

                                        if (!aead.HasExecute && (mTime * 1000) >= aead.Attack.TriggerTime)
                                        {
                                            ExecuteEvent(aead, fTick);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Tick animation
            UnitWrapper.Instance.Tick(fTick);

            // Tick effect
            using (List<EffectWrapper>.Enumerator itr = mEffectWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Tick(fTick);
                }
            }

            // Tick effect
            using (List<AttackDefWrapper>.Enumerator itr = mAttackDefWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Tick(fTick);
                }
            }

            // Tick sound
            AudioMgr.Instance.Update(fTick);

            // tick camera
            CameraMgr.Instance.LateUpdate(fTick);
        }

        private void ExecuteEvent(ActorEventAttackDef evt, float fTick)
        {
            evt.HasExecute = true;
            AttackDefWrapper adw = new AttackDefWrapper(evt.Attack);
            mAttackDefWrapperList.Add(adw);
        }

        private void ExecuteEvent(ActorEvent evt, float fTick)
        {
            evt.HasExecute = true;

            switch (evt.Event.EventType)
            {
                case EEventType.EET_PlayAnim:
                    {
                        EventPlayAnim epa = evt.Event.EventData as EventPlayAnim;
                        UnitWrapper.Instance.PlayAnimation(epa.AnimName);
                    }
                    break;
                case EEventType.EET_PlayEffect:
                    {
                        EventPlayEffect epe = evt.Event.EventData as EventPlayEffect;
                        EffectWrapper ew = new EffectWrapper(epe);
                        mEffectWrapperList.Add(ew);
                    }
                    break;
                case EEventType.EET_PlaySound:
                    {
                        EventPlaySound eps = evt.Event.EventData as EventPlaySound;
                        AudioMgr.Instance.PlaySound(eps.SoundName, eps.InstanceCount);
                    }
                    break;
                case EEventType.EET_CameraShake:
                    {
                        EventCameraShake ecs = evt.Event.EventData as EventCameraShake;
                        mCameraOrignalPos = mCamera.position;
                        CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_Shake, ecs.Duration, ecs.DisableX, ecs.DisableY);
                        ecs.Execute(null);
                    }
                    break;
                case EEventType.EET_CameraEffect:
                    {
                        EventCameraEffect ece = evt.Event.EventData as EventCameraEffect;
                        ece.Execute(null);
                    }
                    break;
                case EEventType.EET_ShowTrail:
                    {
#if USE_PLUGINWEAPONTRAIL
                        Transform trail = Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, "WeaponTrail", true);
                        if (trail)
                        {
                            var wt = trail.GetComponent<Xft.XWeaponTrail>();
                            wt.Activate();
                        }
#endif
                    }
                    break;
                case EEventType.EET_HideTrail:
                    {
#if USE_PLUGINWEAPONTRAIL
                        EventHideTrail e = evt.Event.EventData as EventHideTrail;
                        Transform trail = Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, "WeaponTrail", true);
                        if (trail)
                        {
                            var wt = trail.GetComponent<Xft.XWeaponTrail>();
                            wt.StopSmoothly(e.StopSmoothlyFadeTime);
                        }
#endif
                    }
                    break;
                case EEventType.EET_WeaponAttack:
                    {
                        EventWeaponAttack e = evt.Event.EventData as EventWeaponAttack;
                        UnitWrapper.Instance.DoWeaponAttack(e.IsLeftDummy);
                    }
                    break;
                case EEventType.EET_WeaponIdle:
                    {
                        UnitWrapper.Instance.DoWeaponIdle();
                    }
                    break;
            }
        }

        private void ResetPreview()
        {
            if (mCurActionWrapper == null) return;

            using (Dictionary<EActorType, Actor>.Enumerator itr1 = mCurActionWrapper.ActionActorHash.GetEnumerator())
            {
                while (itr1.MoveNext())
                {
                    using (List<Actor>.Enumerator itr2 = itr1.Current.Value.ActorList.GetEnumerator())
                    {
                        while (itr2.MoveNext())
                        {
                            using (List<Actor>.Enumerator itr3 = itr2.Current.ActorList.GetEnumerator())
                            {
                                while (itr3.MoveNext())
                                {
                                    if (itr3.Current is ActorEvent)
                                    {
                                        ActorEvent ae = itr3.Current as ActorEvent;
                                        ae.HasExecute = false;

                                        switch (ae.Event.EventType)
                                        {
                                            case EEventType.EET_CameraEffect:
                                                CameraEffect.StopCameraEffect(ECameraEffectType.ECET_GrayScale);
                                                break;
                                            case EEventType.EET_CameraShake:
                                                mCamera.position = mCameraOrignalPos;
                                                break;
                                        }
                                    }
                                    else if (itr3.Current is ActorEventAttackDef)
                                    {
                                        ActorEventAttackDef aead = itr3.Current as ActorEventAttackDef;
                                        aead.HasExecute = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // reset effect
            using (List<EffectWrapper>.Enumerator itr = mEffectWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mEffectWrapperList.Clear();

            using (List<AttackDefWrapper>.Enumerator itr = mAttackDefWrapperList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Dispose();
                }
            }
            mAttackDefWrapperList.Clear();

            // reset audio
            AudioMgr.Instance.Destroy();

            // reset camera
            CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_None);
        }

        private void AttachUnit()
        {
            if (mCurProperty == null) return;

            if (!UnitWrapper.Instance.BuildUnit(mCurProperty)) return;

            // Load action list
            DeserializeAction();
        }

        private void AttachCarrier()
        {
            if (!UnitWrapper.Instance.IsReady) return;

            GameObject go = ResourceMgr.Instance.LoadObject(mCarrierName, typeof(GameObject)) as GameObject;
            if (go == null)
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ActionEditor", "carrier is not exist");
                return;
            }

            GameObject carrierObj = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;
            Animator animator = carrierObj.gameObject.GetComponent<Animator>();
            UnitWrapper.Instance.Anim = animator;
            UnitWrapper.Instance.GetAllState();
            UnitWrapper.Instance.GetAllParameter();

            GameObject dummy = Helper.Find(carrierObj.transform, "Player_Dummy").gameObject;
            GameObject player = UnitWrapper.Instance.UnitWrapperUnit;
            if (dummy != null)
            {
                Helper.AddChild(player, dummy);
                player.transform.localPosition = Vector3.zero;
                player.transform.localRotation = Quaternion.identity;
                // hide player
                player.SetActive(false);
            }
            UnitWrapper.Instance.UnitWrapperUnit = carrierObj;
        }

        private void AttachWeapon()
        {
            if (UnitWrapper.Instance.UnitWrapperUnit == null)
            {
                LogMgr.Instance.Log(ELogType.ELT_WARNING, "ActionEditor", "Create unit firstly!!!");
                return;
            }

            WeaponProperty wp = mCurProperty as WeaponProperty;
            if (wp == null)
            {
                LogMgr.Instance.Log(ELogType.ELT_WARNING, "ActionEditor", "Please select weapon!!!");
                return;
            }

            if (string.IsNullOrEmpty(wp.Prefab))
            {
                LogMgr.Instance.Log(ELogType.ELT_WARNING, "ActionEditor", "Please set weapon property!!!");
                return;
            }

            UnitWrapper.Instance.BuildWeapon(wp);
        }

    }

}
