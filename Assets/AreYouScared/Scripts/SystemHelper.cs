using UnityEngine;
using System.Collections;

public class SystemHelper : MonoBehaviour {
	static bool is_initialized = false;
	static bool is_using_rift = false;
	
	public static void setSystem() {
		is_using_rift = (GameObject.Find ("ForwardDirection") != null);
		is_initialized = true;
	}
	
	public static bool isUsingRift() {
		if (!is_initialized)
			setSystem();
		return is_using_rift;
	}
}
