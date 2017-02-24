using UnityEngine;
using System.Collections;

public class GameUIManager : MonoBehaviour {
	public CanvasGroup winPanel;
	public CanvasGroup losePanel;

	void OnEnable(){
		Messenger.AddListener (Events.Buttons.NEXT_LEVEL, CloseResultPanels);
		Messenger.AddListener (Events.Buttons.PLAY_AGAIN, CloseResultPanels);
		Messenger.AddListener (Events.Game.WIN, OnWin);
		Messenger.AddListener (Events.Game.LOSE, OnLose);
	}

	void OnDisable(){
		Messenger.RemoveListener (Events.Buttons.NEXT_LEVEL, CloseResultPanels);
		Messenger.RemoveListener (Events.Buttons.PLAY_AGAIN, CloseResultPanels);
		Messenger.RemoveListener (Events.Game.WIN, OnWin);
		Messenger.RemoveListener (Events.Game.LOSE, OnLose);
	}

	public void CloseResultPanels(){
		ToggleCanvasGroup (winPanel, false);
		ToggleCanvasGroup (losePanel, false);
	}

	public void OnWin(){
		ToggleCanvasGroup (winPanel, true);
	}

	public void OnLose(){
		ToggleCanvasGroup (losePanel, true);
	}

	void ToggleCanvasGroup(CanvasGroup gr, bool state){
		gr.alpha = state ? 1f : 0f;
		gr.blocksRaycasts = state;
		gr.interactable = state;
	}
}
