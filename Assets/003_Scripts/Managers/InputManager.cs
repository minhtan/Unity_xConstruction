using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public bool debug = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			if (debug) {
				Debug.Log ("Hold");
			}
			Messenger.Broadcast<Vector3> (Events.Input.HOLD, Input.mousePosition);	
		}
		if (Input.GetMouseButtonDown(0)) {
			if (debug) {
				Debug.Log ("Pressed");
			}
			Messenger.Broadcast<Vector3> (Events.Input.PRESSED, Input.mousePosition);	
		}
		if (Input.GetMouseButtonUp(0)) {
			if (debug) {
				Debug.Log ("Released");
			}
			Messenger.Broadcast<Vector3> (Events.Input.RELEASED, Input.mousePosition);	
		}
	}
}
