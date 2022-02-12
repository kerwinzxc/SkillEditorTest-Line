/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameState\StateMain.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-26      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class StateMain : IGameState
    {
        private GameObject mMainPanel = null;

        public EGameStateType StateType
        {
            get { return EGameStateType.EGST_Main; }
        }

        public void Init()
        {
            MessageMgr.Instance.Register("CHANGE_SCENE", OnMessage);
        }

        public void Destroy()
        {
            MessageMgr.Instance.Unregister("CHANGE_SCENE", OnMessage);
        }

        public void Enter(List<object> context)
        {
            InputMgr.Instance.EnableInput = false;

            if (mMainPanel == null)
            {
                GameObject go = ResourceMgr.Instance.LoadObject<GameObject>("/Prefabs/UI/UIMainPanel.prefab") as GameObject;
                mMainPanel = GameObject.Instantiate(go, UIMgr.Instance.UI2DRoot.transform) as GameObject;
            }
            else
            {
                mMainPanel.SetActive(true);
            }
        }

        public void Exit()
        {
            mMainPanel.SetActive(false);
        }

        public void Update(float fTick)
        {

        }

        public void FixedUpdate(float fTick)
        {

        }

        public void LateUpdate(float fTick)
        {

        }

        public void OnMessage(Message msg)
        {
            string scene = (string)msg.GetArg("Scene");
            GameStateMgr.Instance.ChangeState(EGameStateType.EGST_Play, scene);
        }

    }
}