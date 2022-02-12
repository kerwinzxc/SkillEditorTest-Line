/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\EffectMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : name to loop effect when playing
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-13      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class EffectMgr : Singleton<EffectMgr>
    {
        private LinkedList<UEffect> mEffectList = new LinkedList<UEffect>();
        private Dictionary<string, UEffect> mLoopEffectHash = new Dictionary<string, UEffect>();

        public override void Init()
        {

        }

        public override void Destroy()
        {
            using (var itr = mEffectList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current != null)
                    {
                        ObjectPoolMgr.Instance.GetPool(itr.Current.gameObject.name).Cycle(itr.Current.gameObject);
                    }
                }
            }
            mEffectList.Clear();

            using (var itr = mLoopEffectHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Value != null)
                    {
                        ObjectPoolMgr.Instance.GetPool(itr.Current.Value.gameObject.name).Cycle(itr.Current.Value.gameObject);
                    }
                }
            }
            mLoopEffectHash.Clear();
        }

        public void PlayEffect(string name, GameObject dummy, Vector3 relativePos, Vector3 relativeEuler, bool loop = false, string loopName = null)
        {
            if (ExistLoopEffect(loop, loopName))
                return;

            GameObject go = ObjectPoolMgr.Instance.GetPool(name).Get(true);
            go.name = name;
            Helper.AddChild(go, dummy);

            UEffect effect = go.GetComponent<UEffect>();
            if (effect == null)
                effect = go.AddComponent<UEffect>();

            if (relativePos != Vector3.zero)
            {
                effect.transform.localPosition = relativePos;
            }
            if (relativeEuler != Vector3.zero)
            {
                effect.transform.localRotation = Quaternion.Euler(relativeEuler);
            }

            PlayEffect(effect, loop, loopName);
        }

        public void PlayEffect(string name, Vector3 pos, Quaternion rotation, bool loop = false, string loopName = null)
        {
            if (ExistLoopEffect(loop, loopName))
                return;

            GameObject go = ObjectPoolMgr.Instance.GetPool(name).Get(pos, rotation, true);
            go.name = name;

            UEffect effect = go.GetComponent<UEffect>();
            if (effect == null)
                effect = go.AddComponent<UEffect>();

            PlayEffect(effect, loop, loopName);
        }

        public void StopEffect(string loopName)
        {
            if (string.IsNullOrEmpty(loopName))
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "Effect", "param error!");
                return;
            }

            UEffect effect = null;
            if (mLoopEffectHash.TryGetValue(loopName, out effect))
            {
                if (effect != null)
                {
                    ObjectPoolMgr.Instance.GetPool(effect.gameObject.name).Cycle(effect.gameObject);
                }
                mLoopEffectHash.Remove(loopName);
            }
        }

        private void PlayEffect(UEffect effect, bool loop, string loopName)
        {
            effect.Init();

            if (loop)
            {
                mLoopEffectHash.Add(loopName, effect);
            }
            else
            {
                effect.OnLifeTimeEnd = OnFinished;
                effect.OnDestroyed = OnDestroy;
                effect.Play();

                mEffectList.AddLast(effect);
            }
        }

        private bool ExistLoopEffect(bool loop, string loopName)
        {
            if (!loop)
                return false;

            if (string.IsNullOrEmpty(loopName))
            {
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "Effect", "need unique loop name!");
                return true;
            }

            UEffect effect = null;

            return mLoopEffectHash.TryGetValue(loopName, out effect);
        }

        private void OnFinished(UEffect effect)
        {
            if (mEffectList.Contains(effect))
            {
                mEffectList.Remove(effect);
                ObjectPoolMgr.Instance.GetPool(effect.gameObject.name).Cycle(effect.gameObject);
            }
        }

        private void OnDestroy(UEffect effect)
        {
            mEffectList.Remove(effect);
        }
    }

}
