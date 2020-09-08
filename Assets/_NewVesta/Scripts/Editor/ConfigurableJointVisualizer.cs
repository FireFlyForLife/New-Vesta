using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfigurableJointVisualizer : MonoBehaviour
{
    [MenuItem("CONTEXT/ConfigurableJoint/Toggle Visualizer")]
    static void ToggleVisualizer()
    {
        Debug.Log("Toggling visualizer!");
    }
}
