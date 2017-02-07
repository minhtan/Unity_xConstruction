using UnityEngine;
using System.Collections;
using Lean;

public class BridgeManager : MonoBehaviour {

	public GameObject bridgePrefab;
	public GameObject pointPrefab;
	public LayerMask pointLayer;

	GameObject currentSelected;
	GameObject newBridge;

	void Awake () {
		Messenger.AddListener<Vector3> (Events.Input.Hold, OnMouseHold);
		Messenger.AddListener<Vector3> (Events.Input.Pressed, OnMousePressed);
		Messenger.AddListener (Events.Input.Realeased, OnMouseReleased);
	}

	void OnDestroy() {
		Messenger.RemoveListener<Vector3> (Events.Input.Hold, OnMouseHold);
		Messenger.RemoveListener<Vector3> (Events.Input.Pressed, OnMousePressed);
		Messenger.RemoveListener (Events.Input.Realeased, OnMouseReleased);
	}

	void OnMousePressed(Vector3 pos){
		Debug.Log ("Catched Pressed");
		Vector2 v = Camera.main.ScreenToWorldPoint (pos);
		RaycastHit2D hitInfo = Physics2D.Raycast(v, Vector2.zero, Mathf.Infinity, pointLayer);
		if (hitInfo != null) {
			currentSelected = hitInfo.collider.gameObject;
			var go = LeanPool.Spawn (bridgePrefab);
			go.transform.position = currentSelected.transform.position;

			var joint = currentSelected.AddComponent<HingeJoint2D> ();
			joint.breakForce = 200f;
			joint.connectedBody = go.GetComponent<Rigidbody2D>();

		}
	}

	void OnMouseHold(Vector3 pos){
		if (currentSelected != null) {
			
		}
	}

	void OnMouseReleased(){
		if (currentSelected != null) {
			currentSelected = null;
		}
	}
	
	bool CheckForExistingPoint(){
		return true;
	}
}
