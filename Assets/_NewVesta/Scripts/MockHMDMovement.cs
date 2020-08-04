using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

public class MockHMDMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 0.25f;
    public bool allowRotation = true;

    private Vector3 lastMousePos = Vector3.zero;
    private Vector2 pitchYaw = Vector2.zero;

    private TrackedPoseDriver trackedPoseDriver;
    

    void Start()
    {
        if (XRSettings.loadedDeviceName == "MockHMD Display")
        {
            trackedPoseDriver = GetComponent<TrackedPoseDriver>();
            if (trackedPoseDriver)
            {
                trackedPoseDriver.enabled = false;
            }
        }
        else
        {
            this.enabled = false;
        }

        lastMousePos = Input.mousePosition;
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        pitchYaw = new Vector2(eulerAngles.x, eulerAngles.y);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float updown = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            updown -= 1.0f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            updown += 1.0f;
        }

        Vector3 localMovement = new Vector3(h, updown, v);
        Vector3 worldMovement = (transform.rotation * localMovement) * movementSpeed * Time.deltaTime; ;

        transform.position += worldMovement;

        if (allowRotation && Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePos = Input.mousePosition;
            }
            Vector2 deltaMousePos = (Input.mousePosition - lastMousePos) * rotationSpeed;
            pitchYaw.x += -deltaMousePos.y;
            pitchYaw.y += deltaMousePos.x;
            transform.rotation = Quaternion.Euler(pitchYaw);

            lastMousePos = Input.mousePosition;
        }
    }
}
