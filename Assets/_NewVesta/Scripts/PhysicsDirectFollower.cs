using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDirectFollower : MonoBehaviour
{
    [Tooltip("The rigidbody that will act as a target point for this rigidbody to follow")]
    public Rigidbody targetRigidbody;

    [Tooltip("The rigidbody that will be moved towards the target rigidbody, If null will be initialized to GetComponent<Rigidbody>()")]
    public Rigidbody thisRigidbody;

    void Start()
    {
        thisRigidbody = thisRigidbody ?? GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 deltaMovement = thisRigidbody.position - targetRigidbody.position;
        targetRigidbody.velocity = deltaMovement / Time.fixedDeltaTime;

        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(targetRigidbody.rotation);
        Vector3 deltaRotationEuler = deltaRotation.eulerAngles;
        Vector3 eulerRotation = new Vector3(Mathf.DeltaAngle(0, deltaRotationEuler.x), Mathf.DeltaAngle(0, deltaRotationEuler.y), Mathf.DeltaAngle(0, deltaRotationEuler.z));
        eulerRotation *= 0.95f * Mathf.Deg2Rad;
        thisRigidbody.angularVelocity = eulerRotation / Time.fixedDeltaTime;
    }
}
