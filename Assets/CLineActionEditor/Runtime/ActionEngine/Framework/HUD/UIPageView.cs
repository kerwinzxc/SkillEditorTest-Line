/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\UIPageView.cs
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
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;

    public class UIPageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private bool mIsDragging = false;
        private bool mStopMove = true;
        private float mMoveTime = 0f;
        private int mCurPageIndex = -1;
        private float mStartDragPos;
        private float mInterpolationPos = 0;
        private List<float> mInterpolationPosList = new List<float>();
        private Ticker mTicker = null;

        private ScrollRect mRect;

        public float Smooting = 4;
        public float Sensitivity = 2;
        public RectTransform Content;
        public Transform ToggleList;

        public Action<int> OnPageChanged;
        public Action<int> OnDragEnd;
        

        void Start()
        {
            mRect = transform.GetComponent<ScrollRect>();

            RectTransform tr = GetComponent<RectTransform>();

            int count = 0;
            for (int i = 0; i < Content.transform.childCount; i++)
            {
                RectTransform tran = Content.transform.GetChild(i) as RectTransform;
                if (tran != null && tran.gameObject.activeSelf == true)
                {
                    count += 1;
                }
            }

            float space = 0;
            GridLayoutGroup gridLayout = Content.GetComponent<GridLayoutGroup>();
            if (gridLayout)
            {
                space = gridLayout.spacing.x;
            }

            float tempWidth = ((float)count * (tr.rect.width + space));
            Content.sizeDelta = new Vector2(tempWidth, tr.rect.height);

            float leftWidth = Content.rect.width - tr.rect.width - space;
            for (int i = 0; i < count; i++)
            {
                mInterpolationPosList.Add((tr.rect.width + space) * i / leftWidth);
            }
        }

        void Update()
        {
            if (!mIsDragging && !mStopMove)
            {
                mMoveTime += Time.deltaTime;

                float t = mMoveTime * Smooting;
                mRect.horizontalNormalizedPosition = Mathf.Lerp(mRect.horizontalNormalizedPosition, mInterpolationPos, t);

                mStopMove = (t >= 1 ? true : false);
            }
        }

        public void PageTo(int index)
        {
            if (index >= 0 && index < mInterpolationPosList.Count)
            {
                mRect.horizontalNormalizedPosition = mInterpolationPosList[index];
                SetPageIndex(index);
                GetIndex(index);
            }
        }

        public void MoveTo(int index, float timer)
        {
            if (mIsDragging)
                return;

            if (index >= 0 && index < mInterpolationPosList.Count)
            {
                float startPos = mRect.horizontalNormalizedPosition;
                float targetPos = mInterpolationPosList[index];

                ResetMoveToTimer();

                mTicker = TimerMgr.Instance.AddTicker(Math.Max(timer, 0.1f), (t, a) =>
                {
                    mRect.horizontalNormalizedPosition = t * (targetPos - startPos) / a + startPos;
                }, () =>
                {
                    mRect.horizontalNormalizedPosition = mInterpolationPosList[index];
                    SetPageIndex(index);
                    GetIndex(index);
                    mTicker = null;
                });
            }
        }

        private void ResetMoveToTimer()
        {
            if (mTicker != null)
            {
                mTicker.Stop();
                mTicker = null;
            }
        }

        private void SetPageIndex(int index)
        {
            if (mCurPageIndex != index)
            {
                mCurPageIndex = index;
                if (OnPageChanged != null)
                    OnPageChanged(index);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mIsDragging = true;

            ResetMoveToTimer();

            mStartDragPos = mRect.horizontalNormalizedPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (mInterpolationPosList.Count == 0)
                return;

            float posX = mRect.horizontalNormalizedPosition;
            posX += ((posX - mStartDragPos) * Sensitivity);
            posX = posX < 1 ? posX : 1;
            posX = posX > 0 ? posX : 0;
            int index = 0;

            float offset = Mathf.Abs(mInterpolationPosList[index] - posX);

            for (int i = 1; i < mInterpolationPosList.Count; i++)
            {
                float temp = Mathf.Abs(mInterpolationPosList[i] - posX);
                if (temp < offset)
                {
                    index = i;
                    offset = temp;
                }
            }

            SetPageIndex(index);
            GetIndex(index);

            mIsDragging = false;
            mStopMove = false;
            mMoveTime = 0;
            mInterpolationPos = mInterpolationPosList[index]; 

            if (OnDragEnd != null)
                OnDragEnd(index);
        }

        public void GetIndex(int index)
        {
            if (ToggleList != null)
            {
                var toogle = ToggleList.GetChild(index).GetComponent<Toggle>();
                toogle.isOn = true;
            }
        }

    }

}