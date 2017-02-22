using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonQuit : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		GetComponent<Button> ().onClick.AddListener (() => {
			Application.Quit();
		});
	}

	void OnDisable() {
		GetComponent<Button> ().onClick.RemoveAllListeners ();
	}
}
