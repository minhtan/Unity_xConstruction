using UnityEngine;
using System.Collections;

public class GameManager : UnitySingletonPersistent<GameManager> {

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

	void OnPlayClick(){
		ConstructionManager.Instance.PutThingsInMotion ();
		ToggleVehicle (true);
	}

	void OnResetClick(){
		ConstructionManager.Instance.ResetThings ();
		ToggleVehicle (false);
	}

	void OnWin(){
		Time.timeScale = 0f;
	}

	public void SetVehicle (GameObject go) {
		vehicle = go;
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
