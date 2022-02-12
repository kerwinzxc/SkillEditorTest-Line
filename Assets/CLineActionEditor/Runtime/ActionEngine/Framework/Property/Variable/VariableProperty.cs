/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\Property\Variable\VariableProperty.cs
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
    using LitJson;
    using UnityEngine;

    public sealed class VariableProperty : IProperty
    {
        [SerializeField] private string mName;
        [SerializeField] private ETAnyType mType;

        [EditorProperty("变量名称", EditorPropertyType.EEPT_String)]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        [EditorProperty("变量类型", EditorPropertyType.EEPT_Enum)]
        public ETAnyType Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public string DebugName
        {
            get { return "VariableProperty"; }
        }

        public void Deserialize(JsonData jd)
        {
            Name = JsonHelper.ReadString(jd["Name"]);
            Type = JsonHelper.ReadEnum<ETAnyType>(jd["Type"]);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            JsonHelper.WriteProperty(ref writer, "Name", Name);
            JsonHelper.WriteProperty(ref writer, "Type", Type.ToString());

            return writer;
        }
    }
}