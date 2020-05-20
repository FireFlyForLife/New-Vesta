using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// VR button event. Used for registering callbacks in the editor
[System.Serializable]
public class VRButtonEvent : UnityEvent<VRButton> {}

/// VR Button. Behaves like a UI button, but exists as a physical button for you to push in VR
public class VRButton : MonoBehaviour {

	/// Callbacks for button pressed event
	public VRButtonEvent ButtonListeners;
	public bool interactable = true;


	/// Controllers currently interacting with the button
	/// TO DO: List of controllers interacting with button.

	void OnTriggerEnter(Collider _collider)
	{
		// If the button hit's the contact switch it has been pressed
		if (interactable == true && _collider.name == "Switch") {
			TriggerButton ();
		}
	}

	void OnCollisionEnter(Collision _collision)
	{
		
		if (interactable == true && _collision.collider.name == "Switch") {
			// If the button hit's the contact switch it has been pressed
			TriggerButton(); 
		} else if (_collision.rigidbody == null)
			return;

		//TO DO: check if colliding with controllers
	}

	void OnCollisionExit(Collision _collision)
	{
		if (_collision.rigidbody == null)
			return;

		//TO DO: if controller remove from ActiveControllers List
			
	}

	public float TriggerHapticStrength = 0.5f;

	void TriggerButton ()
	{
		if (interactable == false)
			return;

		// Trigger callbacks
		if (ButtonListeners != null) { 
			ButtonListeners.Invoke (this);
			Debug.Log("Button Pressed");
		}

		//TO DO: for each Active Controller trigger haptic Feedback
	
	}
}
