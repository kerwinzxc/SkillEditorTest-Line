/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Unit\UUnit.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-4      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;

    public sealed class UUnit : MonoBehaviour
    {
        public Unit Owner;
        private Animator mAnim;
        private float mAnimSpeed;

        public Animator Anim
        {
            get { return mAnim; }
        }

        void Awake()
        {
            mAnim = transform.GetComponent<Animator>();
            mAnimSpeed = mAnim.speed;
        }

        public void PlayAnimation(EAnimType type, string name, string value)
        {
            switch (type)
            {
                case EAnimType.EAT_SetBool:
                    {
                        bool val = Convert.ToBoolean(value);
                        mAnim.SetBool(name, val);
                    }
                    break;
                case EAnimType.EAT_SetInt:
                    {
                        int val = Convert.ToInt32(value);
                        mAnim.SetInteger(name, val);
                    }
                    break;
                case EAnimType.EAT_SetFloat:
                    {
                        float val = Convert.ToSingle(value);
                        mAnim.SetFloat(name, val);
                    }
                    break;
                case EAnimType.EAT_SetTrigger:
                    {
                        mAnim.SetTrigger(name);
                    }
                    break;
                case EAnimType.EAT_Force:
                    {
                        mAnim.Play(name, 0, 0);
                    }
                    break;
            }
        }

        public void PlayAnimation(string name, float transitionDuration, int layer, float normalizedTime)
        {
            mAnim.CrossFade(name, transitionDuration, layer, normalizedTime);
        }

        public void PlayAnimation(string name)
        {
            mAnim.Play(name, 0, 0);
        }

        public void SetAnimationSpeed(float speed)
        {
            if (null == mAnim)
                return;

            if (speed < 0)
            {
                mAnim.speed = mAnimSpeed;
            }
            else
            {
                mAnim.speed = speed;
            }
        }

        public void SetAnimatorLayerWeight(int layer = 1, int weight = 0)
        {
            if (mAnim.layerCount > layer)
                mAnim.SetLayerWeight(layer, weight);
        }

        private void OnDestroy()
        {
            if (Owner != null)
                Owner.OnDetached(this);
        }
    }
}
