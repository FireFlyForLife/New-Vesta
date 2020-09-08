using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;


/// VR Button. Behaves like a UI button, but exists as a physical button for you to push in VR
public class VRButton : XRBaseInteractable
{
    public ConfigurableJoint joint;
    private Rigidbody thisRigidbody;
    private Rigidbody otherRigidbody;

	[SerializeField, Tooltip("Whether or not this button can be pressed by an interactable")]
    bool interactable = true;
    public bool IsInteractable => interactable;

	[Tooltip("The maximum downward movement that the button can do before it bottoms out.")]
    public float maxPressDistance = 0.5f;
	[Tooltip("Percentage that the button has to be pressed down of the maxPressDistance for the activate event to be fired.")]
    [Range(0, 100)]
    public float pressPercentage = 80;

    private bool buttonIsPressed = false;
    private Vector3 buttonTopPosLocalSpace;


    public override bool IsSelectableBy(XRBaseInteractor interactor) => false;

    void Start()
    {
        // https://medium.com/luna-labs-ltd/luna-tech-series-a-deep-dive-into-unity-configurable-joints-96c49138b9b7

        if (!joint)
        {
            Debug.LogError("VRButton's has no Configurable Joint assigned!", this);
            return;
        }

        thisRigidbody = joint.GetComponent<Rigidbody>();
        otherRigidbody = joint.connectedBody;
        if (!otherRigidbody)
        {
            Debug.LogError("VRButton's configurable joint has no 'Connected Body' assigned!", this);
            return;
        }

        buttonTopPosLocalSpace = thisRigidbody.transform.localPosition;

        {
            var linearLimit = joint.linearLimit;
            linearLimit.limit = maxPressDistance / 2;
            joint.linearLimit = linearLimit;
        }
        {
            Vector3 thisAnchor = joint.anchor;

            var thisAnchorWorldPos =
                thisRigidbody.transform.TransformPoint(thisAnchor);
            var connectedAnchorWorldPos = thisAnchorWorldPos -
                thisRigidbody.transform.up * (maxPressDistance / 2);
            //var connectedAnchorLocalPos =
            //    otherRigidbody.transform.InverseTransformPoint(connectedAnchorWorldPos);

            otherRigidbody.position = connectedAnchorWorldPos;

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.zero;

            joint.targetPosition = Vector3.down * (maxPressDistance / 2);
        }
        

        if (!interactable)
        {
			//TODO: lock configurable joint movement
        }
    }

    void OnValidate()
    {
        CheckValidSetup(out var _);
    }

    bool CheckValidSetup(out List<string> problems)
    {
        problems = new List<string>(2);

        if (!joint) 
            problems.Add("VRButton's has no Configurable Joint assigned!");
        else
        {
            if (!joint.connectedBody)
                problems.Add("VRButton's configurable joint has no 'Connected Body' assigned!");
        }


        return problems.Count == 0;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed && joint)
        {
			if(joint.configuredInWorldSpace)
				throw new NotImplementedException("joints configured in worldspace are not supported yet!");

            
            float buttonMovedAmount = buttonTopPosLocalSpace.y - thisRigidbody.transform.localPosition.y;

			if (buttonMovedAmount >= maxPressDistance * (pressPercentage / 100f))
            {
                if (!buttonIsPressed)
                {
                    buttonIsPressed = true;
                    OnActivate(null);
                }
            }
            else
            {
                if (buttonIsPressed)
                {
                    buttonIsPressed = false;
                    OnDeactivate(null);
                }
            }
        }
    }

 //   void OnTriggerEnter(Collider _collider)
	//{
	//	// If the button hit's the contact switch it has been pressed
	//	if (interactable == true && _collider.name == "Switch") {
	//		TriggerButton ();
	//	}
	//}

	//void OnCollisionEnter(Collision _collision)
	//{
		
	//	if (interactable == true && _collision.collider.name == "Switch") {
	//		// If the button hit's the contact switch it has been pressed
	//		TriggerButton(); 
	//	} else if (_collision.rigidbody == null)
	//		return;

	//	//TO DO: check if colliding with controllers
	//}

	//void OnCollisionExit(Collision _collision)
	//{
	//	if (_collision.rigidbody == null)
	//		return;

	//	//TO DO: if controller remove from ActiveControllers List
			
	//}

	//public float TriggerHapticStrength = 0.5f;

	//void TriggerButton ()
	//{
	//	if (interactable == false)
	//		return;

	//	// Trigger callbacks
	//	//if (ButtonListeners != null) { 
	//	//	ButtonListeners.Invoke (this);
	//	//	Debug.Log("Button Pressed");
	//	//}

	//	//TO DO: for each Active Controller trigger haptic Feedback
	
	//}
}
