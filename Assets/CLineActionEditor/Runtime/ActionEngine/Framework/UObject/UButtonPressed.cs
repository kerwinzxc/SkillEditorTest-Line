/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\UButtonPressed.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-7-11      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public System.Action<Button> OnLongPressed = null;
        public System.Action<Button> OnLongPressedEnd = null;

        private float delay = 0.2f;
        private bool isDown = false;
        private float lastIsDownTime;
        private Button owner;

        void Awake()
        {
            owner = transform.GetComponent<Button>();
        }

        void Update()
        {
            if (isDown)
            {
                if (Time.time - lastIsDownTime > delay)
                {
                    if (OnLongPressed != null)
                        OnLongPressed(owner);

                    lastIsDownTime = Time.time;
                }
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            lastIsDownTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            if (OnLongPressedEnd != null)
                OnLongPressedEnd(owner);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isDown = false;
        }

    }

}