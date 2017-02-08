using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnSendEvent : MonoBehaviour {

	public Events.Buttons eventToSend;

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (() => {
			Messenger.Broadcast(eventToSend);
		});
	}

	void OnDestroy(){
		GetComponent<Button> ().onClick.RemoveAllListeners ();
	}
}
