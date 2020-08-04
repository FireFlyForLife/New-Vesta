using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class LeverStateChanged : UnityEvent<int> { }

public class Lever : MonoBehaviour
{
    public HingeJoint hinge;

    [SerializeField] private LeverStateChanged[] leverStateCallbacks = new LeverStateChanged[0]{};
    [SerializeField] private float[] leverStateDistribution = new[] {0.5f};

    private int lastSelectedIndex;
    
    void Start()
    {
        if (!hinge)
            hinge = GetComponent<HingeJoint>();

        lastSelectedIndex = CalculateSelectedIndex(hinge.angle, hinge.limits.min, hinge.limits.max);
    }

    void Update()
    {
        int selectedIndex = CalculateSelectedIndex(hinge.angle, hinge.limits.min, hinge.limits.max);
        if (selectedIndex != lastSelectedIndex)
        {
            lastSelectedIndex = selectedIndex;
            leverStateCallbacks[selectedIndex].Invoke(selectedIndex);
            Debug.Log($"Lever invoking: {selectedIndex}");
        }
    }

    private float[] CalculateSummedStateDistribution()
    {
        float[] floats = new float[leverStateDistribution.Length+1];
        floats[0] = leverStateDistribution[0];
        for (int i = 1; i < leverStateDistribution.Length; i++)
        {
            floats[i] = floats[i - 1] + leverStateDistribution[i];
        }
        floats[floats.Length - 1] = 1f;

        return floats;
    }

    private int CalculateSelectedIndex(float angle, float minAngle, float maxAngle)
    {
        float[] summedDistribution = CalculateSummedStateDistribution();

        for (int i = 0; i < summedDistribution.Length; i++)
        {
            if (angle <= Mathf.Lerp(minAngle, maxAngle, summedDistribution[i]))
                return i;
        }

        return Math.Max(0, summedDistribution.Length-1);
    }
}
