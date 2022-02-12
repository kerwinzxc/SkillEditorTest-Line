/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIDistCustomProperty.cs
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

    public sealed class CheckAIDistCustomProperty : AICondition, IProperty
    {
        [SerializeField] private string mProperty = PropertyName.sAttackDist;
        [SerializeField] private ECompareType mCompareType = ECompareType.ECT_LessEqual;

        #region property
        [EditorProperty("距离自定义属性", EditorPropertyType.EEPT_CustomProperty)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }

        [EditorProperty("距离比较条件", EditorPropertyType.EEPT_Enum)]
        public ECompareType CompareType
        {
            get { return mCompareType; }
            set { mCompareType = value; }
        }
        #endregion

        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIDistCustomProperty; }
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
            bool check = false;
            if (ai.Owner.Target != null)
            {
                float dist = Helper.DistanceClamp(ai.Owner, ai.Owner.Target);
                float v = Helper.GetAny<float>(ai.Owner.PropertyContext.GetProperty(mProperty));
                check = CustomCompare<float>.Compare(mCompareType, dist, v);
            }

            return check;
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
            CheckAIDistCustomProperty ac = new CheckAIDistCustomProperty();
            ac.Property = this.Property;
            ac.CompareType = this.CompareType;

            return ac;
        }

        public void Deserialize(JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mCompareType = JsonHelper.ReadEnum<ECompareType>(jd["CompareType"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", mProperty);
            JsonHelper.WriteProperty(ref writer, "CompareType", mCompareType.ToString());

            return writer;
        }
    }
}