/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AudioMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-17      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

using System;

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections;

    public sealed class AudioMgr : Singleton<AudioMgr>
    {
        private bool mEnableSound = false;
        private bool mEnableMusic = false;
        private float mVolumeSound = 0f;
        private float mVolumeMusic = 0f;

        private AudioSource mMusic = null;
        private Dictionary<string, LinkedList<AudioSource>> mSoundHash = new Dictionary<string, LinkedList<AudioSource>>();
        private Dictionary<string, LinkedList<AudioSource>> mHandleHash = new Dictionary<string, LinkedList<AudioSource>>();
        private Dictionary<AudioSource, string> mFadeoutHash = new Dictionary<AudioSource, string>();

        public bool EnableSound
        {
            get { return mEnableSound; }
            set
            {
                mEnableSound = value;
            }
        }

        public bool EnableMusic
        {
            get { return mEnableMusic; }
            set
            {
                mEnableMusic = value;
                if (null != mMusic)
                    mMusic.mute = !mEnableMusic;
            }
        }

        public float VolumeSound
        {
            get { return mVolumeSound; }
            set
            {
                mVolumeSound = value;
            }
        }

        public float VolumeMusic
        {
            get { return mVolumeMusic; }
            set
            {
                mVolumeMusic = value;
                if (null != mMusic)
                    mMusic.volume = mVolumeMusic;
            }
        }

        public override void Init()
        {
            mEnableSound = GameConfig.Instance.GetBool("EnableSound", true);
            mEnableMusic = GameConfig.Instance.GetBool("EnableMusic", true);
            mVolumeSound = GameConfig.Instance.GetFloat("VolumeSound", 1.0f);
            mVolumeMusic = GameConfig.Instance.GetFloat("VolumeMusic", 1.0f);
        }

        public override void Destroy()
        {
            using (var itrHash = mSoundHash.GetEnumerator())
            {
                while (itrHash.MoveNext())
                {
                    using (var itrSound = itrHash.Current.Value.GetEnumerator())
                    {
                        while (itrSound.MoveNext())
                        {
                            if (null != itrSound.Current)
                            {
                                itrSound.Current.Stop();
                                ObjectPoolMgr.Instance.GetPool(itrSound.Current.gameObject.name).Cycle(itrSound.Current.gameObject);
                            }
                        }
                    }
                }
            }

            mSoundHash.Clear();

            StopMusicAll();
        }

        public AudioSource PlayMusic(string name, bool fadein = false, float time = 0f)
        {
            GameObject go = ObjectPoolMgr.Instance.GetPool(name).Get();
            if (null == go)
                return null;

            CycleAudio(ref mMusic);

            go.name = name;

            mMusic = go.GetComponent<AudioSource>();
            mMusic.mute = !mEnableMusic;
            mMusic.volume = fadein ? 0f : Mathf.Clamp01(mVolumeMusic);
            mMusic.Play();

            if (fadein)
                GameManager.Instance.StartCoroutine(FadeInCoroutine(time));

            return mMusic;
        }

        public AudioSource PlaySound(string name, int instanceCount = 3)
        {
            if (!CheckInstanceCount(name, instanceCount))
                return null;

            GameObject go = ObjectPoolMgr.Instance.GetPool(name).Get();
            if (null == go)
                return null;

            LinkedList<AudioSource> list;
            if (!mSoundHash.TryGetValue(name, out list))
            {
                list = new LinkedList<AudioSource>();
                mSoundHash.Add(name, list);
            }
            else
            {
                list = mSoundHash[name];
            }

            go.name = name;

            AudioSource ac = go.GetComponent<AudioSource>();
            ac.mute = !mEnableSound;
            ac.volume = Mathf.Clamp01(mVolumeSound);
            ac.Play();

            list.AddLast(ac);

            return ac;
        }

        public AudioSource PlaySound(string name, GameObject attach, int instanceCount)
        {
            return null;
        }

        public AudioSource PlaySound(string name, Vector3 pos, int instanceCount)
        {
            return null;
        }

        public void StopMusic(bool fadeout = false, float time = 0f)
        {
            if (mMusic == null)
                return;

            if (fadeout)
            {
                mFadeoutHash.Add(mMusic, mMusic.gameObject.name);
                GameManager.Instance.StartCoroutine(FadeOutCoroutine(mMusic, time));
                mMusic = null;
            }
            else
            {
                CycleAudio(ref mMusic);
            }
        }

        public void StopMusicAll()
        {
            CycleAudio(ref mMusic);

            using (Dictionary<AudioSource, string>.Enumerator itr = mFadeoutHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Key != null)
                    {
                        itr.Current.Key.Stop();
                        ObjectPoolMgr.Instance.GetPool(itr.Current.Key.gameObject.name).Cycle(itr.Current.Key.gameObject);
                    }
                }
            }
        }

        public void StopSoundAll()
        {
            using (var itrHash = mSoundHash.GetEnumerator())
            {
                while (itrHash.MoveNext())
                {
                    using (var itrList = itrHash.Current.Value.GetEnumerator())
                    {
                        while (itrList.MoveNext())
                        {
                            itrList.Current.Stop();
                        }
                    }
                }
            }
        }

        public void Update(float fTick)
        {
            using (var itrHash = mSoundHash.GetEnumerator())
            {
                while (itrHash.MoveNext())
                {
                    using (var itrList = itrHash.Current.Value.GetEnumerator())
                    {
                        while (itrList.MoveNext())
                        {
                            if (itrList.Current == null || !itrList.Current.isPlaying)
                            {
                                LinkedList<AudioSource> list = null;
                                if (!mHandleHash.TryGetValue(itrHash.Current.Key, out list))
                                {
                                    list = new LinkedList<AudioSource>();
                                    mHandleHash.Add(itrHash.Current.Key, list);
                                }
                                list.AddLast(itrList.Current);
                            }
                            else
                            {
                                itrList.Current.volume = mVolumeSound;
                                itrList.Current.mute = !mEnableSound;
                            }
                        }
                    }
                }
            }

            using (var itrHash = mHandleHash.GetEnumerator())
            {
                while (itrHash.MoveNext())
                {
                    LinkedList<AudioSource> updateList = mSoundHash[itrHash.Current.Key];
                    using (var itrList = itrHash.Current.Value.GetEnumerator())
                    {
                        while (itrList.MoveNext())
                        {
                            if (null != itrList.Current)
                                ObjectPoolMgr.Instance.GetPool(itrHash.Current.Key).Cycle(itrList.Current.gameObject);

                            updateList.Remove(itrList.Current);
                        }
                    }

                    if (updateList.Count == 0)
                        mSoundHash.Remove(itrHash.Current.Key);
                }
            }
            mHandleHash.Clear();
        }

        private bool CheckInstanceCount(string name, int instanceCount)
        {
            LinkedList<AudioSource> list;
            if (!mSoundHash.TryGetValue(name, out list))
                return true;

            return list.Count < instanceCount ? true : false;
        }

        private IEnumerator FadeInCoroutine(float time)
        {
            float beginTime = Time.realtimeSinceStartup;
            float curTime = beginTime;
            float endTime = curTime + time;

            while (curTime < endTime && mMusic != null)
            {
                float delta = (curTime - beginTime) / time * mVolumeMusic;
                mMusic.volume = delta;

                yield return null;

                curTime = Time.realtimeSinceStartup;
            }

            if (mMusic != null)
            {
                mMusic.volume = mVolumeMusic;
            }
        }

        private IEnumerator FadeOutCoroutine(AudioSource ac, float time)
        {
            float beginTime = Time.realtimeSinceStartup;
            float curTime = beginTime;
            float endTime = curTime + time;

            while (curTime < endTime && ac != null)
            {
                float delta = (1 - (curTime - beginTime) / time) * mVolumeMusic;
                ac.volume = delta;

                yield return null;

                curTime = Time.realtimeSinceStartup;
            }

            if (ac != null)
            {
                mFadeoutHash.Remove(ac);
                CycleAudio(ref ac);
            }
        }

        private void CycleAudio(ref AudioSource ac)
        {
            if (ac != null)
            {
                ac.Stop();
                ObjectPoolMgr.Instance.GetPool(ac.gameObject.name).Cycle(ac.gameObject);
                ac = null;
            }
        }
    }
}
