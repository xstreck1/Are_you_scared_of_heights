using UnityEngine;
using System.Collections;

public class ORTransport : MonoBehaviour {
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction
	float move_speed = 2f; //< Speed of moving with "fly mode"
	float grav_modif = 0f; //< Storing original gravity modifier
	OVRPlayerController player; //< Reference to the player object
	CharacterController controller; //< Reference to the controller object.
	
	void Awake() {
		player = GetComponent<OVRPlayerController>();
		controller = GetComponent<CharacterController>();
		grav_modif = player.GravityModifier;
	}
	
	void Update() {
		// Control is pressed - set up "fly mode"
		if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded) {
			forward = GameObject.Find("ForwardDirection").transform.forward;
			player.Stop();
			player.GravityModifier = 0f;
		}
		// Control is released - return to the original movement.
		if (Input.GetKeyUp(KeyCode.LeftControl)) {
			player.GravityModifier = grav_modif;
		}
		// Control is held - continue movement.
		if (Input.GetKey(KeyCode.LeftControl)) {
			controller.Move(forward * Time.deltaTime * move_speed);
		}		
	}		
}
