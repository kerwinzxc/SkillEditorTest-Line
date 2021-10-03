/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\BUFF\CBuffProperty.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2020-1-8      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using LitJson;
    using System.Collections.Generic;

    public sealed class CBuffProperty : BuffProperty
    {
        private List<string> mAddBuffIDList = new List<string>();
        private List<string> mDelBuffIDList = new List<string>();
        private List<BuffCondition> mConditionList = new List<BuffCondition>();

        #region property
        [EditorProperty("添加BUFF列表", EditorPropertyType.EEPT_StringList)]
        public List<string> AddBuffIDList
        {
            get { return mAddBuffIDList; }
            set { mAddBuffIDList = value; }
        }
        [EditorProperty("删除BUFF列表", EditorPropertyType.EEPT_StringList)]
        public List<string> DelBuffIDList
        {
            get { return mDelBuffIDList; }
            set { mDelBuffIDList = value; }
        }
        #endregion

        public List<BuffCondition> ConditionList
        {
            get { return mConditionList; }
            set { mConditionList = value; }
        }

        public override void Deserialize(JsonData jd)
        {
            base.Deserialize(jd);

            mAddBuffIDList = JsonHelper.ReadListString(jd["AddBuffIDList"]);
            mDelBuffIDList = JsonHelper.ReadListString(jd["DelBuffIDList"]);

            JsonData jdConditions = jd["Conditions"];
            for (int i = 0; i < jdConditions.Count; ++i)
            {
                EBuffConditionType eit = JsonHelper.ReadEnum<EBuffConditionType>(jdConditions[i]["ConditionType"]);
                BuffCondition cond = Add(eit);
                cond.Deserialize(jdConditions[i]);
            }
        }

        public override JsonWriter Serialize(JsonWriter writer)
        {
            base.Serialize(writer);

            JsonHelper.WriteProperty(ref writer, "AddBuffIDList", mAddBuffIDList);
            JsonHelper.WriteProperty(ref writer, "DelBuffIDList", mDelBuffIDList);

            writer.WritePropertyName("Conditions");
            writer.WriteArrayStart();
            using (List<BuffCondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer.WriteObjectStart();
                    JsonHelper.WriteProperty(ref writer, "ConditionType", itr.Current.BuffCondType.ToString());
                    writer = itr.Current.Serialize(writer);
                    writer.WriteObjectEnd();
                }
            }
            writer.WriteArrayEnd();

            return writer;
        }

        public BuffCondition Add(EBuffConditionType t)
        {
            BuffCondition cond = null;
            switch (t)
            {
                case EBuffConditionType.ECT_CheckBuffBeforeAttackCount:
                    cond = new CheckBuffBeforeAttackCount();
                    break;
                case EBuffConditionType.ECT_CheckBuffAfterAttackCount:
                    cond = new CheckBuffAfterAttackCount();
                    break;
                case EBuffConditionType.ECT_CheckBuffBeforeUseSkill:
                    cond = new CheckBuffBeforeUseSkill();
                    break;
                case EBuffConditionType.ECT_CheckBuffAfterUseSkill:
                    cond = new CheckBuffAfterUseSkill();
                    break;
                case EBuffConditionType.ECT_CheckBuffHitCount:
                    cond = new CheckBuffHitCount();
                    break;
                case EBuffConditionType.ECT_CheckBuffCombos:
                    cond = new CheckBuffCombos();
                    break;
                case EBuffConditionType.ECT_CheckBuffCriticalCount:
                    cond = new CheckBuffCriticalCount();
                    break;
                case EBuffConditionType.ECT_CheckBuffKillMonster:
                    cond = new CheckBuffKillMonster();
                    break;
                case EBuffConditionType.ECT_CheckBuffHP:
                    cond = new CheckBuffHP();
                    break;
                case EBuffConditionType.ECT_CheckBuffEvade:
                    cond = new CheckBuffEvade();
                    break;
                case EBuffConditionType.ECT_CheckBuffSummon:
                    cond = new CheckBuffSummon();
                    break;
            }

            mConditionList.Add(cond);

            return cond;
        }

        public void Del(BuffCondition cond)
        {
            mConditionList.Remove(cond);
        }

        public void DelAll()
        {
            mConditionList.Clear();
        }
    }
}