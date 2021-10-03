/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\Property\PetProperty.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-4-8      CLine           Created
|
+-----------------------------------------------------------------------------*/

using LitJson;

namespace CAE.Core
{
    using LitJson;
    using System;
    using System.Collections.Generic;

    public sealed class PetProperty : IProperty
    {
        private string mID;
        private string mName;
        private List<AISwitch> mAISwitch;

        #region property
        [EditorProperty("ResID(资源ID)", EditorPropertyType.EEPT_String, Edit = false)]
        public string ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        public string DebugName
        {
            get { return mName; }
        }

        public List<AISwitch> AISwitch
        {
            get { return mAISwitch; }
        }

        public void Deserialize(JsonData jd)
        {

        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return writer;
        }
    }

}