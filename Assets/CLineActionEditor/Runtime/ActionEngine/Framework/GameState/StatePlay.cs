/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameState\StatePlay.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

using System;

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class StatePlay : IGameState
    {
        private GameObject mLoadingPanel = null;
        private UIProgressBar mLoadingProgress = null;
        private string mSceneName = string.Empty;

        public EGameStateType StateType
        {
            get
            {
                return EGameStateType.EGST_Play;
            }
        }

        public void Init()
        {
            
        }

        public void Destroy()
        {
            
        }

        public void Enter(List<object> context)
        {
            GameObject go = ResourceMgr.Instance.LoadObject<GameObject>("/Prefabs/UI/UILoadingPanel.prefab") as GameObject;
            mLoadingPanel = GameObject.Instantiate(go, UIMgr.Instance.UI2DRoot.transform) as GameObject;

            mLoadingProgress = mLoadingPanel.GetComponentInChildren<UIProgressBar>();
            mLoadingProgress.Progress = 0f;

            mSceneName = (string)context[0];
            HelperAsynLoading.Instance.LoadSceneAsyn(mSceneName, onSceneLoading, onSceneLoaded);
        }

        public void Exit()
        {
            AudioMgr.Instance.StopMusic(true, 5f);
        }

        public void Update(float fTick)
        {
            HelperAsynLoading.Instance.Update(fTick);
        }

        public void FixedUpdate(float fTick)
        {
             
        }

        public void LateUpdate(float fTick)
        {
            
        }

        public void OnMessage(Message msg)
        {
            
        }

        private void onSceneLoading(float percent)
        {
            // 显示进度分片加载
            mLoadingProgress.Progress = percent;
        }

        private void onSceneLoaded()
        {
            switch (mSceneName)
            {
                case "rpg":
                    {
                        Unit p = UnitMgr.Instance.CreateUnit(EUnitType.EUT_Player, "1", Vector3.zero, 0f, ECampType.EFT_Friend);
                        // camera
                        CameraMgr.Instance.BindCamera();
                        CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_SmoothFollow, p.UObject);
                        InputMgr.Instance.Controller = p;

                        p.UseWeapon("WQ01001");

                        // 添加天赋 - 移动速度+10%
                        p.AddBuff("1");

                        UnitMgr.Instance.CreateUnit(EUnitType.EUT_Monster, "1", new Vector3(0, 0, 12f), 0, ECampType.EFT_Enemy);

                        AudioMgr.Instance.PlayMusic("/Prefabs/Audio/bgm_battle1.prefab", true, 5f);
                    }
                    break;
                case "act":
                    {
                        Unit p = UnitMgr.Instance.CreateUnit(EUnitType.EUT_Player, "2", Vector3.zero, 0f, ECampType.EFT_Friend);
                        // camera
                        CameraMgr.Instance.BindCamera();
                        CameraMgr.Instance.BindCtrl(ECameraCtrlType.ECCT_SmoothFollow, p.UObject);
                        InputMgr.Instance.Controller = p;

                        UnitMgr.Instance.CreateUnit(EUnitType.EUT_Monster, "2", new Vector3(0, 0, 12f), 0, ECampType.EFT_Enemy);

                        AudioMgr.Instance.PlayMusic("/Prefabs/Audio/bgm_battle1.prefab", true, 5f);
                    }
                    break;
            }

            GameObject.Destroy(mLoadingPanel);
            mLoadingPanel = null;

            InputMgr.Instance.EnableInput = true;
        }
    }
}
