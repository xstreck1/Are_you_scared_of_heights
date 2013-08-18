using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	Transport transport;
	UnityEngine.AudioSource feet;
	UnityEngine.AudioSource wind;
	UnityEngine.AudioSource death;
	UnityEngine.AudioSource hit;
	
	void Start () {
		transport = GetComponent<Transport>();
		feet = GameObject.Find("Feet").audio;
		wind = GameObject.Find("Wind").audio;
		death = GameObject.Find("Death").audio;
		hit = GameObject.Find("Hit").audio;
	}	
	
	void Update () {
		if (transport.isMoveIn() && !transport.isFlying() && !transport.isFalling()) {
			if (!feet.isPlaying) {
				feet.loop = true;
				feet.Play();
			}
		}
		else if (feet.isPlaying){
			feet.Stop();	
		}
		
		if (transport.isFalling()) {
			if (!wind.isPlaying) {
				wind.loop = true;
				wind.Play();
			}
		} else if (wind.isPlaying) {
			wind.Stop();
		}
	}
	
	public void playDeath() {
		death.loop = false;
		death.Play();
	}
	
	public void playHit() {
		hit.loop = false;
		hit.Play();
	}
}
