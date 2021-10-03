/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventSetCustomProperty.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-9-9      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEngine;

    public sealed class EventSetCustomProperty : EventData, IEventWhenActionEndExecute
    {
        private string mProperty;
        private bool mValBool;
        private int mValInt;
        private float mValFloat;
        private string mValString;

        #region property
        [EditorProperty("自定义属性", EditorPropertyType.EEPT_CustomPropertyToString)]
        public string Property
        {
            get { return mProperty; }
            set { mProperty = value; }
        }
        [EditorProperty("布尔值", EditorPropertyType.EEPT_Bool)]
        public bool ValBool
        {
            get { return mValBool; }
            set { mValBool = value; }
        }
        [EditorProperty("整形值", EditorPropertyType.EEPT_Int)]
        public int ValInt
        {
            get { return mValInt; }
            set { mValInt = value; }
        }
        [EditorProperty("浮点值", EditorPropertyType.EEPT_Float)]
        public float ValFloat
        {
            get { return mValFloat; }
            set { mValFloat = value; }
        }
        [EditorProperty("字符值", EditorPropertyType.EEPT_String)]
        public string ValString
        {
            get { return mValString; }
            set { mValString = value; }
        }
        #endregion

        public EEventType EventType
        {
            get { return EEventType.EET_SetCustomProperty; }
        }

        public void ExecuteOnActionEnd(Unit unit)
        {
            Execute(unit);
        }

        public void Execute(Unit unit)
        {
            if (mProperty == CustomProperty.sInputMove)
            {
                unit.CustomPropertyHash[CustomProperty.sInputMove].Value = mValBool;
            }
            else if (mProperty == CustomProperty.sInputAttack)
            {
                unit.CustomPropertyHash[CustomProperty.sInputAttack].Value = mValBool;
            }
            else if (mProperty == CustomProperty.sInputSkill)
            {
                unit.CustomPropertyHash[CustomProperty.sInputSkill].Value = mValString;
            }
            else if (mProperty == CustomProperty.sAttackCount)
            {
                Debug.LogError("CustomProperty.sAttackCount 111: " + mValInt);
                unit.CustomPropertyHash[CustomProperty.sAttackCounting].Value = mValInt;
                unit.CustomPropertyHash[CustomProperty.sAttackCount].Value = mValInt;
                unit.CustomPropertyHash[CustomProperty.sInputAttack].Value = (mValInt != 0 ? true : false);
            }
            else if (mProperty == CustomProperty.sSkillCount)
            {
                unit.CustomPropertyHash[CustomProperty.sSkillCounting].Value = mValInt;
                unit.CustomPropertyHash[CustomProperty.sSkillCount].Value = mValInt;
                unit.CustomPropertyHash[CustomProperty.sInputSkill].Value = (mValInt != 0 ? true : false);
            }
            else if (mProperty == CustomProperty.sJumpCount)
            {
                unit.CustomPropertyHash[CustomProperty.sJumpCounting].Value = mValInt;
                unit.CustomPropertyHash[CustomProperty.sJumpCount].Value = mValInt;
                unit.CustomPropertyHash[CustomProperty.sInputJump].Value = (mValInt != 0 ? true : false);
            }
            else if (mProperty == CustomProperty.sInputJump)
            {
                unit.CustomPropertyHash[CustomProperty.sInputJump].Value = mValBool;
            }
            else if (mProperty == CustomProperty.sInputSwitch)
            {
                unit.CustomPropertyHash[CustomProperty.sInputSwitch].Value = mValBool;
            }
            else if (mProperty == CustomProperty.sInputRoll)
            {
                unit.CustomPropertyHash[CustomProperty.sInputRoll].Value = mValBool;
            }
            else if (mProperty == CustomProperty.sVelocityY)
            {
                unit.CustomPropertyHash[CustomProperty.sVelocityY].Value = mValFloat;
            }
            else if (mProperty == CustomProperty.sInputSkillPosition)
            {
                unit.CustomPropertyHash[CustomProperty.sInputSkillPosition].Value = Vector3.positiveInfinity;
            }
            else if (mProperty == CustomProperty.sHitAction)
            {
                unit.CustomPropertyHash[CustomProperty.sHitAction].Value = mValString;
            }
            else if (mProperty == CustomProperty.sAI)
            {
                unit.CustomPropertyHash[CustomProperty.sAI].Value = mValString;
            }
        }

        public void Deserialize(LitJson.JsonData jd)
        {
            mProperty = JsonHelper.ReadString(jd["Property"]);
            mValBool = JsonHelper.ReadBool(jd["ValBool"]);
            mValInt = JsonHelper.ReadInt(jd["ValInt"]);
            mValFloat = JsonHelper.ReadFloat(jd["ValFloat"]);
            mValString = JsonHelper.ReadString(jd["ValString"]);
        }

        public LitJson.JsonWriter Serialize(LitJson.JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Property", mProperty);
            JsonHelper.WriteProperty(ref writer, "ValBool", mValBool);
            JsonHelper.WriteProperty(ref writer, "ValInt", mValInt);
            JsonHelper.WriteProperty(ref writer, "ValFloat", mValFloat);
            JsonHelper.WriteProperty(ref writer, "ValString", mValString);

            return writer;
        }

        public EventData Clone()
        {
            EventSetCustomProperty evt = new EventSetCustomProperty();
            evt.mProperty = this.mProperty;
            evt.mValBool = this.mValBool;
            evt.mValInt = this.mValInt;
            evt.mValFloat = this.mValFloat;
            evt.mValString = this.mValString;

            return evt;
        }
    }
}