using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneToLoad{
	menu, 
	game
}

public class BtnLoadScene : MonoBehaviour {
	public SceneToLoad sceneToLoad;
	void OnEnable(){
		GetComponent<Button> ().onClick.AddListener (() => {
			SceneManager.LoadScene(sceneToLoad.ToString());
		});
	}
	void OnDisable(){
		GetComponent<Button> ().onClick.RemoveAllListeners ();
	}
}
