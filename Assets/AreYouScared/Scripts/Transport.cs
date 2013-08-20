using UnityEngine;
using System.Collections;

// Class that allows the player to fly when space is pressed.
public class Transport : MonoBehaviour
{
	public UnityEngine.KeyCode FLY_KEY = KeyCode.LeftControl;
	public OVRGamepadController.Button FLY_BUTTON = OVRGamepadController.Button.A;
	bool button_down = false; //< Used for determining whether the XBox button was already pressed.
	
	// Common movement control.
	Vector3 forward = Vector3.one; 	//< Vector storing last forward direction.
	CharacterController controller; //< Reference to the controller object.
	
	// OR based movement control.
	OVRPlayerController player; //< Reference to the player object
	GameObject forward_vector; //< Object storing the forward direction of the player.
	float grav_modif = 0f; //< Storing original gravity modifier
	
	// Non-OR based movement control.
	CharacterMotor motor;
	
	GameObject platform; //< Platform underneath the player.
	bool flying = false; //< Determines whether the player is flying now.
	bool falling = false; //< Determines whether the player is falling now.
	public float HIT_DELAY = 1f; //< After how long fall is the hit audible.
	public float HIT_RANGE = 10f; //< The time range in which the hit sound increases in volume.
	float hit_counter = 0f; //< Falling counter.
	
	public float fligth_speed = 2f; //< Speed of moving with "fly mode"
	float acceleration = 0f; //< Acceleration dependent on the controler type.
	public float FLY_TIME = 5f; //< How long can one fly.
	float flight_counter = 0f; //< How long have I already flown.
	bool flight_enabled = false; //< Is the flight mode enabled?
	
	SoundManager sound_manager;
	
	void Start ()
	{
		// Locate references
		controller = GetComponent<CharacterController> ();
		if (SystemHelper.isUsingRift()) {
			player = GetComponent<OVRPlayerController> ();
			forward_vector = GameObject.Find ("ForwardDirection");
			grav_modif = player.GravityModifier; // Remember the original gravity.
			acceleration = player.Acceleration * 15f;
		} else {
			motor = GetComponent<CharacterMotor> ();
			acceleration = motor.MaxSpeedInDirection(Vector3.one) / 5f;
		}
		platform = GameObject.Find ("Platform");
		sound_manager = GetComponent<SoundManager> ();
	}
	
	// Initiate flighting.
	void startFlight ()
	{
		if (SystemHelper.isUsingRift()) {
			forward = forward_vector.transform.forward;
			player.Stop ();
			player.GravityModifier = 0f;
		} else {
			forward = controller.transform.forward;
			motor.SetVelocity (Vector3.zero);
			motor.movement.gravity = 0;
		}
		platform.renderer.enabled = true;
		platform.audio.enabled = true;
		flying = true;
	}
	
	// Cease flighting.
	void stopFlight ()
	{
		if (SystemHelper.isUsingRift()) {
			player.GravityModifier = grav_modif;
		} else { 
			motor.movement.gravity = -Physics.gravity.y;
		}
		platform.renderer.enabled = false;
		platform.audio.enabled = false;
		flying = false;
	}
	
	void Update ()
	{
		// Control is pressed - set up "fly mode"
		if (isFlyKeyDown () && controller.isGrounded && flight_enabled) {
			startFlight ();
		}
		// Control is released - return to the original movement.
		if (isFlyKeyUp () && flying) {
			stopFlight ();
		}
		// Control is held - continue movement.
		if (isFlyKey () && flying) {
			controller.Move (forward * Time.deltaTime * fligth_speed * acceleration);
		}	
		
		// Hit sound management.
		if (controller.isGrounded) {
			if (hit_counter > HIT_DELAY)
				sound_manager.playHit((hit_counter - HIT_DELAY) / HIT_RANGE);
			hit_counter = 0f;
		}
		if (falling) {
			hit_counter += Time.deltaTime;
		}
		
		// Fall determination.
		if (!controller.isGrounded && !flying) {
			falling = true;
		} else {
			falling = false;
		}
		
		// Time window on flying.
		if (flying) {
			flight_counter += Time.deltaTime;
			if (flight_counter >= FLY_TIME)
				stopFlight();
		} else {
			flight_counter = 0;
		}
		
	}
	
	public void resetCounters() {
		flight_counter = hit_counter = 0f;
	}
	
	
	public bool isFlying ()
	{
		return flying;
	}
	
	public bool isFalling ()
	{
		return falling;
	}
	
	public bool isArrowKey ()
	{
		return Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow); 
	}
	
	public bool isWSAD ()
	{
		return Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D); 		
	}
	
	public bool isLeftJoy ()
	{
		return (OVRGamepadController.GPC_GetAxis ((int)OVRGamepadController.Axis.LeftYAxis) != 0f) || (OVRGamepadController.GPC_GetAxis ((int)OVRGamepadController.Axis.LeftXAxis) != 0f);
	}
	
	public bool isMoveIn ()
	{
		return isArrowKey () || isWSAD () || isLeftJoy ();
	}
	
	public bool isFlyKeyDown ()
	{
		if (OVRGamepadController.GPC_GetButton ((int)FLY_BUTTON) && !button_down) {
			button_down = true;
			return true;
		} else 
			return Input.GetKeyDown (FLY_KEY);
	}
	
	public bool isFlyKey ()
	{
		if (OVRGamepadController.GPC_GetButton ((int)FLY_BUTTON) && button_down) 
			return true;
		else 
			return Input.GetKey (FLY_KEY);				
	}
	
	public bool isFlyKeyUp ()
	{
		if (!OVRGamepadController.GPC_GetButton ((int)FLY_BUTTON) && button_down) {
			button_down = false;
			return true;
		} else 
			return Input.GetKeyUp (FLY_KEY);		
	}
	
	public void enableFlight() {
		flight_enabled = true;
	}
}
