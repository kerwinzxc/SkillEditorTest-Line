/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\AISwitch.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

using System;
using LitJson;

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class AISwitch : IProperty
    {
        [SerializeField] private string mID;
        [SerializeField] private string mName;
        [SerializeField] private string mDescription;
        [SerializeField] private int mOrder; // order by asc, same order using random by mRandomWeight
        [SerializeField] private string mActionID;
        [SerializeField] private int mRandomWeight;
        [SerializeReference] private List<AICondition> mConditionList = new List<AICondition>();

        #region property
        [EditorProperty("ID", EditorPropertyType.EEPT_String)]
        public string ID
        {
            get { return mID; }
            set { mID = value; }
        }
        [EditorProperty("名称", EditorPropertyType.EEPT_String)]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        [EditorProperty("描述", EditorPropertyType.EEPT_String)]
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }
        [EditorProperty("AI优先级", EditorPropertyType.EEPT_Int, Description="升序排列,相同序号则按权重随机")]
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }
        [EditorProperty("AI对应的Action", EditorPropertyType.EEPT_Action)]
        public string ActionID
        {
            get { return mActionID; }
            set { mActionID = value; }
        }
        [EditorProperty("随机队列权重", EditorPropertyType.EEPT_Int)]
        public int RandomWeight
        {
            get { return mRandomWeight; }
            set { mRandomWeight = value; }
        }
        #endregion

        public string DebugName
        {
            get
            {
                return mName + "+" + Order.ToString();
            }
        }

        public List<AICondition> ConditionList
        {
            get { return mConditionList; }
            set { mConditionList = value; }
        }

        public void Deserialize(JsonData jd)
        {
            mID = JsonHelper.ReadString(jd["ID"]);
            mName = JsonHelper.ReadString(jd["Name"]);
            mDescription = JsonHelper.ReadString(jd["Description"]);
            mOrder = JsonHelper.ReadInt(jd["Order"]);

            mActionID = JsonHelper.ReadString(jd["ActionID"]);
            mRandomWeight = JsonHelper.ReadInt(jd["RandomWeight"]);

            JsonData jdConditions = jd["Conditions"];
            for (int i = 0; i < jdConditions.Count; ++i)
            {
                EAIConditionType eit = JsonHelper.ReadEnum<EAIConditionType>(jdConditions[i]["ConditionType"]);
                AICondition cond = Add(eit);
                IProperty p = cond as IProperty;
                p.Deserialize(jdConditions[i]);
            }
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            writer.WriteObjectStart();
            JsonHelper.WriteProperty(ref writer, "ID", mID);
            JsonHelper.WriteProperty(ref writer, "Name", mName);
            JsonHelper.WriteProperty(ref writer, "Description", mDescription);
            JsonHelper.WriteProperty(ref writer, "Order", mOrder);

            JsonHelper.WriteProperty(ref writer, "ActionID", mActionID);
            JsonHelper.WriteProperty(ref writer, "RandomWeight", mRandomWeight);

            writer.WritePropertyName("Conditions");
            writer.WriteArrayStart();
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    writer.WriteObjectStart();
                    JsonHelper.WriteProperty(ref writer, "ConditionType", itr.Current.AIType.ToString());
                    IProperty p = itr.Current as IProperty;
                    writer = p.Serialize(writer);
                    writer.WriteObjectEnd();
                }
            }
            writer.WriteArrayEnd();

            writer.WriteObjectEnd();

            return writer;
        }

        public AISwitch Clone()
        {
            AISwitch ai = new AISwitch();
            ai.ID = this.ID;
            ai.Name = this.Name;
            ai.Description = this.Description;
            ai.Order = this.Order;

            ai.ActionID = this.ActionID;
            ai.RandomWeight = this.RandomWeight;

            using (List<AICondition>.Enumerator itr = this.ConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    ai.ConditionList.Add(itr.Current.Clone());
                }
            }

            return ai;
        }

        public AICondition Add(EAIConditionType t)
        {
            AICondition cond = null;
            switch (t)
            {
                case EAIConditionType.EAT_CheckAICD:
                    cond = new CheckAICD();
                    break;
                case EAIConditionType.EAT_CheckAIHP:
                    cond = new CheckAIHP();
                    break;
                case EAIConditionType.EAT_CheckAIBornPos:
                    cond = new CheckAIBornPos();
                    break;
                case EAIConditionType.EAT_CheckAITargetIsNull:
                    cond = new CheckAITargetIsNull();
                    break;
                case EAIConditionType.EAT_CheckAITargetNotNull:
                    cond = new CheckAITargetNotNull();
                    break;
                case EAIConditionType.EAT_CheckAIDead:
                    cond = new CheckAIDead();
                    break;
                case EAIConditionType.EAT_CheckAIDistCustomProperty:
                    cond = new CheckAIDistCustomProperty();
                    break;
                case EAIConditionType.EAT_CheckAIDistMinMax:
                    cond = new CheckAIDistMinMax();
                    break;
                case EAIConditionType.EAT_CheckAIChase:
                    cond = new CheckAIChase();
                    break;
                case EAIConditionType.EAT_CheckAIPatrol:
                    cond = new CheckAIPatrol();
                    break;
            }

            mConditionList.Add(cond);

            return cond;
        }

        public void Del(AICondition cond)
        {
            mConditionList.Remove(cond);
        }

        public void DelAll()
        {
            mConditionList.Clear();
        }

        public void OnMessage(AIStatus ai, Message msg)
        {
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.OnEvent(ai, this, msg);
                }
            }
        }

        public void OnAIStart(AIStatus ai)
        {
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.OnAIStart(ai, this);
                }
            }
        }

        public void OnAIEnd(AIStatus ai)
        {
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.OnAIEnd(ai, this);
                }
            }
        }

        public void OnAIChanging(AIStatus ai, AISwitch next)
        {
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.OnAIChanging(ai, this, next);
                }
            }
        }

        public void Update(AIStatus ai, float fTick)
        {
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Update(ai, this, fTick);
                }
            }
        }

        public bool CheckAI(AIStatus ai, float fTick)
        {
            using (List<AICondition>.Enumerator itr = mConditionList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (!itr.Current.CheckAI(ai, this))
                        return false;
                }
            }

            return true;
        }

    }
}
