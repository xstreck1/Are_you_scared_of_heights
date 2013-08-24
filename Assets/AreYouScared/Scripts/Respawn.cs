using UnityEngine;
using System.Collections;

// Class that covers respawning.
public class Respawn : MonoBehaviour
{
	Vector3 respaw_pos = Vector3.zero; //< Position where the player currently respawns.
	OVRPlayerController control; //< Reference to the player object
	CharacterMotor motor;
	Transport transport;
	SystemHelper system_helper;
	float fall_start;
	const string RESPAWN_NAME = "Respawn"; //< Names of the obects that will cause respawn.
	public float DEATH_HEIGHT = 50f; //< Where to start dying.
	
	// Controls connected to screen fading.
	Texture2D  black_texture; //< Texture to hold the full black color.
	enum FadePart // Different parts of the fade event
	{
		fall,
		pause,
		bright,
		none
	};
	FadePart fade_part = FadePart.none; //< Currently active part of the fading.
	float fading_counter = 0f;
	public float FADING_TIME = 2f;
	float pause_counter = 0f;
	public float PAUSE_TIME = 4f;
	public float PAUSE_FRACT = 4f; //< Fraction of the pause where the fall stops.
	float lighting_counter = 0f;
	public float LIGHTING_TIME = 2f;
	
	// Additional
	SoundManager sound_manager;

	void Start ()
	{
		transport = GetComponent<Transport> ();
		fall_start = transform.position.y;
		
		if (SystemHelper.isUsingRift ()) {
			control = GetComponent<OVRPlayerController> ();
		} else {
			motor = GetComponent<CharacterMotor> ();
		}
		
		sound_manager = GetComponent<SoundManager> ();
		system_helper = GetComponent<SystemHelper>();
		black_texture = new Texture2D (1, 1);
		black_texture.SetPixel (0, 0, new Color (0, 0, 0, 255));
		black_texture.Apply ();
		respaw_pos = transform.position;
	}

	void Update ()
	{
		if (!transport.isFalling()) {
			fall_start = transform.position.y;
		} 
		// Debug.Log((fall_start - transform.position.y).ToString());
		
		if (((fall_start - transform.position.y) > DEATH_HEIGHT) && fade_part == FadePart.none) {
			setFadePart (FadePart.fall);
			Debug.Log ("Fall detected.");
		}
	}
	
	// Set up a new part of the fade event
	void setFadePart (FadePart _new_fade)
	{
		fade_part = _new_fade;
		fading_counter = pause_counter = lighting_counter = 0f;
	}
	
	// When to draw GUI
	void OnGUI ()
	{
		switch (fade_part) {
			
		case FadePart.fall:
			if (fading_counter < FADING_TIME) {
				fading_counter += Time.deltaTime;
				float alphaFadeValue = Mathf.Clamp01 (fading_counter / FADING_TIME);
 
				GUI.color = new Color (0, 0, 0, alphaFadeValue);			
			} else 
				setFadePart (FadePart.pause); 
			break;
		
		case FadePart.pause: 
			if (pause_counter < PAUSE_TIME) {
				if (pause_counter < PAUSE_TIME / PAUSE_FRACT && pause_counter + Time.deltaTime >= PAUSE_TIME / PAUSE_FRACT) {
					if (SystemHelper.isUsingRift ()) {
						control.Stop ();
					} else {
						motor.SetVelocity (Vector3.zero);
					}
					transport.resetCounters();
					transform.position = respaw_pos;
					sound_manager.playDeath ();
					system_helper.onDeath ();
				}
				pause_counter += Time.deltaTime;
 
			} else 
				setFadePart (FadePart.bright);
			break;
			
		case FadePart.bright: 
			if (lighting_counter < LIGHTING_TIME) {
				lighting_counter += Time.deltaTime;
				float alphaFadeValue = Mathf.Clamp01 (lighting_counter / LIGHTING_TIME);
 
				GUI.color = new Color (0, 0, 0, 1 - alphaFadeValue);		
			} else 
				setFadePart (FadePart.none);		
			break;
		}
		
		if (fade_part != FadePart.none)
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), black_texture);			
	}
	
	// What to do when trigger is hit
	void OnTriggerEnter (Collider other)
	{
		if (other.name.Contains (RESPAWN_NAME) || other.tag.CompareTo (RESPAWN_NAME) == 0) {
			respaw_pos = other.transform.position;
			respaw_pos.y += 2f;
			other.collider.enabled = false;
			ParticleSystem particles = other.GetComponent<ParticleSystem>();
			if (particles != null)
				particles.Stop();
			else
				other.renderer.enabled = false;
			other.audio.Play();
		}
	}
	
	public bool isInRespawn() {
		return fade_part != FadePart.none;
	}
}
