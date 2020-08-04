using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Lever))]
public class LeverEditor : Editor
{
    private GuiCascadeUtils.DragCache dragCache = null;

    private SerializedProperty hinge;
    private SerializedProperty callbacks;
    private SerializedProperty leverStateDistribution;

    void OnEnable()
    {
        hinge = serializedObject.FindProperty("hinge");
        callbacks = serializedObject.FindProperty("leverStateCallbacks");
        leverStateDistribution = serializedObject.FindProperty("leverStateDistribution");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Configuration:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(hinge);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Callbacks:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(callbacks);
        SyncArraySize(callbacks, leverStateDistribution);

        EditorGUILayout.LabelField("Distribution:");
        float[] floatDistribution = GetFloatArray(leverStateDistribution);
        GuiCascadeUtils.HandleCascadeSliderGUI(ref floatDistribution, ref dragCache);
        SyncFloatArrayBack(leverStateDistribution, floatDistribution);

        serializedObject.ApplyModifiedProperties();
    }

    private static void SyncArraySize(SerializedProperty callbacks, SerializedProperty leverStateDistribution)
    {
        int differenceArraySize = callbacks.arraySize - (leverStateDistribution.arraySize + 1);
        if (differenceArraySize > 0)
        {
            int startArraySize = leverStateDistribution.arraySize;
            float[] leverStateFloatDistribution = GetFloatArray(leverStateDistribution); 
            float endPercentage = 1f - leverStateFloatDistribution.Sum();
            
            for (int i = 0; i < differenceArraySize; i++)
            {
                leverStateDistribution.InsertArrayElementAtIndex(startArraySize + i);
                SerializedProperty arrayElementProperty =
                    leverStateDistribution.GetArrayElementAtIndex(startArraySize + i);
                arrayElementProperty.floatValue = endPercentage / (differenceArraySize+1);
            }
        }
        else if (differenceArraySize < 0)
        {
            int startArraySize = leverStateDistribution.arraySize;
            for (int i = 0; i < -differenceArraySize; i++)
            {
                leverStateDistribution.DeleteArrayElementAtIndex(startArraySize - 1 - i);
            }
        }
    }

    private static float[] GetFloatArray(SerializedProperty serializedProperty)
    {
        if (!serializedProperty.isArray)
            throw new ArgumentException("serializedProperty is not a float array and thus cannot have it's members float array extracted");

        float[] ret = new float[serializedProperty.arraySize];
        for (int i = 0; i < serializedProperty.arraySize; i++)
        {
            ret[i] = serializedProperty.GetArrayElementAtIndex(i).floatValue;
        }
        return ret;
    }

    private static bool SyncFloatArrayBack(SerializedProperty serializedProperty, float[] floats)
    {
        if (!serializedProperty.isArray)
            throw new ArgumentException("serializedProperty is not a float array and thus cannot have it's members float array set back");
        bool changed = false;

        for (int i = 0; i < serializedProperty.arraySize; i++)
        {
            var arrayElement = serializedProperty.GetArrayElementAtIndex(i);
            if(!Mathf.Approximately(arrayElement.floatValue, floats[i]))
            {
                changed = true;
                arrayElement.floatValue = floats[i];
            }
        }

        return changed;
    }
}
