using System.Collections.Generic;
using Spine.Unity;
using SuperCLine.ActionEngine;
using UnityEngine;

public class UnitSpineAnimator : IUnitAnimator
{
    public List<string> ParameterList { get; private set; }
    public Dictionary<string, UnitAnimatorData> StateHash { get; private set; }
    public List<string> StateNameList { get; private set; }

    private SkeletonAnimation mSkeletonAnim = null;

    public UnitSpineAnimator()
    {
        ParameterList = new List<string>();
        StateHash = new Dictionary<string, UnitAnimatorData>();
        StateNameList = new List<string>();   
    }

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
                Length = animation.Duration
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
}