/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\CustomProperty\PropertyName.cs
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

namespace SuperCLine.ActionEngine
{
    using System;
    using System.Collections.Generic;

    public static class PropertyName
    {
        public static readonly string sInputMove = "InputMove";
        public static readonly string sInputAttack = "InputAttack";
        public static readonly string sInputSkill = "InputSkill";
        public static readonly string sInputJump = "InputJump";
        public static readonly string sInputSwitch = "InputSwitchWeapon";
        public static readonly string sInputRoll = "InputRoll";
        public static readonly string sInputLongPressed = "InputLongPressed";

        public static readonly string sVelocityY = "VelocityY";
        public static readonly string sInputSkillPosition = "InputSkillPosition";//

        public static readonly string sAttackDist = "AttackDist";
        public static readonly string sChaseDist = "ChaseDist";
        public static readonly string sSearchDist = "SearchDist";

        public static readonly string sHitAction = "HitAction";//
        public static readonly string sHitAttackerSpeed = "HitAttackerSpeed";

        // AI
        public static readonly string sAI = "AI";

        // New Property Name Format
        public static readonly string VarBoolTag = "VarBoolTag";
        public static readonly string VarStringTag = "VarStringTag";
        public static readonly string VarInt32AttackRandom = "VarInt32AttackRandom";


        private static List<string> mList = new List<string>();

        static PropertyName()
        {
            mList.Add(VarBoolTag);
            mList.Add(VarStringTag);
            mList.Add(VarInt32AttackRandom);

            mList.Add(sInputMove);
            mList.Add(sInputAttack);
            mList.Add(sInputSkill);
            mList.Add(sInputJump);
            mList.Add(sInputSwitch);
            mList.Add(sInputRoll);
            mList.Add(sInputLongPressed);

            mList.Add(sVelocityY);

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
