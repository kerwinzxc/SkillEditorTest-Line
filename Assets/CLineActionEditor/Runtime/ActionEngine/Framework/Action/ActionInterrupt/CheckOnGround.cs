/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\ActionInterrupt\CheckOnGround.cs
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
    using LitJson;
    using UnityEngine;

    public sealed class CheckOnGround : InterruptCondition, IProperty
    {
        [SerializeField] private bool mOnGround = false;

        [EditorProperty("着地比较值", EditorPropertyType.EEPT_Bool)]
        public bool OnGround
        {
            get { return mOnGround; }
            set { mOnGround = value; }
        }

        public EInterruptType InterruptType
        {
            get
            {
                return EInterruptType.EIT_CheckOnGround;
            }
        }

        public string DebugName
        {
            get { return GetType().Name; }
        }

        public bool CheckInterrupt(Unit unit)
        {
            return unit.OnGround == OnGround;
        }

        public void Deserialize(JsonData jd)
        {
            mOnGround = JsonHelper.ReadBool(jd["OnGround"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "OnGround", mOnGround);

            return writer;
        }

        public InterruptCondition Clone()
        {
            CheckOnGround obj = new CheckOnGround();
            obj.OnGround = this.OnGround;

            return obj;
        }
    }
}