/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\ActionWindow\ActionWindowWeapon.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-26      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEditor;
    using UnityEngine;

    internal sealed partial class ActionWindow
    {
        [SerializeField] private ActorTreeProperty actorWeaponTree = null;

        private void InitInspectorWeapon()
        {
            actorWeaponTree = ScriptableObject.CreateInstance<ActorTreeProperty>();
            actorWeaponTree.Init(null);
            actorWeaponTree.AddManipulator(new ActorTreePropertyManipulator(actorWeaponTree));
        }

        private void DrawInspectorWeapon()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(2);
                GUILayout.BeginHorizontal(editorResources.btnToolBoxStyle, GUILayout.Width(rectInspectorLeft.width), GUILayout.Height(16));
                {
                    if (GUILayout.Button("New"))
                    {
                        CreateProperty(actorWeaponTree, typeof(WeaponProperty));
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        DeleteProperty();
                    }
                    if (GUILayout.Button("Attach"))
                    {
                        AttachWeapon();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        SaveWeapon();
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

                leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, false, true);
                {
                    actorWeaponTree.HandleManipulatorsEvents(this, Event.current);
                    actorWeaponTree.Draw();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void SaveWeapon()
        {
            SerializeProperty<WeaponProperty>(FilePath + "Unit/Weapon.json", actorWeaponTree);

            EditorUtility.DisplayDialog("INFO", "Succeed to save!", "OK");
        }

        private void AttachWeapon()
        {
            if (UnitWrapper.Instance.UnitWrapperUnit == null)
            {
                EditorUtility.DisplayDialog("INFO", "please attach role firstly!", "YES");
                return;
            }

            var selectProperty = GetActorProperty() as ActorProperty;
            if (selectProperty == null)
            {
                EditorUtility.DisplayDialog("INFO", "please select weapon!", "YES");
                return;
            }

            if (selectProperty.property is WeaponProperty wp)
            {
                if (!string.IsNullOrEmpty(wp.Prefab))
                {
                    UnitWrapper.Instance.BuildWeapon(wp);
                }
                else
                {
                    EditorUtility.DisplayDialog("INFO", "please set weapon prefab!", "YES");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("INFO", "please select weapon!", "YES");
            }
        }

    }

}