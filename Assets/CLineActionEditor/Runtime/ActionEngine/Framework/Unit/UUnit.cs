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
        private Unit mOwner;
        public Unit Owner { get { return mOwner; } }
        private IUnitAnimator mAnim;
        private float mAnimSpeed;

        public IUnitAnimator Anim
        {
            get { return mAnim; }
        }

        public void SetUnit(Unit unit)
        {
            mOwner = unit;
            var animatorTypeName = mOwner.AnimatorTypeName;
            if (string.IsNullOrEmpty(animatorTypeName))
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "Unit", "animator type name is null.");
                animatorTypeName = "SuperCLine.ActionEngine.UnitUnityAnimator";
                return;
            }
            System.Type type = Utility.Assembly.GetType(animatorTypeName);
            var ins = System.Activator.CreateInstance(type);
            mAnim = ins as IUnitAnimator;
            mAnim.Init(gameObject);
            mAnimSpeed = mAnim.Speed;
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
                mAnim.Speed = mAnimSpeed;
            }
            else
            {
                mAnim.Speed = speed;
            }
        }

        public void SetAnimatorLayerWeight(int layer = 1, int weight = 0)
        {
            if (mAnim.LayerCount > layer)
                mAnim.SetLayerWeight(layer, weight);
        }

        private void OnDestroy()
        {
            if (Owner != null)
                Owner.OnDetached(this);
        }
    }
}
