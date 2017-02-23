using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuUIManager : MonoBehaviour {
	public CanvasGroup levelsPanel;

	public void ToggleLevels(bool state){
		levelsPanel.alpha = state ? 1f : 0f;
		levelsPanel.blocksRaycasts = state;
		levelsPanel.interactable = state;
	}
}
