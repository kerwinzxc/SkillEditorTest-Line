/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Util\JsonHelper.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-11      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using LitJson;
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public static class JsonHelper
    {
        public static void WriteProperty(ref JsonWriter wr, string propertyName, string value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(string.IsNullOrEmpty(value) ? string.Empty : value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, double value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, int value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, long value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, bool value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, ulong value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, decimal value)
        {
            wr.WritePropertyName(propertyName);
            wr.Write(value);
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, Vector3 value)
        {
            wr.WritePropertyName(propertyName);
            wr.WriteObjectStart();
            WriteProperty(ref wr, "x", value.x);
            WriteProperty(ref wr, "y", value.y);
            WriteProperty(ref wr, "z", value.z);
            wr.WriteObjectEnd();
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, Color value)
        {
            wr.WritePropertyName(propertyName);
            wr.WriteObjectStart();
            WriteProperty(ref wr, "r", value.r);
            WriteProperty(ref wr, "g", value.g);
            WriteProperty(ref wr, "b", value.b);
            WriteProperty(ref wr, "a", value.a);
            wr.WriteObjectEnd();
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, List<string> value)
        {
            wr.WritePropertyName(propertyName);
            wr.WriteArrayStart();
            for (int i=0; i < value.Count; ++i)
            {
                wr.Write(value[i]);
            }
            wr.WriteArrayEnd();
        }

        public static void WriteProperty(ref JsonWriter wr, string propertyName, LinkedList<string> value)
        {
            wr.WritePropertyName(propertyName);
            wr.WriteArrayStart();
            using (var itr = value.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    wr.Write(itr.Current);
                }
            }
            wr.WriteArrayEnd();
        }

        public static string ReadString(JsonData jd)
        {
            if (null == jd || string.IsNullOrEmpty(jd.ToString()))
            {
                return null;
            }
            else
            {
                return jd.ToString();
            }
        }

        public static int ReadInt(JsonData jd)
        {
            return Convert.ToInt32(ReadString(jd));
        }

        public static bool ReadBool(JsonData jd)
        {
            return Convert.ToBoolean(ReadString(jd));
        }

        public static float ReadFloat(JsonData jd)
        {
            return Convert.ToSingle(ReadString(jd));
        }

        public static Vector3 ReadVector3(JsonData jd)
        {
            Vector3 v = Vector3.zero;
            v.x = Convert.ToSingle(ReadString(jd["x"]));
            v.y = Convert.ToSingle(ReadString(jd["y"]));
            v.z = Convert.ToSingle(ReadString(jd["z"]));

            return v;
        }

        public static Color ReadColor(JsonData jd)
        {
            Color c = Color.black;
            c.r = Convert.ToSingle(ReadString(jd["r"]));
            c.g = Convert.ToSingle(ReadString(jd["g"]));
            c.b = Convert.ToSingle(ReadString(jd["b"]));
            c.a = Convert.ToSingle(ReadString(jd["a"]));

            return c;
        }

        public static List<string> ReadListString(JsonData jd)
        {
            List<string> lt = new List<string>();

            for (int i = 0; i < jd.Count; ++i)
            {
                lt.Add(ReadString(jd[i]));
            }

            return lt;
        }

        public static LinkedList<string> ReadLinkListString(JsonData jd)
        {
            LinkedList<string> lt = new LinkedList<string>();

            for (int i = 0; i < jd.Count; ++i)
            {
                lt.AddLast(ReadString(jd[i]));
            }

            return lt;
        }

        public static T ReadEnum<T>(JsonData jd)
        {
            return (T)Enum.Parse(typeof(T), ReadString(jd));
        }
    }
}
