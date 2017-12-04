using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitAttribute)), CanEditMultipleObjects]
public class UnitAttributeEditor : Editor
{
    public SerializedProperty attackType;
    public SerializedProperty attackStandard;
    public SerializedProperty attackDeviation;
    public SerializedProperty longRange;
    public SerializedProperty longRangeStandard;
    public SerializedProperty longRangeDeviation;

    private void OnEnable()
    {
        attackType = serializedObject.FindProperty("attackType");
        attackStandard = serializedObject.FindProperty("attackStandard");
        attackDeviation = serializedObject.FindProperty("attackDeviation");
        longRange = serializedObject.FindProperty("longRange");
        longRangeStandard = serializedObject.FindProperty("longRangeStandard");
        longRangeDeviation = serializedObject.FindProperty("longRangeDeviation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(attackType);
        switch (attackType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(attackStandard);
                EditorGUILayout.PropertyField(attackDeviation);
                break;
            case 1:
                EditorGUILayout.PropertyField(attackStandard);
                EditorGUILayout.PropertyField(attackDeviation);
                EditorGUILayout.PropertyField(longRange);
                EditorGUILayout.PropertyField(longRangeStandard);
                EditorGUILayout.PropertyField(longRangeDeviation);
                break;
        }
    }
}
