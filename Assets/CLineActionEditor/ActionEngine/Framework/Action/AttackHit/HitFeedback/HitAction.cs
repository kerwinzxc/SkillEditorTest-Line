/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\AttackHit\HitFeedback\HitAction.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-21      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class HitRandom : IProperty
    {
        private string mActionID = string.Empty;
        private int mWeight = 50;

        //[EditorProperty("受击Action", EditorPropertyType.EEPT_ActionToString)]
        [EditorProperty("受击Action", EditorPropertyType.EEPT_String)]
        public string ActionID
        {
            get { return mActionID; }
            set { mActionID = value; }
        }
        [EditorProperty("权重", EditorPropertyType.EEPT_Int)]
        public int Weight
        {
            get { return mWeight; }
            set { mWeight = value; }
        }

        public string DebugName
        {
            get { return "HitRandom"; }
        }

        public void Deserialize(JsonData jd)
        {
            mActionID = JsonHelper.ReadString(jd["ActionID"]);
            mWeight = JsonHelper.ReadInt(jd["Weight"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "ActionID", mActionID);
            JsonHelper.WriteProperty(ref writer, "Weight", mWeight);

            return writer;
        }

        public HitRandom Clone()
        {
            HitRandom hr = new HitRandom();
            hr.ActionID = this.ActionID;
            hr.Weight = this.Weight;

            return hr;
        }
    }

    public sealed class HitAction : IProperty, IHitFeedback
    {
        private List<HitRandom> mHitList = new List<HitRandom>();
        private List<string> mRandList = new List<string>();
        
        #region property
        [EditorProperty("受击列表", EditorPropertyType.EEPT_ObjectHitList)]
        public List<HitRandom> HitList
        {
            get { return mHitList; }
            set { mHitList = value; }
        }
        #endregion property

        public string DebugName
        {
            get { return "HitAction"; }
        }

        public void Deserialize(JsonData jd)
        {
            JsonData jdHitList = jd["HitList"];
            for (int i=0; i<jdHitList.Count; ++i)
            {
                HitRandom hr = new HitRandom();
                hr.Deserialize(jdHitList[i]);
                mHitList.Add(hr);
            }
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WritePropertyName("HitList");
            writer.WriteArrayStart();
            using (List<HitRandom>.Enumerator itr = mHitList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer.WriteObjectStart();
                    writer = itr.Current.Serialize(writer);
                    writer.WriteObjectEnd();
                }
            }
            writer.WriteArrayEnd();

            return writer;
        }

        public EHitFeedbackType FeedbackType
        {
            get { return EHitFeedbackType.EHT_HitAction; }
        }

        public void OnHitFeedback(Unit attacker, Unit attackee, params object[] param)
        {
            if (attackee.IsDead ||
                !attackee.ActionStatus.ActiveAction.CanHit)
            {
                return;
            }

            string hitAction = string.Empty;
            if (mHitList.Count == 1)
            {
                hitAction = mHitList[0].ActionID;
            }
            else
            {
                int max = 0;
                mRandList.Clear();
                using (List<HitRandom>.Enumerator itr = mHitList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        max += itr.Current.Weight;
                        mRandList.Add(itr.Current.ActionID);
                    }
                }

                int w = 0;
                int rand = Random.Range(0, max);
                using (List<HitRandom>.Enumerator itr = mHitList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        w += itr.Current.Weight;
                        if (rand < w)
                        {
                            hitAction = itr.Current.ActionID;
                            break;
                        }
                    }
                }
            }

            attackee.CustomPropertyHash[CustomProperty.sHitAction].Value = hitAction;
        }

    }
}