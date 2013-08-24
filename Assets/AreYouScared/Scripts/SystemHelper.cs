using UnityEngine;
using System.Collections;

public class SystemHelper : MonoBehaviour {
	static bool is_initialized = false;
	static bool is_using_rift = false;
	bool is_elevator = false;
	Vector3 down;
	Vector3 elev_pos;
	
	const string TRANSPORTER_TAG = "Transporter";
	const string DEBRIS_TRIGGER_TAG = "DebrisTrigger"; 
	const string DEBRIS_OBJECT_TAG = "DebrisObject";
	const string ELEVATOR_TRIGGER_TAG = "ElevatorTrigger";
	const string ELEVATOR_PLATFORM = "ElevatorPlatform"; 
	
	SoundManager sound_manager;
	Transport transport;
	
	GameObject elevator_platform;
	CharacterController controller; //< Reference to the controller object.
	
	void Start () {
		transport = GetComponent<Transport>();
		sound_manager = GetComponent<SoundManager> ();
		controller = GetComponent<CharacterController> ();
		elevator_platform = GameObject.FindGameObjectWithTag(ELEVATOR_PLATFORM);
		elev_pos = elevator_platform.transform.position;
		down = new Vector3(0f,-40f,0f);
	}

	
	public static void setSystem() {
		is_using_rift = (GameObject.Find ("ForwardDirection") != null);
		is_initialized = true;
	}
	
	public static bool isUsingRift() {
		if (!is_initialized)
			setSystem();
		return is_using_rift;
	}
	
	void Update() {
		if (is_elevator) {
			elevator_platform.transform.Translate(down * Time.deltaTime);
			controller.Move(down * Time.deltaTime);
		}
	}
	
	// What to do when trigger is hit
	void OnTriggerEnter (Collider other)
	{
		if (other.tag.CompareTo(TRANSPORTER_TAG) == 0) {
			GetComponent<Transport>().enableFlight();
			other.collider.enabled = false;
			other.renderer.enabled = false;
			other.audio.Stop();
		}
		if (other.tag.CompareTo(DEBRIS_TRIGGER_TAG) == 0) {
			sound_manager.playDebrisFall();
			GameObject[] gos;
        	gos = GameObject.FindGameObjectsWithTag(DEBRIS_OBJECT_TAG);
			// Let them fall
			foreach (GameObject my_object in gos) {
				my_object.rigidbody.useGravity = true;
			}
			other.collider.enabled = false;
			other.renderer.enabled = false;
		}
		if (other.tag.CompareTo(ELEVATOR_TRIGGER_TAG) == 0) {
			transport.ceaseFlight();
			is_elevator = !is_elevator;
			transport.setFlightEnabled(!is_elevator);
		}
		if (other.name.CompareTo("BGMusicStart") == 0) {
			this.audio.loop = true;
			this.audio.Play();
		}
		
		if (other.name.CompareTo("TriggerOff") == 0) {
			this.audio.Stop();
		}
	}
	
	public void onDeath() {
		elevator_platform.transform.position = elev_pos;
		is_elevator = false;
	}
}
