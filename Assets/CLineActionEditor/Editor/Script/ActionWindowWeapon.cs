/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \actioneditor\Assets\CLineActionEditor\Editor\Script\ActionWindowWeapon.cs
| AUTHOR     : CLine
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2019-11-20      CLine           Created
|
+-----------------------------------------------------------------------------*/

namespace CAE.Core
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public partial class ActionWindow : EditorWindow
    {
        private void DrawInspectorWeapon()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("New"))
            {
                CreateWeapon();
            }
            if (GUILayout.Button("Delete"))
            {
                DeleteWeapon();
            }
            if (GUILayout.Button("Attach"))
            {
                AttachWeapon();
            }
            if (GUILayout.Button("Save"))
            {
                SaveWeapon();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            mScrollViewPosition = GUILayout.BeginScrollView(mScrollViewPosition, false, true);
            DrawPropertyList(mWeaponPropertyList);
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private void CreateWeapon()
        {
            WeaponProperty weapon = new WeaponProperty();
            mWeaponPropertyList.Add(weapon);
        }

        private void DeleteWeapon()
        {
            if (mCurProperty is WeaponProperty)
            {
                mWeaponPropertyList.Remove(mCurProperty as WeaponProperty);
                mCurProperty = null;
            }
        }

        private void SaveWeapon()
        {
            SerializeProperty<WeaponProperty>(FilePath + "Unit/Weapon.json", mWeaponPropertyList);
        }

    }

}