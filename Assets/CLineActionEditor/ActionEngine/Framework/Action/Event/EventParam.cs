/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Action\Event\EventParam.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-19      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using System;

    public enum EEventParamType
    {
        EEPT_None,
        EEPT_Bool,
        EEPT_Int,
        EEPT_Float,
        EEPT_String,
        EEPT_Vector2,
        EEPT_Vector3,
        EEPT_Vector4,
        EEPT_Quaternion,
        EEPT_Color,
    }

    public sealed class EventParam
    {
        private EEventParamType mType = EEventParamType.EEPT_None;
        private string mValue = string.Empty;

        public EventParam(EEventParamType type, string val)
        {
            mType = type;
            mValue = val;
        }

        public bool ToBool()
        {
            return Convert.ToBoolean(Convert.ToInt32(mValue));
        }

        public new string ToString()
        {
            return mValue;
        }
    }

}
