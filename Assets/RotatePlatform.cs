using UnityEngine;
using System.Collections;

public class RotatePlatform : MonoBehaviour {
	
	public int rotationX = 0;
	public int rotationY = 0;
	public int rotationZ = 5;
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(rotationX*Time.deltaTime, rotationY*Time.deltaTime, rotationZ*Time.deltaTime);
	}
}
