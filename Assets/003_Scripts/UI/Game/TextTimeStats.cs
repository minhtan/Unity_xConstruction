using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextTimeStats : MonoBehaviour {
	Text txt;

	void OnEnable () {
		txt = GetComponent<Text> ();
		Messenger.AddListener<float, float> (Events.Game.TIME_CHANGED, UpdateTimeStat);
	}

	void OnDisable () {
		Messenger.RemoveListener<float, float> (Events.Game.TIME_CHANGED, UpdateTimeStat);
	}

	void UpdateTimeStat(float current, float total){
		txt.text = "time: " + current.ToString("0.00") + "/" + total;
	}
}
