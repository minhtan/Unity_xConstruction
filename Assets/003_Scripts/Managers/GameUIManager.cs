using UnityEngine;
using System.Collections;

public class GameUIManager : MonoBehaviour {
	public CanvasGroup winPanel;
	public CanvasGroup losePanel;

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
