using UnityEngine;
using System.Collections;

public class SystemHelper : MonoBehaviour {
	static bool is_initialized = false;
	static bool is_using_rift = false;
	
	const string TRANSPORTER_TAG = "Transporter"; //< Transporter object tag.
	const string DEBRIS_TRIGGER_TAG = "DebrisTrigger"; //< Debris trigger tag
	const string DEBRIS_OBJECT_TAG = "DebrisObject"; //< Debris object tag
	
	SoundManager sound_manager;
	
	void Start () {
		sound_manager = GetComponent<SoundManager> ();
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
	
	// What to do when trigger is hit
	void OnTriggerEnter (Collider other)
	{
		if (other.tag.CompareTo(TRANSPORTER_TAG) == 0) {
			GetComponent<Transport>().enableFlight();
			other.collider.enabled = false;
			other.renderer.enabled = false;
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
	}
}
