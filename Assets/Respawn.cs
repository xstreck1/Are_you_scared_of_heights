using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {
	Vector3 respaw_pos = Vector3.zero;
	OVRPlayerController player; //< Reference to the player object
	const float DEATH_HEIGHT = -30f;
	const string RESPAWN_NAME = "Respawn";
	
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
	
	// 
	void OnTriggerEnter(Collider other) {
		if (other.name.Contains(RESPAWN_NAME)) 
        	respaw_pos = other.transform.position;
    }
}
