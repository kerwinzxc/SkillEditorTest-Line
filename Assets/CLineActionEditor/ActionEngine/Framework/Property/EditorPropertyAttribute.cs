/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\ActionEngine\Framework\Property\EditorPropertyAttribute.cs
| AUTHOR     : CLine
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-11      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using System;

    public enum EditorPropertyType
    {
        EEPT_Bool = 1,
        EEPT_Int,
        EEPT_Float,
        EEPT_String,
        EEPT_Vector2,
        EEPT_Vector3,
        EEPT_Vector4,
        EEPT_Color,
        EEPT_Quaternion,

        EEPT_Transform,
        EEPT_Texture,
        EEPT_Material,

        EEPT_GameObject,         // [GameObject => GameObject]
        EEPT_GameObjectToString, // display at GameObject, configure at string [GameObject => string]
        EEPT_TransfromToString,
        EEPT_Object,

        EEPT_Enum,

        EEPT_AnimatorStateToString,
        EEPT_AnimatorParamToString,

        EEPT_CustomPropertyToString,
        EEPT_ActionToString,

        // for list
        EEPT_GameObjectToStringList,
        EEPT_StringList,
        EEPT_ObjectHitList,
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorPropertyAttribute : Attribute
    {
        private string mPropertyName;
        private EditorPropertyType mPropertyType;
        private bool mEdit;
        private string mDescription;
        private string mDeprecated;

        public EditorPropertyAttribute(string name, EditorPropertyType type)
        {
            mPropertyName = name;
            mPropertyType = type;
            mEdit = true;
            mDeprecated = null;
        }

        #region property
        public string PropertyName
        {
            get { return mPropertyName; }
            set { mPropertyName = value; }
        }
        public EditorPropertyType PropertyType
        {
            get { return mPropertyType; }
            set { mPropertyType = value; }
        }
        public bool Edit
        {
            get { return mEdit; }
            set { mEdit = value; }
        }
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }
        public string Deprecated
        {
            get { return mDeprecated; }
            set { mDeprecated = value; }
        }
        #endregion

    }

}
