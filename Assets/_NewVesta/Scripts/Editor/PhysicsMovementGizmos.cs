using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEditor;

public class PhysicsMovementGizmos : EditorWindow
{
    private Rigidbody targetRigidbody = null;
    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private EditorCoroutine fixedUpdateCoroutine;

    [MenuItem("Tools/PhysicsMovementGizmos")]
    public static void ShowPhysicsMovementGizmos()
    {
        var physicsMovementWindow = EditorWindow.GetWindow<PhysicsMovementGizmos>();
        physicsMovementWindow.Show();
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += this.OnSceneGUI;
        fixedUpdateCoroutine = EditorCoroutineUtility.StartCoroutine(FixedUpdateCoroutine(), this);
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        EditorCoroutineUtility.StopCoroutine(fixedUpdateCoroutine);
    }

    // ReSharper disable once InconsistentNaming
    protected void OnGUI()
    {
        EditorGUILayout.LabelField("Hello Physics Movement Gizmos!");
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!targetRigidbody || !Application.isPlaying)
            return;

        Tools.current = Tool.None;

        EditorGUI.BeginChangeCheck();
        Handles.TransformHandle(ref targetPosition, ref targetRotation);
        if (EditorGUI.EndChangeCheck())
        {

        }
    }

    void OnSelectionChange()
    {
        var selectedGameObject = Selection.activeGameObject;
        if (!selectedGameObject)
        {
            targetRigidbody = null;
            return;
        }
        targetRigidbody = Selection.activeGameObject.GetComponent<Rigidbody>();
        if (targetRigidbody)
        {
            targetPosition = targetRigidbody.position;
            targetRotation = targetRigidbody.rotation;
        }
    }

    IEnumerator FixedUpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (targetRigidbody && Application.isPlaying)
            {
                Vector3 deltaMovement = targetPosition - targetRigidbody.position;
                targetRigidbody.velocity = deltaMovement / Time.fixedDeltaTime;

                Quaternion deltaRotation = targetRotation * Quaternion.Inverse(targetRigidbody.rotation);
                Vector3 deltaRotationEuler = deltaRotation.eulerAngles;
                Vector3 eulerRotation = new Vector3(Mathf.DeltaAngle(0, deltaRotationEuler.x),
                    Mathf.DeltaAngle(0, deltaRotationEuler.y), Mathf.DeltaAngle(0, deltaRotationEuler.z));
                eulerRotation *= 0.95f * Mathf.Deg2Rad;
                targetRigidbody.angularVelocity = eulerRotation / Time.fixedDeltaTime;
            }
        }
    }
}
