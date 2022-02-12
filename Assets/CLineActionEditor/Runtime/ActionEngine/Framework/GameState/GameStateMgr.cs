/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameState\GameStateMgr.cs
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
    using System;
    using System.Collections.Generic;

    public enum EGameStateType
    {
        EGST_None,
        EGST_Main,
        EGST_Play,
    }

    public sealed class GameStateMgr : Singleton<GameStateMgr>
    {
        private IGameState mToState = null;
        private IGameState mCurState = null;

        private System.Action mHandlerBeforStateChange = null;
        private System.Action mHandlerAfterStateChange = null;

        private List<object> mContext = new List<object>();

        private Dictionary<EGameStateType, IGameState> mGameStateHash = new Dictionary<EGameStateType, IGameState>();

        public System.Action HandlerBeforStateChange
        {
            set { mHandlerBeforStateChange = value; }
        }

        public System.Action HandlerAfterStateChange
        {
            set { mHandlerAfterStateChange = value; }
        }

        public override void Init()
        {
            mGameStateHash.Add(EGameStateType.EGST_Main, new StateMain());
            mGameStateHash.Add(EGameStateType.EGST_Play, new StatePlay());

            using (var itr = mGameStateHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Value.Init();
                }
            }
        }

        public override void Destroy()
        {
            using (var itr = mGameStateHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Value.Destroy();
                }
            }
            mGameStateHash.Clear();

            mToState = null;
            mCurState = null;
            mHandlerBeforStateChange = null;
            mHandlerAfterStateChange = null;
        }

        public void ChangeState(EGameStateType to, params object[] context)
        {
            mToState = mGameStateHash[to];

            mContext.Clear();
            mContext.AddRange(context);
        }

        public void Update(float fTick)
        {
            if (mToState != mCurState)
            {
                if (null != mHandlerBeforStateChange)
                    mHandlerBeforStateChange();
                if (null != mCurState)
                    mCurState.Exit();
                if (null != mToState)
                    mToState.Enter(mContext);
                mCurState = mToState;
                if (null != mHandlerAfterStateChange)
                    mHandlerAfterStateChange();
            }

            if (null != mCurState)
                mCurState.Update(fTick);
        }

        public void FixedUpdate(float fTick)
        {
            if (null != mCurState)
                mCurState.FixedUpdate(fTick);
        }

        public void LateUpdate(float fTick)
        {
            if (null != mCurState)
                mCurState.LateUpdate(fTick);
        }
    }
}


