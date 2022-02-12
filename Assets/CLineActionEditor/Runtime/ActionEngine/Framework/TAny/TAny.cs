/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\TAny\TAny.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-8-20      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;

    public enum ETAnyType
    {
        Bool,
        Int32,
        Int64,
        Float,
        String,
        Vector2,
        Vector2Int,
        Vector3,
        Vector3Int,
        Vector4,
        Quaternion,
        Color,
    }

    public interface TAny
    { }

    public class TAnyVal<T> : TAny
    {
        public T value
        {
            get;
            set;
        }

        public TAnyVal(T v)
        {
            value = v;
        }
    }

    public sealed class TAnyBool : TAnyVal<bool>
    {
        public TAnyBool(bool v) : base(v)
        { }
    }

    public sealed class TAnyInt32 : TAnyVal<Int32>
    {
        public TAnyInt32(Int32 v) : base(v)
        { }
    }

    public sealed class TAnyInt64 : TAnyVal<Int64>
    {
        public TAnyInt64(Int64 v) : base(v)
        { }
    }

    public sealed class TAnyFloat : TAnyVal<float>
    {
        public TAnyFloat(float v) : base(v)
        { }
    }

    public sealed class TAnyString : TAnyVal<string>
    {
        public TAnyString(string v) : base(v)
        { }
    }

    public sealed class TAnyVector2 : TAnyVal<Vector2>
    {
        public TAnyVector2(Vector2 v) : base(v)
        { }
    }

    public sealed class TAnyVector2Int : TAnyVal<Vector2Int>
    {
        public TAnyVector2Int(Vector2Int v) : base(v)
        { }
    }

    public sealed class TAnyVector3 : TAnyVal<Vector3>
    {
        public TAnyVector3(Vector3 v) : base(v)
        { }
    }


    public sealed class TAnyVector3Int : TAnyVal<Vector3Int>
    {
        public TAnyVector3Int(Vector3Int v) : base(v)
        { }
    }

    public sealed class TAnyVector4 : TAnyVal<Vector4>
    {
        public TAnyVector4(Vector4 v) : base(v)
        { }
    }

    public sealed class TAnyQuaternion : TAnyVal<Quaternion>
    {
        public TAnyQuaternion(Quaternion v) : base(v)
        { }
    }

    public sealed class TAnyColor : TAnyVal<Color>
    {
        public TAnyColor(Color v) : base(v)
        { }
    }

    public static partial class Helper
    {
        public static void SetAny<T>(TAny obj, T v)
        {
            TAnyVal<T> t = obj as TAnyVal<T>;
            t.value = v;
        }

        public static T GetAny<T>(TAny obj)
        {
            TAnyVal<T> t = obj as TAnyVal<T>;
            return t.value;
        }

        public static TAny NewAny(ETAnyType type)
        {
            TAny obj = null;
            switch (type)
            {
                case ETAnyType.Bool:
                    obj = new TAnyBool(false);
                    break;
                case ETAnyType.Int32:
                    obj = new TAnyInt32(0);
                    break;
                case ETAnyType.Int64:
                    obj = new TAnyInt64(0);
                    break;
                case ETAnyType.Float:
                    obj = new TAnyFloat(0f);
                    break;
                case ETAnyType.String:
                    obj = new TAnyString(string.Empty);
                    break;
                case ETAnyType.Vector2:
                    break;
                case ETAnyType.Vector3:
                    obj = new TAnyVector3(Vector3.zero);
                    break;
                case ETAnyType.Vector3Int:
                    obj = new TAnyVector3Int(Vector3Int.zero);
                    break;
                case ETAnyType.Vector4:
                    obj = new TAnyVector4(Vector4.zero);
                    break;
                case ETAnyType.Quaternion:
                    obj = new TAnyQuaternion(Quaternion.identity);
                    break;
                case ETAnyType.Color:
                    obj = new TAnyColor(Color.black);
                    break;
            }
            return obj;
        }

    }
}