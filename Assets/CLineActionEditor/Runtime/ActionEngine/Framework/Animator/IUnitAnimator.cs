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

using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace SuperCLine.ActionEngine
{
    public interface IUnitAnimator
    {
        List<string> ParameterList { get; }
        Dictionary<string, UnitAnimatorData> StateHash { get; }
        List<string> StateNameList { get; }

        void Init(GameObject go);
        void GetAllParameter();
        void GetAllState();
        void Update(float fTick);
        void Play(string animationClipName);
    }
}