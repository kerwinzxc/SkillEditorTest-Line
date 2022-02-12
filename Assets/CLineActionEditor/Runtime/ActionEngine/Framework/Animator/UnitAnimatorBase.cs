using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperCLine.ActionEngine
{
    public abstract class UnitAnimatorBase : IUnitAnimator
    {
        public List<string> ParameterList { get; }
        public Dictionary<string, UnitAnimatorData> StateHash { get; }
        public List<string> StateNameList { get; }

        public abstract void Init(GameObject go);

        public void GetAllParameter()
        {
            throw new System.NotImplementedException();
        }

        public void GetAllState()
        {
            throw new System.NotImplementedException();
        }

        public void Update(float fTick)
        {
            throw new System.NotImplementedException();
        }

        public void Play(string animationClipName)
        {
            throw new System.NotImplementedException();
        }
    }   
}
