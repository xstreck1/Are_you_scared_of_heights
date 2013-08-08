using UnityEngine;
using System.Collections;

// Class that covers respawning.
public class Respawn : MonoBehaviour {
	Vector3 respaw_pos = Vector3.zero; //< Position where the player currently respawns.
	OVRPlayerController player; //< Reference to the player object
	const float DEATH_HEIGHT = -30f; //< Where to start dying.
	const string RESPAWN_NAME = "Respawn"; //< Names of the obects that will cause respawn.
	
	// Use this for initialization
	void Start () {
		player = GetComponent<OVRPlayerController>();
		respaw_pos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y < DEATH_HEIGHT) {
			player.Stop();
			player.transform.position = respaw_pos;
		}
	}
	
	// What to do when trigger is hit
	void OnTriggerEnter(Collider other) {
		if (other.name.Contains(RESPAWN_NAME)) 
        	respaw_pos = other.transform.position;
    }
}
