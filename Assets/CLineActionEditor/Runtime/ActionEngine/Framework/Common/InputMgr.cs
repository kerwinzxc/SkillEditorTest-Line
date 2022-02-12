/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\InputMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-20      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class InputMgr : Singleton<InputMgr>
    {
        private UInputMgr mInput = null;

        // TO CLine: simulate keyboard input just pc-standlone
        private Vector3 mSimulateInput = Vector3.zero;
        private Vector3 mSimulateLastInput = Vector3.zero;
        private Vector2 mSimulateMove = Vector2.zero;
        private Vector2 mSimulateGesture = Vector2.zero;
        private SimulateInput mSimulateKeyboard = new SimulateInput();

        public override void Init()
        {
            GameObject prefab = ResourceMgr.Instance.LoadObject<GameObject>("/Prefabs/Common/InputMgr.prefab");
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            Helper.AddChild(go, UIMgr.Instance.UI2DRoot);
            //GameObject.DontDestroyOnLoad(go);
            mInput = go.GetComponent<UInputMgr>();
            mInput.InputJoyStick.OnJoystickMoveStart = OnJoystickMoveStart;
            mInput.InputJoyStick.OnJoystickMove = OnJoystickMove;
            mInput.InputJoyStick.OnJoystickMoveEnd = OnJoystickMoveEnd;
            mInput.OnClicked = OnClicked;
            mInput.OnLongPressed = OnLongPressed;
            mInput.OnLongPressedEnd = OnLongPressedEnd;
            mInput.Init();
        }

        public override void Destroy()
        {
            
        }

        public bool EnableInput
        {
            set { mInput.gameObject.SetActive(value); }
        }

        public bool EnableJoyStick
        {
            set { mInput.EnableJoyStick = value; }
        }

        public Unit Controller
        {
            get;
            set;
        }

        private void OnJoystickMoveStart(Vector2 dir)
        {

        }

        private void OnJoystickMove(Vector2 dir)
        {
            Controller?.LocalMove(dir);
        }

        private void OnJoystickMoveEnd(Vector2 dir)
        {
            Controller?.LocalMoveStop();
        }

        private void OnClicked(Button bt)
        {
            Controller?.LocalAttack(bt.name);
        }

        private void OnLongPressed(Button bt)
        {
            Controller.LocalLongPressed(bt.name);
        }

        private void OnLongPressedEnd(Button bt)
        {
            Controller.LocalLongPressedEnd(bt.name);
        }

        private void OnSwipeStart(Vector2 dir)
        {

        }

        private void OnSwipe(Vector2 dir)
        {

        }

        private void OnSwipeEnd(Vector2 dir)
        {

        }

        public void Update(float fTick)
        {
#if (((UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY) && !UNITY_EDITOR))
            UpdateTouch(fTick);
#else
            UpdateKeyboard(fTick);
#endif
        }

        private void UpdateTouch(float fTick)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Ended:
                        break;
                }
            }
        }

        public void PostUpdate(float fTick)
        {
            Controller?.PostUpdate(fTick);
        }

        private void UpdateKeyboard(float fTick)
        {
            mSimulateKeyboard.UpdateKeyStatus(fTick);

            bool doubleClick = false;
            if (mSimulateKeyboard.CheckKeyStatus(KeyCode.W, EInputType.EIT_DoubleClick, fTick))
            {
                doubleClick = true;
                mSimulateInput += new Vector3(0, 0, 1f);
            }
            if (mSimulateKeyboard.CheckKeyStatus(KeyCode.S, EInputType.EIT_DoubleClick, fTick))
            {
                doubleClick = true;
                mSimulateInput += new Vector3(0, 0, -1f);
            }
            if (mSimulateKeyboard.CheckKeyStatus(KeyCode.A, EInputType.EIT_DoubleClick, fTick))
            {
                doubleClick = true;
                mSimulateInput += new Vector3(-1f, 0, 0);
            }
            if (mSimulateKeyboard.CheckKeyStatus(KeyCode.D, EInputType.EIT_DoubleClick, fTick))
            {
                doubleClick = true;
                mSimulateInput += new Vector3(1f, 0, 0);
            }

            if (doubleClick)
            {
                if (mSimulateInput != Vector3.zero)
                {
                    mSimulateGesture.x = mSimulateInput.x;
                    mSimulateGesture.y = mSimulateInput.z;
                    OnSwipeEnd(mSimulateGesture);
                }
            }
            else
            {
                mSimulateLastInput = mSimulateInput;
                mSimulateInput = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    mSimulateInput += new Vector3(0, 0, 1f);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    mSimulateInput += new Vector3(0, 0, -1f);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    mSimulateInput += new Vector3(-1f, 0, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    mSimulateInput += new Vector3(1f, 0, 0);
                }

                if (mSimulateInput != Vector3.zero)
                {
                    mSimulateMove.x = mSimulateInput.x;
                    mSimulateMove.y = mSimulateInput.z;
                    OnJoystickMove(mSimulateMove);
                }
                else
                {
                    if (mSimulateLastInput != Vector3.zero)
                        OnJoystickMoveEnd(mSimulateMove);
                }
            }

            if (Input.GetKey(KeyCode.J))
            {

            }
            else if (Input.GetKey(KeyCode.K))
            {

            }
            else if (Input.GetKeyUp(KeyCode.L))
            {

            }
            else if (Input.GetKeyUp(KeyCode.I))
            {

            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {

            }
        }


    }

}
