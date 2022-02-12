/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\UIMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-11      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class UIMgr : Singleton<UIMgr>
    {
        private GameObject mUI2DRoot = null;
        private GameObject mUI2DEventSystem = null;
        private Canvas mUICanvas = null;
        private RectTransform mUICanvasTrans = null;

        public Canvas UICanvas
        {
            get { return mUICanvas; }
        }

        public RectTransform UICanvasTrans
        {
            get { return mUICanvasTrans; }
        }

        public override void Init()
        {
            GameObject go = UI2DRoot;
            //LogMgr.Instance.Log(ELogType.ELT_DEBUG, "UI", go.name);
        }

        public override void Destroy()
        {
            mUI2DRoot = null;
            mUI2DEventSystem = null;
            mUICanvas = null;
        }

        public GameObject UI2DRoot
        {
            get
            {
                if (mUI2DRoot == null)
                {
                    GameObject canvas = GameObject.FindWithTag("UI2DRoot");
                    mUICanvas = canvas.GetComponent<Canvas>();
                    mUICanvasTrans = mUICanvas.GetComponent<RectTransform>();
                    mUI2DRoot = Helper.Find(canvas.transform, "UI2DRoot").gameObject;
                    GameObject.DontDestroyOnLoad(canvas);
                }

                if (mUI2DEventSystem == null)
                {
                    mUI2DEventSystem = GameObject.FindWithTag("UI2DEventSystem");
                    GameObject.DontDestroyOnLoad(mUI2DEventSystem);
                }

                return mUI2DRoot;
            }
        }
    }
}
