using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour
{
    public float turnSpeed = 50f;
    public float ToothCount;

    public RotateGearChild child;

    private void Update()
    {
        float rotation = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            rotation = turnSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
            rotation = -turnSpeed * Time.deltaTime;

        transform.Rotate(transform.forward, rotation);

        child.Rotate(-rotation * ToothCount);
    }
}
