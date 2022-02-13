using System.Collections.Generic;
using Spine.Unity;
using SuperCLine.ActionEngine;
using UnityEngine;

public class UnitSpineAnimator : IUnitAnimator
{
    public List<string> ParameterList { get; private set; }
    public Dictionary<string, UnitAnimatorData> StateHash { get; private set; }
    public List<string> StateNameList { get; private set; }
    float IUnitAnimator.Speed { get; set; }

    public float Speed
    {
        get
        {
            if (mSkeletonAnim == null)
            {
                return 0;
            }
            return mSkeletonAnim.timeScale;
        }
    }

    private SkeletonAnimation mSkeletonAnim = null;

    public UnitSpineAnimator()
    {
        ParameterList = new List<string>();
        StateHash = new Dictionary<string, UnitAnimatorData>();
        StateNameList = new List<string>();   
    }

    public int LayerCount { get; }

    public void Init(GameObject go)
    {
        mSkeletonAnim = go.GetComponentInChildren<SkeletonAnimation>();
    }
    
    public void GetAllParameter()
    {
    }

    public void GetAllState()
    {
        StateNameList.Clear();
        StateHash.Clear();

        foreach (var animation in mSkeletonAnim.AnimationState.Data.SkeletonData.Animations)
        {
            StateNameList.Add(animation.Name);
            StateHash.Add(animation.Name, new UnitAnimatorData()
            {
                Length = animation.Duration,
            });
        }
    }

    public void Update(float fTick)
    {
        mSkeletonAnim.Update(fTick);
        mSkeletonAnim.LateUpdate();
    }

    public void Play(string animationClipName)
    {
        mSkeletonAnim.AnimationState.SetAnimation(1, animationClipName, false);
    }

    public void Play(string animationClipName, int layer, float normalizedTime)
    {
        mSkeletonAnim.AnimationState.SetAnimation(1, animationClipName, false);
    }

    public void CrossFade(string animationClipName, float transitionDuration, int layer, float normalizedTime)
    {
        mSkeletonAnim.AnimationState.SetAnimation(1, animationClipName, false);
    }

    public void SetLayerWeight(int layer, float weight)
    {
        throw new System.NotImplementedException();
    }

    public void SetBool(string paramName, bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SetInteger(string paramName, int value)
    {
        throw new System.NotImplementedException();
    }

    public void SetFloat(string paramName, float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetTrigger(string paramName)
    {
        throw new System.NotImplementedException();
    }
}