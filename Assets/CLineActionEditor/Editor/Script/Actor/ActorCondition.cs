/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorCondition.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-23      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using LitJson;
    using UnityEditor;
    using UnityEngine;

    internal class ActorCondition : ActorTreeProperty
    {
        public IProperty property = null;

        public override void Draw()
        {
            var selected = window.HasSelectCondition(this);
            GUILayout.BeginHorizontal();
            {
                var offset = selected ? 50f : 20f;
                var rc = GUILayoutUtility.GetRect(window.rectInspectorRight.width - offset, window.propertyHeight);
                if (rc.height == window.propertyHeight)
                {
                    rect = rc;
                }

                if (GUI.Button(rc, property.GetType().Name))
                {

                }
                if (selected)
                {
                    window.DrawSelectable(window.editorResources.colorRed);
                }
            }
            GUILayout.EndHorizontal();

            if (selected)
            {
                GUILayout.Space(3);
                window.DrawProperty(property);
                GUILayout.Space(3);
            }
        }

        public override string GetPropertyType()
        {
            return property.GetType().Name;
        }

        public void Deserialize(JsonData jd)
        {
            property.Deserialize(jd);
        }

        public JsonWriter Serialize(JsonWriter writer)
        {
            return property.Serialize(writer);
        }

    }

}
