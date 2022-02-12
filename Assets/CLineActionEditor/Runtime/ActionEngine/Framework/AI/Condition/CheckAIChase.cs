/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\AI\Condition\CheckAIChase.cs
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

    public sealed class CheckAIChase : AICondition, IProperty
    {
        public EAIConditionType AIType
        {
            get { return EAIConditionType.EAT_CheckAIChase; }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public void OnEvent(AIStatus ai, AISwitch owner, Message msg)
        { }

        public bool CheckAI(AIStatus ai, AISwitch owner)
        {
            if (ai.Owner.Target != null &&
                ai.Owner.ActionStatus.ActiveAction.ActionStatus == EActionState.Move &&
                ai.Owner.ActionStatus.CanMove &&
                ai.NavAgent.enabled)
            {
                ai.StartNavigation(ai.Owner.Target.Position);
            }

            return true;
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
            CheckAIChase ac = new CheckAIChase();

            return ac;
        }

        public void Deserialize(JsonData jd)
        {

        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return writer;
        }
    }
}