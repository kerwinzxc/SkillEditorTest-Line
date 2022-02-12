/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\SimulateInput.cs
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
    using UnityEngine;
    using System.Collections.Generic;


    public enum EInputType
    {
        EIT_Click,
        EIT_DoubleClick,
        EIT_Press,
        EIT_Release,
        EIT_Pressing,
        EIT_Releasing,
    }

    public sealed class SimulateInput
    {
        public sealed class KeyState
        {
            public float PressedTime = 10f;
            public float ReleasedTime = 10f;
            public int Pressed = 0;
        }

        private Dictionary<KeyCode, KeyState> mKeyHash = new Dictionary<KeyCode, KeyState>();

        public SimulateInput()
        {
            mKeyHash.Add(KeyCode.W, new KeyState());
            mKeyHash.Add(KeyCode.S, new KeyState());
            mKeyHash.Add(KeyCode.A, new KeyState());
            mKeyHash.Add(KeyCode.D, new KeyState());
            mKeyHash.Add(KeyCode.J, new KeyState());
        }

        public void UpdateKeyStatus(float fTick)
        {
            using (Dictionary<KeyCode, KeyState>.Enumerator itr = mKeyHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    KeyState ks = itr.Current.Value;
                    ks.PressedTime += fTick;
                    ks.ReleasedTime += fTick;

                    if (Input.GetKeyDown(itr.Current.Key) && ks.Pressed == 0)
                        OnKeyDown(ks);
                    if (Input.GetKeyUp(itr.Current.Key) && ks.Pressed != 0)
                        OnKeyUp(ks);
                }
            }
        }

        public bool CheckKeyStatus(KeyCode kc, EInputType type, float fTick)
        {
            int clickCount = mKeyHash[kc].Pressed;
            float pressedTime = mKeyHash[kc].PressedTime;
            float releasedTime = mKeyHash[kc].ReleasedTime;
            bool flag = false;

            switch (type)
            {
                case EInputType.EIT_Click:
                    flag = pressedTime == 0f;
                    break;
                case EInputType.EIT_DoubleClick:
                    flag = clickCount == 2 && pressedTime == 0f;
                    break;
                case EInputType.EIT_Press:
                    flag = clickCount != 0 && (pressedTime < 0.3f && pressedTime + fTick >= 0.3f);
                    break;
                case EInputType.EIT_Release:
                    flag = clickCount == 0 && releasedTime == 0f;
                    break;
                case EInputType.EIT_Pressing:
                    flag = clickCount != 0;
                    break;
                case EInputType.EIT_Releasing:
                    flag = clickCount == 0;
                    break;
            }

            return flag;
        }

        private void OnKeyDown(KeyState keyStatus)
        {
            keyStatus.Pressed = keyStatus.PressedTime >= 0.4f ? 1 : 2;
            keyStatus.PressedTime = 0f;
        }

        private void OnKeyUp(KeyState keyStatus)
        {
            keyStatus.Pressed = 0;
            keyStatus.ReleasedTime = 0f;
        }
    }

}
