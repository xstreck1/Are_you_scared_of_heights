using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour {
	const int CUBE_X_COUNT = 5;
	const int CUBE_Z_COUNT = 5;	
	const int CUBE_COUNT = CUBE_X_COUNT * CUBE_Z_COUNT;
	
	const float CUBE_SIZE = 0.20f;
	const float CUBE_PADDING = 0.01f;
	const float CUBE_SPACE = CUBE_PADDING + CUBE_SIZE;
	GameObject cube_obj; // Central cube object
	List<GameObject> cubes;
	Quaternion rotation;
	
	// Use this for initialization
	void Start () {
		cube_obj = GameObject.Find("PlatformCube");
		cubes = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void startFlight() {
		rotation = transform.rotation;
		Vector3 pos = Vector3.zero;
		pos.x -= CUBE_SPACE * CUBE_X_COUNT / 2.0f;
		pos.z -= CUBE_SPACE * CUBE_Z_COUNT / 2.0f;
		for (int x = 0; x < CUBE_X_COUNT; x++) {
			for (int z = 0; z < CUBE_Z_COUNT; z++) {
				Vector3 new_pos = pos;
				new_pos.x += x*CUBE_SPACE;
				new_pos.z += z*CUBE_SPACE;
				GameObject new_cube = (GameObject) Instantiate(cube_obj, cube_obj.transform.position, cube_obj.transform.rotation);
				new_cube.transform.Translate(new_pos);
				new_cube.renderer.enabled = true;
				new_cube.transform.parent = transform;
				cubes.Add(new_cube);
			}			
		}
		// renderer.enabled = true;
		audio.loop = true;
		audio.Play();
	}
	
	public void stopFlight() {
		// renderer.enabled = false;
		audio.Stop();
	}
	
	public void move(Vector3 translation) {
		transform.rotation = rotation;
		/*foreach (GameObject cube in cubes) {
			cube.transform.Translate(translation);
		}*/
	}
}
