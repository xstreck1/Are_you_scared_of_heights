using UnityEngine;
using System.Collections;

// Class that covers respawning.
public class Respawn : MonoBehaviour {
	Vector3 respaw_pos = Vector3.zero; //< Position where the player currently respawns.
	OVRPlayerController player; //< Reference to the player object
	const float DEATH_HEIGHT = -30f; //< Where to start dying.
	const string RESPAWN_NAME = "Respawn"; //< Names of the obects that will cause respawn.
	
	Texture2D  black_texture;
	bool do_fading = false;
	float fading_counter = 0;
	const float FADING_TIME = 2;
	bool do_pause = false;
	float pause_counter = 0;
	const float PAUSE_TIME = 2;
	bool do_lighting = false;
	float lighting_counter = 0;
	const float LIGHTING_TIME = 2;

	// Use this for initialization
	void Start () {
		player = GetComponent<OVRPlayerController>();
		black_texture = new Texture2D(1,1);
		black_texture.SetPixel(0,0,new Color(0,0,0,255));
		black_texture.Apply();
		respaw_pos = player.transform.position;
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (do_fading == true && fading_counter < FADING_TIME) {
			fading_counter += Time.deltaTime;
			float alphaFadeValue = Mathf.Clamp01(fading_counter / FADING_TIME);
 
			GUI.color = new Color(0, 0, 0, alphaFadeValue);
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), black_texture);
		}
		
		else if (do_fading == true && fading_counter >= FADING_TIME) {
			do_fading = false;
			do_pause = true;
			fading_counter = 0;
		} 
		
		if (do_pause == true && pause_counter < PAUSE_TIME) {
			if (pause_counter < PAUSE_TIME / 2 && pause_counter + Time.deltaTime >= PAUSE_TIME / 2) {
				player.Stop();
				player.transform.position = respaw_pos;					
			}
			pause_counter += Time.deltaTime;
 
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), black_texture);
		}
		
		else if (do_pause == true && pause_counter >= PAUSE_TIME) {
			do_pause = false;
			do_lighting = true;
			pause_counter = 0;
		}
		
		else if (do_lighting && lighting_counter < LIGHTING_TIME) {
			lighting_counter += Time.deltaTime;
			float alphaFadeValue = Mathf.Clamp01(lighting_counter / LIGHTING_TIME);
 
			GUI.color = new Color(0, 0, 0, 1 - alphaFadeValue);
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), black_texture);			
		}
		
		else if (do_lighting && lighting_counter >= LIGHTING_TIME) {
			do_lighting = false;
			lighting_counter = 0;			
		}
		
		else if (player.transform.position.y < DEATH_HEIGHT && !do_fading) {
			do_fading = true;
		}
	}
	
	// What to do when trigger is hit
	void OnTriggerEnter(Collider other) {
		if (other.name.Contains(RESPAWN_NAME)) 
        	respaw_pos = other.transform.position;
    }
}
