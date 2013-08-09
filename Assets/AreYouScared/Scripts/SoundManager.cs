using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	ORTransport transport;
	UnityEngine.AudioSource feet;
	UnityEngine.AudioSource wind;
	UnityEngine.AudioSource hit;
	
	void Start () {
		transport = GetComponent<ORTransport>();
		feet = GameObject.Find("Feet").audio;
		wind = GameObject.Find("Wind").audio;
		hit = GameObject.Find("Hit").audio;
	}	
	
	void Update () {
		if (transport.isMoveIn() && !transport.isFlying() && !transport.isFalling()) {
			feet.enabled = true;
		}
		else {
			feet.enabled = false;	
		}
		
		if (transport.isFalling()) {
			wind.enabled = true;
		} else {
			wind.enabled = false;
		}
	}
	
	public void playHit() {
		hit.loop = false;
		hit.Play();
	}
}
