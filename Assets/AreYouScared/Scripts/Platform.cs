using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
	const int CUBE_X_COUNT = 5;
	const int CUBE_Z_COUNT = 5;
	const int CUBE_COUNT = CUBE_X_COUNT * CUBE_Z_COUNT;
	const float CUBE_SIZE = 0.20f;
	const float CUBE_PADDING = 0.01f;
	const float CUBE_SPACE = CUBE_PADDING + CUBE_SIZE;
	
	
	public float APPEAR_TIME = 0.40f;
	float appear_counter = float.MaxValue;
	public float DISAPPAER_TIME = 1.5f;
	
	GameObject cube_obj; // Central cube object
	List<GameObject> cubes;
	List<float> cubes_TTL;
	Quaternion rotation;
	bool flying = false;
	bool platform_built = false;
	
	// Use this for initialization
	void Start ()
	{
		cube_obj = GameObject.Find ("PlatformCube");
		cubes = new List<GameObject> ();
		cubes_TTL = new List<float>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (flying)
			transform.rotation = rotation;
		else
			transform.rotation = transform.parent.rotation;
		
		if (appear_counter < APPEAR_TIME) {
			appear_counter += Time.deltaTime;
			float scale_fract = CUBE_SIZE / Mathf.Max(APPEAR_TIME / appear_counter, 1.0f);
			foreach (GameObject cube in cubes)
				cube.transform.localScale = new Vector3(scale_fract,scale_fract,scale_fract);
		}
		
		platform_built = appear_counter >= APPEAR_TIME && flying;
		
		if (platform_built) {
			for(int cube_no = 0; cube_no < CUBE_COUNT; cube_no++) {
				cubes_TTL[cube_no] -= Time.deltaTime;
				float scale_fract = Mathf.Clamp(cubes_TTL[cube_no], 0f, DISAPPAER_TIME) / DISAPPAER_TIME * CUBE_SIZE;
				cubes[cube_no].transform.localScale = new Vector3(scale_fract,scale_fract,scale_fract);
			}
		}		
	}
	
	// Create a single cube and move it from the central cube
	void createCube (Vector3 move, float TTL, bool randomize)
	{
		GameObject new_cube = (GameObject)Instantiate (cube_obj, cube_obj.transform.position, cube_obj.transform.rotation);
		new_cube.transform.Translate (move);
		new_cube.transform.localScale = Vector3.zero;
		new_cube.renderer.enabled = true;
		new_cube.transform.parent = transform;
		cubes.Add (new_cube);
		float time = randomize ? TTL : Random.Range(0f, TTL);
		cubes_TTL.Add(time - APPEAR_TIME + DISAPPAER_TIME);
	}
	
	// Start flying mode - create a platform and play it's sound
	public void startFlight (float TTL)
	{
		audio.loop = true;
		audio.Play ();
		flying = true;
		rotation = transform.rotation;
		
		Vector3 pos = Vector3.zero;
		pos.x -= CUBE_SPACE * CUBE_X_COUNT / 2.0f;
		pos.z -= CUBE_SPACE * CUBE_Z_COUNT / 2.0f;
		
		int last_brick = Random.Range(0,CUBE_X_COUNT-1); 
		for (int x = 0; x < CUBE_X_COUNT; x++) {
			for (int z = 0; z < CUBE_Z_COUNT; z++) {
				Vector3 new_pos = pos;
				new_pos.x += x * CUBE_SPACE;
				new_pos.z += z * CUBE_SPACE;
				createCube (new_pos, TTL, (x == CUBE_X_COUNT/2) && (z == CUBE_Z_COUNT/2));
			}			
		}
		
		appear_counter = 0f;
	}
	
	public void dissolvePlatform() {
		for(int cube_no = 0; cube_no < CUBE_COUNT; cube_no++)
			cubes_TTL[cube_no] = Mathf.Min(cubes_TTL[cube_no], DISAPPAER_TIME);	
	}
	
	// Stop flying mode - destroy the platform.
	public void destroyPlatform ()
	{
		flying = false;
		foreach (GameObject cube in cubes)
			Destroy (cube);
		cubes.Clear ();
		cubes_TTL.Clear ();
		audio.Stop ();
	}
	
	public bool isPlatformBuilt() {
		return platform_built;
	}
	
	public float getDissaperTime() {
		return DISAPPAER_TIME;
	}
}
