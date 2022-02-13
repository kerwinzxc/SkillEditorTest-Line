/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Preview\UnitWrapper.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-31      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/
namespace SuperCLine.ActionEngine
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public interface IUnitAnimator
    {
        List<string> ParameterList { get; }
        Dictionary<string, UnitAnimatorData> StateHash { get; }
        List<string> StateNameList { get; }
        float Speed { get; set; }
        int LayerCount { get; }

        void Init(GameObject go);
        void GetAllParameter();
        void GetAllState();
        void Update(float fTick);
        void Play(string animationClipName);
        void Play(string animationClipName, int layer, float normalizedTime);
        void CrossFade(string animationClipName, float transitionDuration, int layer, float normalizedTime);
        void SetLayerWeight(int layer, float weight);
        
        void SetBool(string paramName, bool value);
        void SetInteger(string paramName, int value);
        void SetFloat(string paramName, float value);
        void SetTrigger(string paramName);
    }
}