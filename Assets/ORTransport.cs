using UnityEngine;
using System.Collections;

public class ORTransport : MonoBehaviour {
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction
	float move_speed = 2f; //< Speed of moving with "fly mode"
	
	void Update() {
		// CharacterMotor motor = GetComponent<CharacterMotor>();
		CharacterController controller = GetComponent<CharacterController>();
		OVRPlayerController cam = GetComponent<OVRPlayerController>();
		
		// Control is pressed - set up "fly mode"
		if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded) {
			forward = GameObject.Find("ForwardDirection").transform.forward;
			// motor.movement.gravity = 0;
		}
		// Control is released - return to the original movement.
		if (Input.GetKeyUp(KeyCode.LeftControl)) {

			// motor.movement.gravity = -Physics.gravity.y;
		}
		// Control is held - continue movement.
		if (Input.GetKey(KeyCode.LeftControl)) {
			controller.Move(forward * Time.deltaTime * move_speed);
		}		
	}		
}
