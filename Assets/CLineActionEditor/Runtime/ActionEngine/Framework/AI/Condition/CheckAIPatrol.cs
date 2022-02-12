/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIPatrol.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-9-1      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;

    public sealed class CheckAIPatrol : AICondition, IProperty
    {
        [SerializeField] private float mRange = 5f;
        [SerializeField] private int mRandom = 30;

        private Vector3 mTargetPosition = Vector3.zero;

        #region property
        [EditorProperty("巡逻半径", EditorPropertyType.EEPT_Float)]
        public float Range
        {
            get { return mRange; }
            set { mRange = value; }
        }
        [EditorProperty("巡逻几率[0,100)", EditorPropertyType.EEPT_Int)]
        public int Random
        {
            get { return mRandom; }
            set { mRandom = Helper.Clamp(value, 0, 99); }
        }
        #endregion

        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIPatrol; }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void OnEvent(AIStatus ai, AISwitch owner, Message msg)
        { }

        public bool CheckAI(AIStatus ai, AISwitch owner)
        {
            if (mTargetPosition.Equals(Vector3.zero))
            {
                int r = UnityEngine.Random.Range(0, 100);
                if (r < mRandom)
                {
                    mTargetPosition = ai.Owner.BornPosition + UnityEngine.Random.insideUnitSphere * Range;
                    mTargetPosition = Helper.ConvertNavMeshPoint(mTargetPosition);
                }
            }
            else
            {
                if (ai.Owner.ActionStatus.ActiveAction.ActionStatus == EActionState.Move &&
                    ai.Owner.ActionStatus.CanMove &&
                    ai.NavAgent.enabled)
                {
                    ai.StartNavigation(mTargetPosition);
                }
            }

            return !mTargetPosition.Equals(Vector3.zero);
        }

        public void OnAIStart(AIStatus ai, AISwitch owner)
        {

        }

        public void OnAIEnd(AIStatus ai, AISwitch owner)
        {

        }

        public void OnAIChanging(AIStatus ai, AISwitch owner, AISwitch next)
        {
            if (owner != next)
            {
                mTargetPosition = Vector3.zero;
            }
        }

        public void Update(AIStatus ai, AISwitch owner, float fTick)
        {
            if (!mTargetPosition.Equals(Vector3.zero) &&
                Helper.DistanceSqr(mTargetPosition, ai.Owner.Position, true) < 0.001f)
            {
                mTargetPosition = Vector3.zero;
            }
        }

        public AICondition Clone()
        {
            CheckAIPatrol ac = new CheckAIPatrol();
            ac.Range = Range;
            ac.Random = Random;

            return ac;
        }

        public void Deserialize(JsonData jd)
        {
            mRange = JsonHelper.ReadFloat(jd["Range"]);
            mRandom = JsonHelper.ReadInt(jd["Random"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Range", mRange);
            JsonHelper.WriteProperty(ref writer, "Random", mRandom);

            return writer;
        }
    }
}