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

		Messenger.AddListener (Events.Buttons.NEXT_LEVEL, OnNextLevelClick);
		Messenger.AddListener (Events.Buttons.PLAY_AGAIN, OnResetClick);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Buttons.PLAY, OnPlayClick);
		Messenger.RemoveListener (Events.Buttons.RESET, OnResetClick);
		Messenger.RemoveListener (Events.Game.WIN, OnWin);

		Messenger.RemoveListener (Events.Buttons.NEXT_LEVEL, OnNextLevelClick);
		Messenger.RemoveListener (Events.Buttons.PLAY_AGAIN, OnResetClick);
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
		ConstructionManager.Instance.OnPlayClick ();
		ToggleVehicle (true);
		trackingTime = true;
	}

	void OnResetClick(){
		ConstructionManager.Instance.OnResetClick ();
		ToggleVehicle (false);
		trackingTime = false;

		time = 0f;
		Messenger.Broadcast<float, float> (Events.Game.TIME_CHANGED, time, maxTime);
	}

	void OnNextLevelClick(){
	}

	void OnWin(){
		trackingTime = false;
	}

	void OnLose(){
		trackingTime = false;
		Messenger.Broadcast (Events.Game.LOSE);
	}

	public void Init (GameObject go, float maxTime) {
		vehicle = go;
		this.maxTime = maxTime;
		Messenger.Broadcast<float, float> (Events.Game.TIME_CHANGED, time, maxTime);
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
