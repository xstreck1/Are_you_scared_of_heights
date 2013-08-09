using UnityEngine;
using System.Collections;

// Class that allows the player to fly when space is pressed.
public class ORTransport : MonoBehaviour {
	const UnityEngine.KeyCode FLY_KEY = KeyCode.Space;
	const int FLY_BUTTON = (int) OVRGamepadController.Button.A;
	bool button_down = false; //< Used for determining whether the XBox button was already pressed.
	
	OVRPlayerController player; //< Reference to the player object
	CharacterController controller; //< Reference to the controller object.
	GameObject forward_vector; //< Object storing the forward direction of the player.
	GameObject platform; //< Platform underneath the player.
	
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction
	float move_speed = 2f; //< Speed of moving with "fly mode"
	float grav_modif = 0f; //< Storing original gravity modifier
	bool flying = false; //< Determines whether the player is flying now.
	bool falling = false; //< Determines whether the player is falling now.
	
	void Start() {
		// Locate references
		player = GetComponent<OVRPlayerController>();
		controller = GetComponent<CharacterController>();
		forward_vector = GameObject.Find("ForwardDirection");
		platform = GameObject.Find("Platform");
		
		grav_modif = player.GravityModifier; // Remember the original gravity.
	}
	
	void Update() {
		// Control is pressed - set up "fly mode"
		if (isFlyKeyDown() && controller.isGrounded) {
			forward = forward_vector.transform.forward;
			player.Stop();
			player.GravityModifier = 0f;
			platform.renderer.enabled = true;
			platform.audio.enabled = true;
			flying = true;
		}
		// Control is released - return to the original movement.
		if (isFlyKeyUp()) {
			player.GravityModifier = grav_modif;
			platform.renderer.enabled = false;
			platform.audio.enabled = false;
			flying = false;
		}
		// Control is held - continue movement.
		if (isFlyKey() && flying) {
			controller.Move(forward * Time.deltaTime * move_speed);
		}	
		
		if (!controller.isGrounded && !flying)
			falling = true;
		else
			falling = false;
	}
	
	public bool isFlying() {
		return flying;
	}
	
	public bool isFalling() {
		return falling;
	}
	
	public bool isArrowKey() {
		return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow); 
	}
	
	public bool isWSAD() {
		return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D); 		
	}
	
	public bool isLeftJoy() {
		return (OVRGamepadController.GPC_GetAxis((int)OVRGamepadController.Axis.LeftYAxis) != 0f) || (OVRGamepadController.GPC_GetAxis((int)OVRGamepadController.Axis.LeftXAxis) != 0f);
	}
	
	public bool isMoveIn() {
		return isArrowKey() || isWSAD() || isLeftJoy();
	}
	
	public bool isFlyKeyDown() {
		if (OVRGamepadController.GPC_GetButton(FLY_BUTTON) && !button_down) {
			button_down = true;
			return true;
		}
		else 
			return Input.GetKeyDown(FLY_KEY);
	}
	
	public bool isFlyKey() {
		if (OVRGamepadController.GPC_GetButton(FLY_BUTTON) && button_down) 
			return true;
		else 
			return Input.GetKey(FLY_KEY);				
	}
	
	public bool isFlyKeyUp() {
		if (!OVRGamepadController.GPC_GetButton(FLY_BUTTON) && button_down) 
			return true;
		else 
			return Input.GetKeyUp(FLY_KEY);		
	}
}
