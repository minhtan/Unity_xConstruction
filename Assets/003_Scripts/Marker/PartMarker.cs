using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartMarker : MonoBehaviour {
	List<GameObject> connections = new List<GameObject>();

	public void AddConnection(GameObject go){
		connections.Add (go);
	}

	public void ClearConnection(){
		var rBody = GetComponent<Rigidbody2D> ();
		for (int i = 0; i < connections.Count; i++) {
			var hinges = connections [i].GetComponents<HingeJoint2D> ();
			for (int j = 0; j < hinges.Length; j++) {
				if (hinges [j].connectedBody == rBody) {
					Destroy (hinges [j]);
				}
			}
		}
		connections.Clear ();
	}
}
