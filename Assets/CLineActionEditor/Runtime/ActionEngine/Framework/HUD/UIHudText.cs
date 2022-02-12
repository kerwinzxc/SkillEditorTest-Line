/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\HUD\UIHudText.cs
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
    using System;
    using System.Collections.Generic;

    public enum EHudType
    {
        EHT_Default = 0,
        EHT_PlayerBehurt,

        EHT_MonsterBehurt_Normal,
        EHT_MonsterBehurt_Critical,
        EHT_MonsterBehurt_Normal_Skill,
        EHT_MonsterBehurt_Critical_SKill,

        EHT_DropGold,
    }

    [Serializable]
    public sealed class HudCurve
    {
        public AnimationCurve CurveY = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 150f) });
        public AnimationCurve CurveXLeft = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0f), new Keyframe(1f, -150f) });
        public AnimationCurve CurveXRight = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0f), new Keyframe(1f, 150f) });
        public AnimationCurve CurveAlpha = new AnimationCurve(new Keyframe[] { new Keyframe(0.5f, 1f), new Keyframe(1f, 0f) });
        public AnimationCurve CurveScale = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.25f, 0.6f), new Keyframe(1f, 0.3f) });

        public float MaxTime
        {
            get
            {
                float tY = CurveY[CurveY.keys.Length - 1].time;
                float tXL = CurveXLeft[CurveXLeft.keys.Length - 1].time;
                float tXR = CurveXRight[CurveXRight.keys.Length - 1].time;
                float tA = CurveScale[CurveScale.keys.Length - 1].time;
                float tS = CurveAlpha[CurveAlpha.keys.Length - 1].time;

                return Mathf.Max(tY, Mathf.Max(tXL, Mathf.Max(tXR, Mathf.Max(tA, tS))));
            }
        }
    }

    [Serializable]
    public sealed class HudCurveDictionary : Dictionary<EHudType, HudCurve>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<EHudType> _keys = new List<EHudType>();

        [SerializeField]
        private List<HudCurve> _values = new List<HudCurve>();

        public void OnAfterDeserialize()
        {
            this.Clear();

            int num = Mathf.Min(_keys.Count, _values.Count);
            for (int i = 0; i < num; ++i)
            {
                this.Add(_keys[i], _values[i]);
            }
        }

        public void OnBeforeSerialize()
        {

        }
    }

    public sealed class UIHudText : MonoBehaviour
    {
        public HudCurveDictionary CurveHash;

        private Transform mOwner = null;
        private LinkedList<UIHudLabel> mLabelList = new LinkedList<UIHudLabel>();

        public void Init(Transform owner)
        {
            mOwner = owner;
        }

        public void OnDestroy()
        {
            mOwner = null;

            using (var itr = mLabelList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current != null)
                    {
                        itr.Current.OnDestroy();
                        ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudLabel.prefab").Cycle(itr.Current.gameObject);
                    }
                }
            }
            mLabelList.Clear();
        }

        public void ShowText(EHudType hudType, double value, float duration)
        {
            GameObject go = ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudLabel.prefab").Get();
            go.name = value.ToString();

            Helper.AddChild(go, gameObject);

            HudCurve curve = null;
            if (!CurveHash.TryGetValue(hudType, out curve))
            {
                curve = CurveHash[EHudType.EHT_Default];
            }

            UIHudLabel labelText = go.GetComponent<UIHudLabel>();
            labelText.Show(hudType, value, duration, curve, Cycle);
        }

        private void Cycle(UIHudLabel label)
        {
            label.OnDestroy();
            mLabelList.Remove(label);

            ObjectPoolMgr.Instance.GetPool("/Prefabs/UI/UIHudLabel.prefab").Cycle(label.gameObject);
        }

        private void Update()
        {
            if (mOwner == null || Camera.main == null)
                return;

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, mOwner.position);
            Vector2 uiPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Instance.UICanvasTrans, screenPos, UIMgr.Instance.UICanvas.worldCamera, out uiPos))
            {
                RectTransform tr = transform as RectTransform;
                tr.anchoredPosition = uiPos;
            }
        }
    }
}