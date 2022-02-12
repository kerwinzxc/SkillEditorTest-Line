/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\UIProgressBar.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-26      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class UIProgressBar : MonoBehaviour
    {
        private Image mProgress = null;

        void Awake()
        {
            mProgress = transform.GetComponent<Image>();
            if (mProgress == null)
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "UI", "Need Image component!(%s)", transform.gameObject.name);
            }
            else
            {
                mProgress.fillAmount = 0f;
            }
        }
        
        public float Progress
        {
            get { return mProgress.fillAmount; }
            set { mProgress.fillAmount = value; }
        }
    }
}
