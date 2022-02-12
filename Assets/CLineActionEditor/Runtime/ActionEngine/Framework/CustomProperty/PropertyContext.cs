/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\CustomProperty\PropertyContext.cs
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
    using System.Collections.Generic;

    public sealed class PropertyContext
    {
        private Dictionary<string, TAny> propertyHash = new Dictionary<string, TAny>();

        public void AddProperty(string key, ETAnyType type)
        {
            propertyHash[key] = Helper.NewAny(type);
        }

        public void AddProperty<T>(string key, ETAnyType type, T defaultV)
        {
            propertyHash[key] = Helper.NewAny(type);
            Helper.SetAny<T>(GetProperty(key), defaultV);
        }

        public void RemoveProperty(string key)
        {
            propertyHash.Remove(key);
        }

        public TAny GetProperty(string key)
        {
            return propertyHash[key];
        }
    }
}