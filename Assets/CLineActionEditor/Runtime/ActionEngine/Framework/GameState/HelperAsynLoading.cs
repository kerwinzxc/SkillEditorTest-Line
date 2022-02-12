/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\GameState\HelperAsynLoading.cs
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

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;
    using System.Collections;

    public sealed class HelperAsynLoading : Singleton<HelperAsynLoading>
    {
        private bool mIsDone = false;
        private float mLoadStep = 0f;
        private float mProgress = 0f;
        private AsyncOperation mAsynLoad = null;
        private System.Action mHandlerFinished = null;
        private Action<float> mHandlerUpdate = null;

        public override void Init()
        {
            
        }

        public override void Destroy()
        {
            
        }

        public void LoadSceneAsyn(string name, Action<float> update, System.Action finish, float step = 0.01f)
        {
            mHandlerUpdate = update;
            mHandlerFinished = finish;
            mLoadStep = step;
            mIsDone = false;
            mProgress = 0f;
            mAsynLoad = null;

            GameManager.Instance.StartCoroutine(Load(name));
        }

        public void Update(float fTick)
        {
            if (mAsynLoad == null || mIsDone)
                return;

            float progress = mProgress + mLoadStep;
            mProgress = Mathf.Clamp(progress, 0f, mAsynLoad.progress);

            if (null != mHandlerUpdate)
                mHandlerUpdate(mProgress);

            if (progress >= 1f)
            {
                mIsDone = true;
                if (null != mHandlerFinished)
                    mHandlerFinished();
            }
        }

        private IEnumerator Load(string name)
        {
            mAsynLoad = Application.LoadLevelAsync(name);
            yield return mAsynLoad;
        }
    }
}
