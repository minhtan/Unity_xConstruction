using UnityEngine;
using System.Collections;
using Lean;

public class BridgeManager : MonoBehaviour {

	public GameObject bridgePrefab;
	public GameObject pointPrefab;
	public LayerMask pointLayer;

	GameObject selectedPoint;
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
		Vector2 v = Camera.main.ScreenToWorldPoint (pos);
		RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero, Mathf.Infinity, pointLayer);
		if (hit.collider != null) {
			selectedPoint = hit.collider.gameObject;

			newBridge = LeanPool.Spawn (bridgePrefab);
			newBridge.transform.position = selectedPoint.transform.position;
			var dir = Camera.main.ScreenToWorldPoint (pos) - newBridge.transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			newBridge.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			var joint = selectedPoint.AddComponent<HingeJoint2D> ();
			joint.breakForce = 200f;
			joint.connectedBody = newBridge.GetComponent<Rigidbody2D>();
		}
	}

	void OnMouseHold(Vector3 pos){
		if (selectedPoint != null && newBridge != null) {
			var dir = Camera.main.ScreenToWorldPoint (pos) - newBridge.transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			newBridge.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

	void OnMouseReleased(){
		if (selectedPoint != null) {
			selectedPoint = null;
		}
	}
	
	bool CheckForExistingPoint(){
		return true;
	}
}
