using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGearChild : MonoBehaviour
{
    public float ToothCount;

    public List<RotateGearChild> rotateGearChildren;

    public void Rotate(float rotation)
    {
        rotation /= ToothCount;
        transform.Rotate(transform.forward * rotation);

        foreach (RotateGearChild gear in rotateGearChildren)
        {
            gear.Rotate(-rotation * ToothCount);
        }
    }
}
