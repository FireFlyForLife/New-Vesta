using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

public class DrawArrowForward : MonoBehaviour
{
#if UNITY_EDITOR
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosArrow(DrawArrowForward drawArrow, GizmoType type)
    {
        Gizmos.DrawLine(drawArrow.transform.position, drawArrow.transform.position + drawArrow.transform.forward * 0.75f);
        Gizmos.DrawWireSphere(drawArrow.transform.position, 0.05f);
    }
#endif
}
