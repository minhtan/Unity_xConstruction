using UnityEngine;
using System.Collections;

public class PointMarker : MonoBehaviour {
	public void Reset(){
		GetComponent<Rigidbody2D> ().isKinematic = true;
		var hinges = GetComponents<HingeJoint2D> ();
		for (int i = 0; i < hinges.Length; i++) {
			DestroyImmediate (hinges [i]);
		}
	}
}
