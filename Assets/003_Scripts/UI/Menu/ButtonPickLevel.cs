using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonPickLevel : MonoBehaviour {

	// Use this for initialization
	public void RegisterButton (string lvlToLoad) {
		GetComponent<Button> ().onClick.AddListener (()=>{
			PlayerPrefs.SetString(ConstantData.PP_LevelPicked, lvlToLoad);
			SceneManager.LoadScene("game");
		});

		GetComponentInChildren<Text> ().text = lvlToLoad;
	}
	
	// Update is called once per frame
	void OnDisable () {
		GetComponent<Button> ().onClick.RemoveAllListeners ();
	}
}
