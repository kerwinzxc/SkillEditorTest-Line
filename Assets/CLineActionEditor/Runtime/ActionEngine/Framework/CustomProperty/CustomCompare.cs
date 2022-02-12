/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\CustomProperty\CustomCompare.cs
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

    public enum ECompareType
    {
        ECT_Equal = 0,
        ECT_NotEqual = 1,
        ECT_Greater = 2,
        ECT_Less = 3,
        ECT_GreaterEqual = 4,
        ECT_LessEqual = 5,
    }

    public static class CustomCompare<T> where T : IComparable<T>
    {
        public static bool Compare(ECompareType type, T lhs, T rhs)
        {
            bool flag = false;

            switch (type)
            {
                case ECompareType.ECT_Equal:
                    flag = (lhs.CompareTo(rhs) == 0);
                    break;
                case ECompareType.ECT_NotEqual:
                    flag = (lhs.CompareTo(rhs) != 0);
                    break;
                case ECompareType.ECT_Greater:
                    flag = (lhs.CompareTo(rhs) > 0);
                    break;
                case ECompareType.ECT_Less:
                    flag = (lhs.CompareTo(rhs) < 0);
                    break;
                case ECompareType.ECT_GreaterEqual:
                    flag = (lhs.CompareTo(rhs) >= 0);
                    break;
                case ECompareType.ECT_LessEqual:
                    flag = (lhs.CompareTo(rhs) <= 0);
                    break;
                default:
                    flag = false;
                    break;
            }

            return flag;
        }
    }

}
