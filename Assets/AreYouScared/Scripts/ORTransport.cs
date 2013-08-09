using UnityEngine;
using System.Collections;

// Class that allows the player to fly when space is pressed.
public class ORTransport : MonoBehaviour {
	const UnityEngine.KeyCode FLY_KEY = KeyCode.Space;
	
	OVRPlayerController player; //< Reference to the player object
	CharacterController controller; //< Reference to the controller object.
	GameObject forward_vector; //< Object storing the forward direction of the player.
	GameObject platform; //< Platform underneath the player.
	
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction
	float move_speed = 2f; //< Speed of moving with "fly mode"
	float grav_modif = 0f; //< Storing original gravity modifier
	bool flying = false; //< Determines whether the player is flying now.
	
	void Awake() {
		// Locate references
		player = GetComponent<OVRPlayerController>();
		controller = GetComponent<CharacterController>();
		forward_vector = GameObject.Find("ForwardDirection");
		platform = GameObject.Find("Platform");
		
		grav_modif = player.GravityModifier; // Remember the original gravity.
	}
	
	void Update() {
		// Control is pressed - set up "fly mode"
		if (Input.GetKeyDown(FLY_KEY) && controller.isGrounded) {
			forward = forward_vector.transform.forward;
			player.Stop();
			player.GravityModifier = 0f;
			platform.renderer.enabled = true;
			platform.audio.enabled = true;
			flying = true;
		}
		// Control is released - return to the original movement.
		if (Input.GetKeyUp(FLY_KEY)) {
			player.GravityModifier = grav_modif;
			platform.renderer.enabled = false;
			platform.audio.enabled = false;
			flying = false;
		}
		// Control is held - continue movement.
		if (Input.GetKey(FLY_KEY) && flying) {
			controller.Move(forward * Time.deltaTime * move_speed);
		}		
	}		
}
