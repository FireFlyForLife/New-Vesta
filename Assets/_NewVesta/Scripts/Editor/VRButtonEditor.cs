using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VRButton))]
public class VRButtonEditor : Editor
{
    private SerializedProperty interactionManager;
    private SerializedProperty joint;
    private SerializedProperty isInteractable;
    private SerializedProperty maxPressDistance;
    private SerializedProperty pressPercentage;

    private bool showEvents = false;
    // ReSharper disable InconsistentNaming
    private SerializedProperty m_OnFirstHoverEnter;
    private SerializedProperty m_OnHoverEnter;
    private SerializedProperty m_OnHoverExit;
    private SerializedProperty m_OnLastHoverExit;
    private SerializedProperty m_OnActivate;
    private SerializedProperty m_OnDeactivate;
    // ReSharper restore InconsistentNaming

    void OnEnable()
    {
        interactionManager = serializedObject.FindProperty("m_InteractionManager");
        joint = serializedObject.FindProperty("joint");
        isInteractable = serializedObject.FindProperty("interactable");
        maxPressDistance = serializedObject.FindProperty("maxPressDistance");
        pressPercentage = serializedObject.FindProperty("pressPercentage");

        m_OnFirstHoverEnter = serializedObject.FindProperty("m_OnFirstHoverEnter");
        m_OnHoverEnter = serializedObject.FindProperty("m_OnHoverEnter");
        m_OnHoverExit = serializedObject.FindProperty("m_OnHoverExit");
        m_OnLastHoverExit = serializedObject.FindProperty("m_OnLastHoverExit");
        m_OnActivate = serializedObject.FindProperty("m_OnActivate");
        m_OnDeactivate = serializedObject.FindProperty("m_OnDeactivate");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(interactionManager);
        EditorGUILayout.PropertyField(joint);
        EditorGUILayout.PropertyField(isInteractable);
        EditorGUILayout.PropertyField(maxPressDistance);
        EditorGUILayout.PropertyField(pressPercentage);

        showEvents = EditorGUILayout.Toggle("Show Events", showEvents);
        if (showEvents)
        {
            EditorGUILayout.PropertyField(m_OnFirstHoverEnter);
            EditorGUILayout.PropertyField(m_OnHoverEnter);
            EditorGUILayout.PropertyField(m_OnHoverExit);
            EditorGUILayout.PropertyField(m_OnLastHoverExit);
            EditorGUILayout.PropertyField(m_OnActivate);
            EditorGUILayout.PropertyField(m_OnDeactivate);
        }

        serializedObject.ApplyModifiedProperties();
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoForButton(VRButton button, GizmoType gizmoType)
    {
        ConfigurableJoint joint = button.joint;
        if (!joint)
            return;
        if (!joint.connectedBody)
            return;

        Gizmos.DrawLine(joint.transform.position, joint.transform.position - joint.transform.up * button.maxPressDistance);
    }
}
