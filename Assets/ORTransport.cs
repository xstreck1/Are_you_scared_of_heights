using UnityEngine;
using System.Collections;

// Class that allows the player to fly when space is pressed.
public class ORTransport : MonoBehaviour {
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction
	float move_speed = 2f; //< Speed of moving with "fly mode"
	float grav_modif = 0f; //< Storing original gravity modifier
	OVRPlayerController player; //< Reference to the player object
	CharacterController controller; //< Reference to the controller object.
	GameObject forward_vector; //< Object storing the forward direction of the player.
	GameObject platform; //< Platform underneath the player.
	
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
		if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded) {
			forward = forward_vector.transform.forward;
			player.Stop();
			player.GravityModifier = 0f;
			platform.renderer.enabled = true;
			platform.audio.enabled = true;
		}
		// Control is released - return to the original movement.
		if (Input.GetKeyUp(KeyCode.LeftControl)) {
			player.GravityModifier = grav_modif;
			platform.renderer.enabled = false;
			platform.audio.enabled = false;
		}
		// Control is held - continue movement.
		if (Input.GetKey(KeyCode.LeftControl)) {
			controller.Move(forward * Time.deltaTime * move_speed);
		}		
	}		
}
