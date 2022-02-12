/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIDistMinMax.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-15      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckAIDistMinMax : AICondition, IProperty
    {
        [SerializeField] private ECompareType mCompareMinType = ECompareType.ECT_GreaterEqual;
        [SerializeField] private ECompareType mCompareMaxType = ECompareType.ECT_Less;
        [SerializeField] private float mCompareMin = 0;
        [SerializeField] private float mCompareMax = 3;

        #region property
        [EditorProperty("最小值比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareMinType
        {
            get { return mCompareMinType; }
            set { mCompareMinType = value; }
        }
        [EditorProperty("最大值比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareMaxType
        {
            get { return mCompareMaxType; }
            set { mCompareMaxType = value; }
        }
        [EditorProperty("距离最小值", EditorPropertyType.EEPT_Float)]
        public float CompareMin
        {
            get { return mCompareMin; }
            set { mCompareMin = value; }
        }
        [EditorProperty("距离最大值", EditorPropertyType.EEPT_Float)]
        public float CompareMax
        {
            get { return mCompareMax; }
            set { mCompareMax = value; }
        }
        #endregion property

        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIDistMinMax; }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void OnEvent(AIStatus ai, AISwitch owner, Message msg)
        {

        }

        public bool CheckAI(AIStatus ai, AISwitch owner)
        {
            if (ai.Owner.Target == null)
                return false;

            float dist = Helper.Distance(ai.Owner, ai.Owner.Target);

            return CustomCompare<float>.Compare(mCompareMinType, dist, mCompareMin) &&
                CustomCompare<float>.Compare(mCompareMaxType, dist, mCompareMax);
        }

        public void OnAIStart(AIStatus ai, AISwitch owner)
        {

        }

        public void OnAIEnd(AIStatus ai, AISwitch owner)
        {

        }

        public void OnAIChanging(AIStatus ai, AISwitch owner, AISwitch next)
        {

        }

        public void Update(AIStatus ai, AISwitch owner, float fTick)
        {

        }

        public AICondition Clone()
        {
            CheckAIDistMinMax ac = new CheckAIDistMinMax();
            ac.CompareMinType = this.CompareMinType;
            ac.CompareMaxType = this.CompareMaxType;
            ac.CompareMin = this.CompareMin;
            ac.CompareMax = this.CompareMax;

            return ac;
        }

        public void Deserialize(JsonData jd)
        {
            mCompareMinType = JsonHelper.ReadEnum<ECompareType>(jd["CompareMinType"]);
            mCompareMaxType = JsonHelper.ReadEnum<ECompareType>(jd["CompareMaxType"]);
            mCompareMin = JsonHelper.ReadFloat(jd["CompareMin"]);
            mCompareMax = JsonHelper.ReadFloat(jd["CompareMax"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareMinType.ToString());
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareMaxType.ToString());
            JsonHelper.WriteProperty(ref writer, "CompareMin", mCompareMin);
            JsonHelper.WriteProperty(ref writer, "CompareMax", mCompareMax);

            return writer;
        }
    }
}