using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnSendEvent : MonoBehaviour {

	public Events.Buttons eventToSend;

	// Use this for initialization
	void OnEnable () {
		GetComponent<Button> ().onClick.AddListener (() => {
			Messenger.Broadcast(eventToSend);
		});
	}

	void OnDisable(){
		GetComponent<Button> ().onClick.RemoveAllListeners ();
	}
}
