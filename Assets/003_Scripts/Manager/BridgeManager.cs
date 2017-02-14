using UnityEngine;
using System.Collections;
using Lean;
using System.Collections.Generic;

public class BridgeManager : MonoBehaviour {

	public GameObject railPrefab;
	public GameObject suspensionPrefab;
	public GameObject pointPrefab;
	public LayerMask pointLayer;
	public LayerMask partLayer;

	GameObject prefabToSpawn;
	GameObject selectedPoint;
	GameObject newPart;
	List<GameObject> points = new List<GameObject>();
	List<GameObject> parts = new List<GameObject>();

	float angleSnapDegree;
	List<float> angleToSnap = new List<float>();
	float minScale;
	float maxScale;

	Vector2 prevMousePos;

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
		InitAngleParams ();
		InitScaleParams ();
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

	void InitAngleParams(){
		angleSnapDegree = Mathf.PI * 5/180;

		angleToSnap.Add (Mathf.PI *  0/4);
		angleToSnap.Add (Mathf.PI *  1/4);
		angleToSnap.Add (Mathf.PI *  2/4);
		angleToSnap.Add (Mathf.PI *  3/4);
		angleToSnap.Add (Mathf.PI *  4/4);
		angleToSnap.Add (Mathf.PI * -3/4);
		angleToSnap.Add (Mathf.PI * -2/4);
		angleToSnap.Add (Mathf.PI * -1/4);
	}

	void InitScaleParams(){
		minScale = 4f;
		maxScale = 12f;
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
		if (prefabToSpawn != null) {
			prevMousePos = Camera.main.ScreenToWorldPoint (pos);
			RaycastHit2D hit = Physics2D.Raycast (prevMousePos, Vector2.zero, Mathf.Infinity, pointLayer);
			if (hit.collider != null) {
				selectedPoint = hit.collider.gameObject;
				newPart = AddPart (selectedPoint, pos);
				AddJoint (selectedPoint, newPart, 200f);
			}
		} else {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (pos), Vector2.zero, Mathf.Infinity, partLayer);
			if (hit.collider != null) {
				hit.collider.gameObject.GetComponent<PartMarker> ().ClearConnection ();
				LeanPool.Despawn (hit.collider.gameObject);
				for (int i = 0; i < points.Count; i++) {
					if (points[i].GetComponents<HingeJoint2D>().Length == 0) {
						points.RemoveAt (i);
						LeanPool.Despawn (points [i]);
					}
				}
			}	
		}
	}

	void OnMouseHold(Vector3 pos){
		if (selectedPoint != null && newPart != null) {
			RotatePart (newPart, Camera.main.ScreenToWorldPoint (pos));
			ScalePart (newPart, (Vector2)Camera.main.ScreenToWorldPoint (pos));
			SnapToPoint (newPart);
		}
	}

	void OnMouseReleased(Vector3 pos){
		if (selectedPoint != null && newPart != null) {
			var point = GetPointAtEnd (newPart);
			if (point != null) {
				AddJoint (point, newPart, 200f);
			} else {
				point = AddPoint (newPart);
				AddJoint (point, newPart);
			}
			RotatePart (newPart, point.transform.position);
		}
		selectedPoint = null;
		newPart = null;
	}

	void RotatePart(GameObject part, Vector3 toWorldPoint){
		var dir = toWorldPoint - part.transform.position;

		//no snapping
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//snapping
//		var angle = SnapAngle(Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;

		part.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void ScalePart(GameObject part, Vector2 toWorldPoint){
		float step;
		var prevMouseDis = (prevMousePos - (Vector2)part.transform.position).magnitude;
		var mouseDis = (toWorldPoint - (Vector2)part.transform.position).magnitude;

		if (prevMouseDis > mouseDis && part.transform.localScale.x > minScale) {
			step = -0.1f;
		} else if (prevMouseDis < mouseDis && part.transform.localScale.x < maxScale) {
			step = 0.1f;
		} else {
			step = 0f;
		}
		prevMousePos = toWorldPoint;

		var endPosDis = ((Vector2)GetEndPosition (part) - (Vector2)part.transform.position).magnitude;
		var dif = Mathf.Abs(mouseDis - endPosDis);
		while (step != 0 && dif > 0.01f && (part.transform.localScale.x >= minScale && part.transform.localScale.x <= maxScale)) {
			part.transform.localScale = new Vector3 (part.transform.localScale.x + step, part.transform.localScale.y, part.transform.localScale.z);

			endPosDis = ((Vector2)GetEndPosition (part) - (Vector2)part.transform.position).magnitude;
			var newDif = Mathf.Abs(mouseDis - endPosDis);
			if (newDif >= dif) {
				part.transform.localScale = new Vector3 (part.transform.localScale.x - step, part.transform.localScale.y, part.transform.localScale.z);
				break;
			} else {
				dif = newDif;
			}
		}

		part.transform.localScale = new Vector3 (Mathf.Clamp (part.transform.localScale.x, minScale, maxScale), part.transform.localScale.y, part.transform.localScale.z);
	}

	void SnapToPoint(GameObject part){
		var point = GetPointAtEnd (part, 0.3f);
		if (point != null) {
			RotatePart (part, point.transform.position);
			ScalePart (part, (Vector2)point.transform.position);
		}
	}
		
	void AddJoint(GameObject point, GameObject body, float breakForce = 0f){
		var joint = point.AddComponent<HingeJoint2D> ();
		if (breakForce != 0f) {
			joint.breakForce = breakForce;
		}
		joint.connectedBody = body.GetComponent<Rigidbody2D>();
		body.GetComponent<PartMarker> ().AddConnection (point);
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
		var angle = SnapAngle(Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
		part.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		part.transform.localScale = new Vector3 (minScale, 1, 1);

		parts.Add (part);
		return part;
	}

	GameObject GetPointAtEnd(GameObject part, float minDistance = 0.1f){
		for (int i = 0; i < points.Count; i++) {
			if (Vector3.Distance( points[i].transform.position, GetEndPosition(part)) < minDistance) {
				return points [i];
			}
		}

		return null;
	}

	Vector3 GetEndPosition(GameObject obj){
		return obj.transform.position + (new Vector3(obj.GetComponent<SpriteRenderer> ().sprite.bounds.size.x * obj.transform.localScale.x, 0f, 0f).magnitude * obj.transform.right);
	}

	float SnapAngle(float rad){
		//snap to 45deg angles
		for (int i = 0; i < angleToSnap.Count; i++) {
			if (rad > angleToSnap[i] - angleSnapDegree && rad < angleToSnap[i] + angleSnapDegree) {
				return angleToSnap [i];
			}
		}
		return rad;

		//round to 45deg angles
//		return Mathf.Round(rad/(Mathf.PI/4))*(Mathf.PI/4);
	}
}
