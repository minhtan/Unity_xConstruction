using UnityEngine;
using System.Collections;

public class OriginMarker : MonoBehaviour {
	Vector3 pos;
	Quaternion rot;
	void Awake(){
		pos = transform.localPosition;
		rot = transform.localRotation;
	}

	public void Reset(){
		GetComponent<Rigidbody2D> ().isKinematic = true;
		transform.localPosition = pos;
		transform.localRotation = rot;
	}
}
