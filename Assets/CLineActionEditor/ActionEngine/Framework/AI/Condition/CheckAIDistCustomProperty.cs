/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIDistCustomProperty.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-2-15      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckAIDistCustomProperty : AICondition
    {
        private string mProperty = CustomProperty.sAttackDist;
        private ECompareType mCompareType = ECompareType.ECT_LessEqual;

        [EditorProperty("距离自定义属性", EditorPropertyType.EEPT_CustomPropertyToString)]
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

        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIDistCustomProperty; }
        }

        public bool CheckAI(Unit unit)
        {
            Debug.Assert(unit.CustomPropertyHash[mProperty] != null, string.Format("'{0}' is not defined.", mProperty));

            bool check = false;
            if (unit.Target != null)
            {
                float dist = Helper.DistanceClamp(unit, unit.Target);
                check = CustomCompare<float>.Compare(mCompareType, dist,
                    (float)unit.CustomPropertyHash[mProperty].Value);
            }

            return check;
        }

        public void OnAIStart(Unit unit, AISwitch owner)
        {

        }

        public void OnAIEnd(Unit unit, AISwitch owner)
        {

        }

        public void Update(Unit unit, AISwitch owner, float fTick)
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