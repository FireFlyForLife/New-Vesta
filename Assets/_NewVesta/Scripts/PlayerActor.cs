using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerActor : MonoBehaviour
{
    public static PlayerActor Instance { get; private set; }
    
    [Header("Unity XR Rig")]
    public GameObject xrRigObject;
    public GameObject xrCameraOffsetObject;
    public Camera xrCamera;
    public XRController xrLeftHand;
    public XRController xrRightHand;

    [Header("Physics Player")]
    public Rigidbody headRigidbody;
    public Rigidbody leftHandRigidbody;
    public Rigidbody rightHandRigidbody;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There is more than one PlayerActor in the scene! Disabling {this}");
            gameObject.SetActive(false);
        }
    }
}
