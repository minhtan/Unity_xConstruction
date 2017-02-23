using UnityEngine;
using System.Collections;

public class VehicleCollisionDetect : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.GetComponent<VehicleMarker>() != null) {
			Messenger.Broadcast (Events.Game.WIN);
		}
	}

}
