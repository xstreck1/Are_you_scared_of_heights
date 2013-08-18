using UnityEngine;
using System.Collections;

public class RotatePlatform : MonoBehaviour {
	
	public int rotationSpeed = 5;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, rotationSpeed*Time.deltaTime);
	}
}
