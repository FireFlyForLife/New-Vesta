using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PhysicsMovementGizmos : EditorWindow
{
    [MenuItem("Tools/PhysicsMovementGizmos")]
    public static void ShowPhysicsMovementGizmos()
    {
        var physicsMovementWindow = EditorWindow.GetWindow<PhysicsMovementGizmos>();
        physicsMovementWindow.Show();
    }

    // ReSharper disable once InconsistentNaming
    protected void OnGUI()
    {
        EditorGUILayout.LabelField("Hello Physics Movement Gizmos!");
    }
}
