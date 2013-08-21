using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	const int CUBE_X_COUNT = 5;
	const int CUBE_Z_COUNT = 5;	
	const int CUBE_COUNT = CUBE_X_COUNT * CUBE_Z_COUNT;
	GameObject cube_obj;
	const float cube_width = 0.2f;
	const float cube_depth = 0.2f;
	
	// Use this for initialization
	void Start () {
		cube_obj = GameObject.Find("PlatformCube");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void startFlight() {
		Vector3 pos = Vector3.zero;
		pos.x -= cube_width * CUBE_X_COUNT / 2.0f;
		pos.z -= cube_width * CUBE_Z_COUNT / 2.0f;
		for (int x = 0; x < CUBE_X_COUNT; x++) {
			for (int z = 0; z < CUBE_Z_COUNT; z++) {
				Vector3 new_pos = pos;
				new_pos.x += x*cube_width;
				new_pos.z += z*cube_depth;
				GameObject new_cube = (GameObject) Instantiate(cube_obj, cube_obj.transform.position, cube_obj.transform.rotation);
				new_cube.transform.Translate(new_pos);
				new_cube.renderer.enabled = true;
				new_cube.transform.parent = transform;
			}			
		}
		// renderer.enabled = true;
		audio.enabled = true;
	}
	
	public void stopFlight() {
		// renderer.enabled = false;
		audio.enabled = false;
	}
}
