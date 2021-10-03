/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\Editor\Script\ActionWindowRole.cs
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
        private string mCarrierName = "";

        private void DrawInspectorRole()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Player"))
            {
                CreatePlayer();
            }
            if (GUILayout.Button("Monster"))
            {
                CreateMonster();
            }
            if (GUILayout.Button("Pet"))
            {
                CreatePet();
            }
            if (GUILayout.Button("Delete"))
            {
                DeleteRole();
            }
            if (GUILayout.Button("Attach"))
            {
                AttachUnit();
            }
            if (GUILayout.Button("Carrier"))
            {
                AttachCarrier();
            }
            if (GUILayout.Button("Save"))
            {
                SaveRole();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            mScrollViewPosition = GUILayout.BeginScrollView(mScrollViewPosition, false, true);

            GUI.color = Color.cyan;
            GUILayout.Label("horse");
            GUI.color = Color.white;
            mCarrierName = GUILayout.TextField(mCarrierName);

            GUI.color = Color.cyan;
            GUILayout.Label("player");
            GUI.color = Color.white;
            DrawPropertyList(mPlayerPropertyList);
            GUILayout.Space(10);
            GUI.color = Color.cyan;
            GUILayout.Label("monster");
            GUI.color = Color.white;
            DrawPropertyList(mMonsterPropertyList);
            GUILayout.Space(10);
            GUI.color = Color.cyan;
            GUILayout.Label("ai player");
            GUI.color = Color.white;
            DrawPropertyList(mAIPlayerPropertyList);
            GUILayout.Space(10);
            GUI.color = Color.cyan;
            GUILayout.Label("pet");
            GUI.color = Color.white;
            DrawPropertyList(mPetPropertyList);
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private void SaveRole()
        {
            SerializeProperty<PlayerProperty>(FilePath + "Unit/Player.json", mPlayerPropertyList);
            SerializeProperty<MonsterProperty>(FilePath + "Unit/Monster.json", mMonsterPropertyList);
            SerializeProperty<PetProperty>(FilePath + "Unit/Pet.json", mPetPropertyList);
        }

        private void CreatePlayer()
        {
            PlayerProperty player = new PlayerProperty();
            player.ID = (mPlayerPropertyList.Count + 1).ToString();
            mPlayerPropertyList.Add(player);
        }

        private void CreateMonster()
        {
            MonsterProperty monster = new MonsterProperty();
            monster.ID = (mMonsterPropertyList.Count + 1).ToString();
            mMonsterPropertyList.Add(monster);
        }


        private void CreatePet()
        {
            PetProperty pet = new PetProperty();
            pet.ID = (mPetPropertyList.Count + 1).ToString();
            mPetPropertyList.Add(pet);
        }

        private void DeleteRole()
        {
            if (mCurProperty is PlayerProperty)
            {
                mPlayerPropertyList.Remove(mCurProperty as PlayerProperty);
                mCurProperty = null;
                UpdatePlayerID();
            }
            if (mCurProperty is MonsterProperty)
            {
                mMonsterPropertyList.Remove(mCurProperty as MonsterProperty);
                mCurProperty = null;
                //UpdateMonsterID();
            }
            if (mCurProperty is PetProperty)
            {
                mPetPropertyList.Remove(mCurProperty as PetProperty);
                mCurProperty = null;
                UpdatePetID();
            }
        }

        private void UpdatePlayerID()
        {
            for (int i = 0; i < mPlayerPropertyList.Count; ++i)
            {
                PlayerProperty p = mPlayerPropertyList[i] as PlayerProperty;
                p.ID = (i + 1).ToString();
            }
        }

        private void UpdateMonsterID()
        {
            for (int i = 0; i < mMonsterPropertyList.Count; ++i)
            {
                MonsterProperty m = mMonsterPropertyList[i] as MonsterProperty;
                m.ID = (i + 1).ToString();
            }
        }

        private void UpdatePetID()
        {
            for (int i = 0; i < mPetPropertyList.Count; ++i)
            {
                PetProperty pa = mPetPropertyList[i] as PetProperty;
                pa.ID = (i + 1).ToString();
            }
        }

    }

}