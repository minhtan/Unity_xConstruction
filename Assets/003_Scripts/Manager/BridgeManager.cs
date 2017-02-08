using UnityEngine;
using System.Collections;
using Lean;
using System.Collections.Generic;

public class BridgeManager : MonoBehaviour {

	public GameObject railPrefab;
	public GameObject suspensionPrefab;
	public GameObject pointPrefab;
	public LayerMask pointLayer;

	Vector3 initMousePos;
	GameObject prefabToSpawn;
	GameObject selectedPoint;
	GameObject newPart;
	List<GameObject> points = new List<GameObject>();
	List<GameObject> parts = new List<GameObject>();

	void Awake () {
		Messenger.AddListener<Vector3> (Events.Input.Pressed, OnMousePressed);
		Messenger.AddListener<Vector3> (Events.Input.Hold, OnMouseHold);
		Messenger.AddListener<Vector3> (Events.Input.Realeased, OnMouseReleased);

		Messenger.AddListener (Events.Buttons.SUSPENSION, OnSuspensionClick);
		Messenger.AddListener (Events.Buttons.RAIL, OnRailClick);
		Messenger.AddListener (Events.Buttons.DELETE, OnDeleteClick);
		Messenger.AddListener (Events.Buttons.PLAY, OnPlayClick);
	}

	void OnDestroy() {
		Messenger.RemoveListener<Vector3> (Events.Input.Pressed, OnMousePressed);
		Messenger.RemoveListener<Vector3> (Events.Input.Hold, OnMouseHold);
		Messenger.RemoveListener<Vector3> (Events.Input.Realeased, OnMouseReleased);

		Messenger.RemoveListener (Events.Buttons.SUSPENSION, OnSuspensionClick);
		Messenger.RemoveListener (Events.Buttons.RAIL, OnRailClick);
		Messenger.RemoveListener (Events.Buttons.DELETE, OnDeleteClick);
		Messenger.RemoveListener (Events.Buttons.PLAY, OnPlayClick);
	}

	void Start(){
		prefabToSpawn = railPrefab;

		var startPoints = FindObjectsOfType<PointMarker> ();
		for (int i = 0; i < startPoints.Length; i++) {
			points.Add(startPoints[i].gameObject);
		}

		var startParts = FindObjectsOfType<PartMarker> ();
		for (int i = 0; i < startParts.Length; i++) {
			parts.Add (startParts [i].gameObject);
		}
	}

	void OnSuspensionClick(){
		prefabToSpawn = suspensionPrefab;
	}

	void OnRailClick(){
		prefabToSpawn = railPrefab;
	}

	void OnDeleteClick(){
		prefabToSpawn = null;
	}

	void OnPlayClick(){
		for (int i = 0; i < points.Count; i++) {
			points [i].GetComponent<Rigidbody2D> ().isKinematic = false;
		}

		for (int i = 0; i < parts.Count; i++) {
			parts [i].GetComponent<Rigidbody2D> ().isKinematic = false;
		}
	}

	void OnMousePressed(Vector3 pos){
		initMousePos = pos;
		Vector2 v = Camera.main.ScreenToWorldPoint (pos);
		RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero, Mathf.Infinity, pointLayer);
		if (hit.collider != null) {
			selectedPoint = hit.collider.gameObject;
			newPart = AddPart (selectedPoint, pos);
			AddJoint (selectedPoint, newPart, 200f);
		}
	}

	void OnMouseHold(Vector3 pos){
		if (selectedPoint != null && newPart != null) {
			var dir = Camera.main.ScreenToWorldPoint (pos) - newPart.transform.position;
			var angle = ClampAngle(Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
			newPart.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			var step = 0.1f;
			var dis = Vector2.Distance ((Vector2)GetEndPosition (newPart), (Vector2)Camera.main.ScreenToWorldPoint (pos));
			while (dis > 0.05f) {
				newPart.transform.localScale = new Vector3 (newPart.transform.localScale.x + step, newPart.transform.localScale.y, newPart.transform.localScale.z);
				var newDis = Vector2.Distance ((Vector2)GetEndPosition (newPart), (Vector2)Camera.main.ScreenToWorldPoint (pos));
				if (newDis > dis) {
					step = -step;
				}
				dis = newDis;
			}
		}
	}

	void OnMouseReleased(Vector3 pos){
		if (selectedPoint != null && newPart != null) {
			var point = GetEndPoint (newPart);
			if (point != null) {
				AddJoint (point, newPart, 200f);
			} else {
				point = AddPoint (newPart);
				AddJoint (point, newPart);
			}
		}
		selectedPoint = null;
		newPart = null;
	}
		
	void AddJoint(GameObject point, GameObject body, float breakForce = 0f){
		var joint = point.AddComponent<HingeJoint2D> ();
		if (breakForce != 0f) {
			joint.breakForce = breakForce;
		}
		joint.connectedBody = body.GetComponent<Rigidbody2D>();
	}

	GameObject AddPoint(GameObject pos){
		var newPoint = LeanPool.Spawn (pointPrefab);
		newPoint.transform.position = GetEndPosition (pos);

		points.Add (newPoint);
		return newPoint;
	}

	GameObject AddPart(GameObject origin, Vector3 mousePos){
		var part = LeanPool.Spawn (prefabToSpawn);
		part.transform.position = origin.transform.position;
		var dir = Camera.main.ScreenToWorldPoint (mousePos) - part.transform.position;
		var angle = ClampAngle(Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
		part.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		part.transform.localScale = new Vector3 (0, 1, 1);

		parts.Add (part);
		return part;
	}

	GameObject GetEndPoint(GameObject part){
		for (int i = 0; i < points.Count; i++) {
			if (Vector3.Distance( points[i].transform.position, GetEndPosition(part)) < 0.1f) {
				return points [i];
			}
		}

		return null;
	}

	Vector3 GetEndPosition(GameObject obj){
		return obj.transform.position + (new Vector3(obj.GetComponent<SpriteRenderer> ().sprite.bounds.size.x * obj.transform.localScale.x, 0f, 0f).magnitude * obj.transform.right);
	}

	float ClampAngle(float rad){
		return rad;
		if (rad >= -Mathf.PI * 1/8 && rad < Mathf.PI * 1/8) {
			return 0f;
		}else if (rad >= Mathf.PI * 1/8 && rad < Mathf.PI * 3/8) {
			return Mathf.PI / 4;
		}else if (rad >= Mathf.PI * 3/8 && rad < Mathf.PI * 5/8) {
			return Mathf.PI / 2;
		}else if (rad >= Mathf.PI * 5/8 && rad < Mathf.PI * 7/8) {
			return Mathf.PI * 3/4;
		}else if (rad >= Mathf.PI * 7/8 || rad < -Mathf.PI * 7/8) {
			return Mathf.PI;
		}else if (rad >= -Mathf.PI * 7/8 && rad < -Mathf.PI * 5/8) {
			return -Mathf.PI * 3/4;
		}else if (rad >= -Mathf.PI * 5/8 && rad < -Mathf.PI * 3/8) {
			return -Mathf.PI / 2;
		}else if (rad >= -Mathf.PI * 3/8 && rad < -Mathf.PI * 1/8) {
			return -Mathf.PI / 4;
		}else {
			return 0f;
		}
	}
}
