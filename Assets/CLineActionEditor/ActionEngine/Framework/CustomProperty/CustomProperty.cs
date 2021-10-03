/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\CustomProperty\CustomProperty.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-12      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using System;
    using System.Collections.Generic;

    public enum EProperyType
    {
        EPT_Bool,
        EPT_Int,
        EPT_Float,
        EPT_String,
        EPT_Vector3,
    }

    public sealed class CustomProperty
    {
        private string mName;
        private EProperyType mType;
        private object mValue;

        public CustomProperty(string name, string type, string val)
        {
            mName = name;
            switch (type)
            {
                case "bool":
                    {
                        mType = EProperyType.EPT_Bool;
                        mValue = Convert.ToBoolean(Convert.ToInt32(val));
                    }
                    break;
                case "int":
                    {
                        mType = EProperyType.EPT_Int;
                        mValue = Convert.ToInt32(val);
                    }
                    break;
                case "float":
                    {
                        mType = EProperyType.EPT_Float;
                        mValue = Convert.ToSingle(val);
                    }
                    break;
                case "string":
                    {
                        mType = EProperyType.EPT_String;
                        mValue = val;
                    }
                    break;
            }
        }

        public CustomProperty(string name, EProperyType type, object val)
        {
            mName = name;
            mType = type;
            mValue = val;
        }

        public string Name
        {
            get { return mName; }
        }

        public EProperyType PropertyType
        {
            get { return mType; }
        }

        public object Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
            }
        }


        public static readonly string sInputMove = "InputMove";
        public static readonly string sInputAttack = "InputAttack";
        public static readonly string sInputSkill = "InputSkill";
        public static readonly string sAttackCount = "AttackCount";
        public static readonly string sSkillCount = "SkillCount";
        public static readonly string sJumpCount = "JumpCount";

        public static readonly string sInputJump = "InputJump";
        public static readonly string sInputSwitch = "InputSwitchWeapon";
        public static readonly string sInputRoll = "InputRoll";
        public static readonly string sInputLongPressed = "InputLongPressed";

        public static readonly string sVelocityY = "VelocityY";
        public static readonly string sKeepAttakRandom = "KeepAttakRandom";
        public static readonly string sInputSkillPosition = "InputSkillPosition";//

        public static readonly string sAttackCounting = "AttackCounting";//
        public static readonly string sSkillCounting = "SkillCounting";//
        public static readonly string sJumpCounting = "JumpCounting";//

        public static readonly string sAttackDist = "AttackDist";
        public static readonly string sChaseDist = "ChaseDist";
        public static readonly string sSearchDist = "SearchDist";

        public static readonly string sHitAction = "HitAction";//
        public static readonly string sHitAttackerSpeed = "HitAttackerSpeed";

        // AI
        public static readonly string sAI = "AI";

        //public static readonly string sHitNormal = "HitNormal";         //普通
        //public static readonly string sHitKnockBack = "HitKnockBack";   //击退
        //public static readonly string sHitKnockDown = "HitKnockDown";   //击倒
        //public static readonly string sHitKnockOut = "HitKnockOut";     //击飞
        //public static readonly string sHitInAir = "HitInAir";           //浮空(比如屠夫勾子)

        private static List<string> mList = new List<string>();

        static CustomProperty()
        {
            mList.Add(sInputMove);
            mList.Add(sInputAttack);
            mList.Add(sInputSkill);
            mList.Add(sAttackCount);
            mList.Add(sSkillCount);
            mList.Add(sJumpCount);
            mList.Add(sInputJump);
            mList.Add(sInputSwitch);
            mList.Add(sInputRoll);
            mList.Add(sInputLongPressed);

            mList.Add(sVelocityY);
            mList.Add(sKeepAttakRandom);

            mList.Add(sAttackDist);
            mList.Add(sChaseDist);
            mList.Add(sSearchDist);

            mList.Add(sAI);
            mList.Add(sHitAction);
        }

        public static List<string> CustomPropertyList()
        {
            return mList;
        }

    }

}
