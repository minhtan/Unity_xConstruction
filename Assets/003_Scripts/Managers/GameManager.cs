using UnityEngine;
using System.Collections;

public class GameManager : UnitySingletonPersistent<GameManager> {

	bool trackingTime = false;
	float time = 0f;
	float maxTime = 15f;

	GameObject vehicle;

	void OnEnable(){
		Messenger.AddListener (Events.Buttons.PLAY, OnPlayClick);
		Messenger.AddListener (Events.Buttons.RESET, OnResetClick);
		Messenger.AddListener (Events.Game.WIN, OnWin);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Buttons.PLAY, OnPlayClick);
		Messenger.RemoveListener (Events.Buttons.RESET, OnResetClick);
		Messenger.RemoveListener (Events.Game.WIN, OnWin);
	}

	void Update(){
		if (trackingTime) {
			time += Time.deltaTime;
			if (time > maxTime) {
				OnLose ();
			}
			Messenger.Broadcast<float, float> (Events.Game.TIME_CHANGED, time, maxTime);
		}
	}

	void OnPlayClick(){
		ConstructionManager.Instance.PutThingsInMotion ();
		ToggleVehicle (true);
		trackingTime = true;
		time = 0f;
	}

	void OnResetClick(){
		ConstructionManager.Instance.ResetThings ();
		ToggleVehicle (false);
		trackingTime = false;
	}

	void OnWin(){
		Time.timeScale = 0f;
		trackingTime = false;
	}

	void OnLose(){
		Time.timeScale = 0f;
		trackingTime = false;
	}

	public void Init (GameObject go, float maxTime) {
		vehicle = go;
		this.maxTime = maxTime;
		Messenger.Broadcast<float, float> (Events.Game.TIME_CHANGED, 0f, maxTime);
	}

	void ToggleVehicle(bool state){
		var wheels = vehicle.GetComponentsInChildren<WheelJoint2D> ();
		for (int i = 0; i < wheels.Length; i++) {
			wheels [i].useMotor = state;
		}

		var cols = vehicle.GetComponentsInChildren<Collider2D> ();
		for (int i = 0; i < cols.Length; i++) {
			cols [i].isTrigger = !state;
		}
	}
}
