/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\UJoyStick.cs
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
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class UJoyStick : ScrollRect
    {
        public System.Action<Vector2> OnJoystickMoveStart = null;
        public System.Action<Vector2> OnJoystickMove = null;
        public System.Action<Vector2> OnJoystickMoveEnd = null;

        protected float mRadius = 0f;
        protected bool mIsDragging = false;
        protected Vector2 mContentPostion = Vector2.zero;

        protected override void Start()
        {
            base.Start();

            mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            if (OnJoystickMoveStart != null)
                OnJoystickMoveStart(eventData.delta);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            mIsDragging = true;
            mContentPostion = this.content.anchoredPosition;

            if (mContentPostion.magnitude > mRadius)
            {
                mContentPostion = mContentPostion.normalized * mRadius;
                SetContentAnchoredPosition(mContentPostion);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            mIsDragging = false;

            if (OnJoystickMoveEnd != null)
                OnJoystickMoveEnd(eventData.delta);
        }

        private void Update()
        {
            if (mIsDragging && OnJoystickMove != null)
                OnJoystickMove(mContentPostion.normalized);
        }
    }

}
