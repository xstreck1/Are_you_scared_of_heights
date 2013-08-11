using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	GameObject platform;
	
	
	// Use this for initialization
	void Start () {
		platform = GameObject.Find("Rotating");
	}
	
	// Update is called once per frame
	void Update () {
		platform.transform.Rotate(0f,0.1f,0f);
	}
}
