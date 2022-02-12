/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\HudMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-3-5      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class HudMgr : Singleton<HudMgr>
    {
        private GameObject mHudRoot = null;

        private LinkedList<UIHudText> mTextList = new LinkedList<UIHudText>();
        private Dictionary<UIProgressBar, UIHudBlood> mBloodHash = new Dictionary<UIProgressBar, UIHudBlood>();

        public override void Init()
        {
            mHudRoot = GameObject.Find("HudRoot");
        }

        public override void Destroy()
        {
            mHudRoot = null;

            using (var itr = mTextList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current != null)
                    {
                        itr.Current.OnDestroy();
                        ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudText.prefab").Cycle(itr.Current.gameObject);
                    }
                }
            }
            mTextList.Clear();

            using (var itr = mBloodHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Value != null)
                    {
                        itr.Current.Value.OnDestroy();
                        ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudBlood.prefab").Cycle(itr.Current.Value.gameObject);
                    }
                }
            }
            mBloodHash.Clear();
        }

        public UIHudText GetHudText(Transform owner)
        {
            GameObject go = ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudText.prefab").Get();
            go.name = "hudtext";

            Helper.AddChild(go, mHudRoot);

            UIHudText text = go.GetComponent<UIHudText>();
            text.Init(owner);
            mTextList.AddLast(text);

            return text;
        }

        public UIProgressBar GetHudBlood(Transform owner, EBloodType eType)
        {
            GameObject go = ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudBlood.prefab").Get();
            go.name = "hudblood";

            Helper.AddChild(go, mHudRoot);

            UIHudBlood blood = go.GetComponent<UIHudBlood>();
            UIProgressBar progress = blood.Init(owner, eType);
            mBloodHash.Add(progress, blood);

            return progress;
        }

        public void CycleHudText(UIHudText text)
        {
            text.OnDestroy();
            mTextList.Remove(text);

            ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudText.prefab").Cycle(text.gameObject);
        }

        public void CycleHudBlood(UIProgressBar progress)
        {
            UIHudBlood blood = null;
            if (mBloodHash.TryGetValue(progress, out blood))
            {
                blood.OnDestroy();
                mBloodHash.Remove(progress);

                ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudBlood.prefab").Cycle(blood.gameObject);
            }
        }
    }
}