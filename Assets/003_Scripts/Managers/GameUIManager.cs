using UnityEngine;
using System.Collections;

public class GameUIManager : MonoBehaviour {
	public CanvasGroup winPanel;
	public CanvasGroup losePanel;

	void OnEnable(){
		Messenger.AddListener (Events.Buttons.NEXT_LEVEL, OnNextLevelClick);
		Messenger.AddListener (Events.Buttons.PLAY_AGAIN, OnPlayAgainClick);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Buttons.NEXT_LEVEL, OnNextLevelClick);
		Messenger.RemoveListener (Events.Buttons.PLAY_AGAIN, OnPlayAgainClick);
	}

	void OnNextLevelClick(){

	}

	void OnPlayAgainClick(){
		ToggleCanvasGroup (winPanel, false);
		ToggleCanvasGroup (losePanel, false);
	}

	void ToggleCanvasGroup(CanvasGroup gr, bool state){
		gr.alpha = state ? 1f : 0f;
		gr.blocksRaycasts = state;
		gr.interactable = state;
	}
}
