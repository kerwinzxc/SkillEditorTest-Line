/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Scripts\EffectWrapper.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-22      CLine           Created
|
+-----------------------------------------------------------------------------*/


namespace CAE.Core
{
    using UnityEngine;
    using System.Collections.Generic;

    public class EffectWrapper : XObject
    {
        private GameObject mEffect;
        private ParticleSystem[] mParticleList;
        private float mSimulateTime = 0f;

        public EffectWrapper(EventPlayEffect epe)
        {
            string effectname = epe.EffectName;
            if (epe.UseRandom)
            {
                int idx = UnityEngine.Random.Range(0, epe.RandomEffectList.Count);
                effectname = epe.RandomEffectList[idx];
            }

            GameObject go = ResourceMgr.Instance.LoadObject<GameObject>(epe.EffectName);

            switch (epe.DummyType)
            {
                case EEffectDummyType.EEDT_DummyFollow:
                    {
                        Transform dummyRoot = string.IsNullOrEmpty(epe.DummyRoot) ? UnitWrapper.Instance.UnitWrapperUnit.transform : Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, epe.DummyRoot);

                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;
                        mEffect.transform.parent = Helper.Find(dummyRoot, epe.DummyAttach);
                        mEffect.transform.localPosition = epe.Position;
                        mEffect.transform.localRotation = Quaternion.Euler(epe.Euler);
                    }
                    break;
                case EEffectDummyType.EEDT_DummyPosition:
                    {
                        Transform dummyRoot = string.IsNullOrEmpty(epe.DummyRoot) ? UnitWrapper.Instance.UnitWrapperUnit.transform : Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, epe.DummyRoot);

                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;
                        Transform tr = Helper.Find(dummyRoot, epe.DummyAttach);
                        mEffect.transform.position = tr.position + epe.Position;
                        mEffect.transform.rotation = Quaternion.Euler(tr.rotation.eulerAngles + epe.Euler);
                    }
                    break;
                case EEffectDummyType.EEDT_Scene:
                    {
                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;
                        mEffect.transform.position = UnitWrapper.Instance.UnitWrapperUnit.transform.position + epe.Position;
                        mEffect.transform.rotation = Quaternion.Euler(epe.Euler);
                    }
                    break;
            }

            
            if (mEffect)
            {
                mParticleList = mEffect.GetComponentsInChildren<ParticleSystem>();
            }
        }

        public void Tick(float fTick)
        {
            mSimulateTime += fTick;

            for (int i = 0; i < mParticleList.Length; ++i)
            {
                mParticleList[i].Simulate(mSimulateTime);
            }
        }

        public float GetEffectTime()
        {
            float time = 0;
            for (int i = 0; i < mParticleList.Length; ++i)
            {
                if (time < (mParticleList[i].main.duration + mParticleList[i].main.startDelayMultiplier))
                {
                    time = (mParticleList[i].main.duration + mParticleList[i].main.startDelayMultiplier);
                }
            }

            return time;
        }

        protected override void OnDispose()
        {
            if (mEffect != null)
            {
                GameObject.DestroyImmediate(mEffect);
                mEffect = null;
            }
        }

    }
}
