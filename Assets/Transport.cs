using UnityEngine;
using System.Collections;

public class Transport : MonoBehaviour {
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction
	bool is_ctrl_pressed = false;
	float move_speed = 2f; //< Speed of moving with "fly mode"
	
	void Update() {
		CharacterMotor motor = GetComponent<CharacterMotor>();
		CharacterController controller = GetComponent<CharacterController>();
		
		// Control is pressed - set up "fly mode"
		if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded) {
			forward = controller.transform.forward;
			is_ctrl_pressed = true;
			motor.movement.gravity = 0;
		}
		// Control is released - return to the original mode.
		if (Input.GetKeyUp(KeyCode.LeftControl)) {
			is_ctrl_pressed = false;
			motor.movement.gravity = -Physics.gravity.y;
		}
		// Control is held - continue "flying".
		if (Input.GetKey(KeyCode.LeftControl)) {
			controller.Move(forward * Time.deltaTime * move_speed);
		}		
	}		
}
