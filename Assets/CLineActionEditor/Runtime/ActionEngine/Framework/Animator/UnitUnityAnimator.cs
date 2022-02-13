namespace SuperCLine.ActionEngine
{
    using System.Collections.Generic;
    using UnityEditor.Animations;
    using UnityEngine;

    public class UnitUnityAnimator : IUnitAnimator
    {
        public List<string> ParameterList { get; private set; }
        public Dictionary<string, UnitAnimatorData> StateHash { get; private set; }
        public List<string> StateNameList { get; private set; }
        public float Speed { get; set; }

        private Animator animator;

        public UnitUnityAnimator()
        {
            ParameterList = new List<string>();
            StateHash = new Dictionary<string, UnitAnimatorData>();
            StateNameList = new List<string>();
        }

        public int LayerCount => animator.layerCount;

        public void Init(GameObject go)
        {
            animator = go.GetComponentInChildren<Animator>();
        }

        public void GetAllParameter()
        {
            ParameterList.Clear();

            AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;

            for (int i = 0; i < ac.parameters.Length; ++i)
            {
                ParameterList.Add(ac.parameters[i].name);
            }
        }

        public void GetAllState()
        {
            StateNameList.Clear();
            StateHash.Clear();

            AnimatorController ac = animator.runtimeAnimatorController as AnimatorController;
            // TO CLine: more layer later
            ChildAnimatorState[] stList = ac.layers[0].stateMachine.states;

            for (int i = 0; i < stList.Length; ++i)
            {
                StateNameList.Add(stList[i].state.name);
                StateHash.Add(stList[i].state.name, new UnitAnimatorData()
                {
                    Length = stList[i].state.motion.averageDuration
                });
            }
        }

        public void Update(float fTick)
        {
            animator.Update(fTick);
        }

        public void Play(string animationClipName)
        {
            animator.Play(animationClipName, 0, 0);
        }

        public void Play(string animationClipName, int layer, float normalizedTime)
        {
            animator.Play(animationClipName, layer, normalizedTime);
        }

        public void CrossFade(string animationClipName, float transitionDuration, int layer, float normalizedTime)
        {
            animator.CrossFade(animationClipName, transitionDuration, layer, normalizedTime);
        }

        public void SetLayerWeight(int layer, float weight)
        {
            throw new System.NotImplementedException();
        }

        public void SetBool(string paramName, bool value)
        {
            animator.SetBool(paramName, value);
        }

        public void SetInteger(string paramName, int value)
        {
            animator.SetInteger(paramName, value);
        }

        public void SetFloat(string paramName, float value)
        {
            animator.SetFloat(paramName, value);
        }

        public void SetTrigger(string paramName)
        {
            animator.SetTrigger(paramName);
        }
    }

}