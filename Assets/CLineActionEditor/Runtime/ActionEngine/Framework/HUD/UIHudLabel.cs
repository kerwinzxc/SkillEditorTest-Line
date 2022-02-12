/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\UIHudLabel.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-3-4      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;

    public sealed class UIHudLabel : MonoBehaviour
    {
        public Text Label;

        private bool mEnable = false;
        private float mBeginTime = 0f;
        private float mDuration = 0f;
        private float mCurveMaxTime = 0f;
        private HudCurve mCurve = null;
        private Action<UIHudLabel> mHandler = null;

        private Vector3 mPos = Vector3.zero;
        private Vector3 mScale = Vector3.zero;
        private Color mColor = Color.white;

        public void Show(EHudType hudType, double value, float duration, HudCurve curve, Action<UIHudLabel> cycleHandler)
        {
            mEnable = true;
            mBeginTime = Time.realtimeSinceStartup;
            mDuration = duration;
            mCurveMaxTime = curve.MaxTime;
            mCurve = curve;
            mHandler = cycleHandler;

            // set text
            if (value > 0)
            {
                Label.text = "+" + value.ToString();
            }
            else if (value < 0)
            {
                Label.text = value.ToString();
            }
            else
            {
                Label.text = "0";//miss
            }
        }

        public void OnDestroy()
        {
            mEnable = false;
            mBeginTime = 0f;
            mDuration = 0f;
            mCurve = null;
            mHandler = null;
        }

        private void Update()
        {
            if (!mEnable)
                return;

            float time = Time.realtimeSinceStartup;
            float dt1 = time - mBeginTime;
            float scale = mCurve.CurveScale.Evaluate(dt1);
            scale = Helper.Clamp(scale, 0.001f, 10f);
            mScale.x = scale;
            mScale.y = scale;
            mScale.z = scale;
            Label.transform.localScale = mScale;

            float dt2 = time - mBeginTime - mDuration;
            mColor.a = mCurve.CurveAlpha.Evaluate(dt2);
            mColor.r = Label.color.r;
            mColor.g = Label.color.g;
            mColor.b = Label.color.b;
            Label.color = mColor;

            mPos.y = mCurve.CurveY.Evaluate(dt2);
            Label.transform.localPosition = mPos;

            if (dt2 > mCurveMaxTime)
            {
                mHandler(this);
            }
        }
    }
}