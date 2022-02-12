using UnityEngine;
using System.Collections;
using UnityEditor;
#if USE_PLUGINWEAPONTRAIL
using PluginWeaponTrail;

[CustomEditor(typeof(XWeaponTrail))]
#endif
[CanEditMultipleObjects]
public class PluginSwordTrailEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Please Buy And Support Official Version!!!");
        EditorGUILayout.EndVertical();

        GUI.color = Color.white;

        GUILayout.Space(10);
        //DrawDefaultInspector();

        SerializedProperty UseWith2D = serializedObject.FindProperty("UseWith2D");
        EditorGUILayout.PropertyField(UseWith2D);
        if (UseWith2D.boolValue) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SortingLayerName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SortingOrder"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("PointStart"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PointEnd"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxFrame"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Granularity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Fps"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MyColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MyMaterial"));

        if (GUI.changed) {

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(target);
        }
    }
}

