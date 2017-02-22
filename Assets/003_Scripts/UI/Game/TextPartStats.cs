using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextPartStats : MonoBehaviour {
	Text txt;

	// Use this for initialization
	void OnEnable () {
		txt = GetComponent<Text> ();
		Messenger.AddListener<int, int> (Events.Game.PART_CHANGED, UpdatePartStat);
	}
	
	// Update is called once per frame
	void OnDisable () {
		Messenger.RemoveListener<int, int> (Events.Game.PART_CHANGED, UpdatePartStat);
	}


	void UpdatePartStat(int current, int total){
		txt.text = "parts: " + current + "/" + total;
	}
}
