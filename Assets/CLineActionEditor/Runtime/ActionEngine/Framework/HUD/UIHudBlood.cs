/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\UIHudBlood.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-3-5      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;

    public sealed class UIHudBlood : MonoBehaviour
    {
        public UIProgressBar Green;
        public UIProgressBar Red;

        private Transform mOwner = null;

        public UIProgressBar Init(Transform owner, EBloodType eType)
        {
            mOwner = owner;
            UIProgressBar progress = null;
            switch (eType)
            {
                case EBloodType.EBT_Green:
                    progress = Green;
                    Green.gameObject.SetActive(true);
                    Red.gameObject.SetActive(false);
                    break;
                case EBloodType.EBT_Red:
                    progress = Red;
                    Green.gameObject.SetActive(false);
                    Red.gameObject.SetActive(true);
                    break;
            }

            progress.Progress = 1f;

            return progress;
        }

        public void OnDestroy()
        {
            mOwner = null;
        }

        private void Update()
        {
            if (mOwner == null || Camera.main == null)
                return;

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, mOwner.position);
            Vector2 uiPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Instance.UICanvasTrans, screenPos, UIMgr.Instance.UICanvas.worldCamera, out uiPos))
            {
                RectTransform tr = transform as RectTransform;
                tr.anchoredPosition = uiPos;
            }
        }
    }
}