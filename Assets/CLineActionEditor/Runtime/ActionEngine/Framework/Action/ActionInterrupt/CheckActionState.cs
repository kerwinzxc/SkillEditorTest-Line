/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckActionState.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-18      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using LitJson;

    public sealed class CheckActionState : InterruptCondition, IProperty
    {
        [SerializeField] private EActionState mActionState;

        [EditorProperty("Action状态", EditorPropertyType.EEPT_Enum)]
        public EActionState ActionState
        {
            get { return mActionState; }
            set { mActionState = value; }
        }

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckActionState;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            return unit.ActionStatus.ActiveAction != null && unit.ActionStatus.ActiveAction.ActionStatus == ActionState;
        }

        public void Deserialize(JsonData jd)
        {
            mActionState = JsonHelper.ReadEnum<EActionState>(jd["ActionState"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "ActionState", mActionState.ToString());

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckActionState obj = new CheckActionState();
            obj.ActionState = this.ActionState;

            return obj;
        }
    }
}