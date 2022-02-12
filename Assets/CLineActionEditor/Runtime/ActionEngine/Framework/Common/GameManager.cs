/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameManager.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-11        SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        private static GameManager ms_instance = null;
        public static GameManager Instance
        {
            get { return ms_instance; }
        }

        void Awake()
        {
            ms_instance = this;

            GameConfig.Instance.Init();
            LogMgr.Instance.Init();

            FrameCore.Instance.Init();
            ResourceMgr.Instance.Init();
            ObjectPoolMgr.Instance.Init();

            PropertyMgr.Instance.Init();
            HudMgr.Instance.Init();

            AttackHitMgr.Instance.Init();
            MessageMgr.Instance.Init();
            TimerMgr.Instance.Init();
            AudioMgr.Instance.Init();
            UIMgr.Instance.Init();
            InputMgr.Instance.Init();
            CameraMgr.Instance.Init();

            UnitMgr.Instance.Init();
            GameStateMgr.Instance.Init();
        }

        void Start()
        {
            GameStateMgr.Instance.ChangeState(EGameStateType.EGST_Main);
        }

        void OnDestroy()
        {
            GameStateMgr.Instance.Destroy();
            UnitMgr.Instance.Destroy();

            CameraMgr.Instance.Destroy();
            InputMgr.Instance.Destroy();
            UIMgr.Instance.Destroy();
            AudioMgr.Instance.Destroy();
            TimerMgr.Instance.Destroy();
            MessageMgr.Instance.Destroy();
            AttackHitMgr.Instance.Destroy();

            HudMgr.Instance.Destroy();
            PropertyMgr.Instance.Destroy();

            ObjectPoolMgr.Instance.Destroy();
            ResourceMgr.Instance.Destroy();

            FrameCore.Instance.Destroy();

            LogMgr.Instance.Destroy();
            GameConfig.Instance.Destroy();
        }

        void FixedUpdate()
        {
            GameStateMgr.Instance.FixedUpdate(Time.fixedDeltaTime);
            UnitMgr.Instance.FixedUpdate(Time.fixedDeltaTime);
            AttackHitMgr.Instance.FixedUpdate(Time.fixedDeltaTime);
        }

        void Update()
        {
            PreUpdate(Time.deltaTime);
            InputMgr.Instance.Update(Time.deltaTime);
            GameStateMgr.Instance.Update(Time.deltaTime);
            UnitMgr.Instance.Update(Time.deltaTime);
            AttackHitMgr.Instance.Update(Time.deltaTime);
            MessageMgr.Instance.Update(Time.deltaTime);
            TimerMgr.Instance.Update(Time.deltaTime);
            AudioMgr.Instance.Update(Time.deltaTime);
            PostUpdate(Time.deltaTime);
        }

        void PreUpdate(float fTick)
        {

        }

        void PostUpdate(float fTick)
        {
            InputMgr.Instance.PostUpdate(fTick);
        }

        void LateUpdate()
        {
            GameStateMgr.Instance.LateUpdate(Time.deltaTime);
            CameraMgr.Instance.LateUpdate(Time.deltaTime);
        }

        void OnGUI()
        {
            LogMgr.Instance.OnGUI();
        }

        void OnApplicationQuit()
        {
            ms_instance = null;
        }

        void OnApplicationPause(bool pause)
        {

        }

        void OnApplicationFocus(bool focus)
        {

        }
    }
}

