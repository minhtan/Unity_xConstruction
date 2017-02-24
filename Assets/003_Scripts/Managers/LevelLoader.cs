using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
	GameObject currentLvl;

	void Start(){
		if (!PlayerPrefs.HasKey (ConstantData.PP_LevelPicked)) {
			LoadLevel ("Level_001");
		} else {
			LoadLevel (PlayerPrefs.GetString(ConstantData.PP_LevelPicked));
		}
	}

	public void LoadLevel (string lvlToLoad) {
		if (currentLvl != null) {
			DestroyImmediate (currentLvl);
		}

		StartCoroutine (LoadLevel (lvlToLoad, (go) => {
			currentLvl = GameObject.Instantiate (go);
			Messenger.Broadcast<GameObject>(Events.Background.LEVEL_LOADED, currentLvl);
		}));
	}

	IEnumerator LoadLevel(string lvlToLoad, System.Action<GameObject> callback){
		var r = Resources.LoadAsync<GameObject> (ConstantData.LevelsFolder + lvlToLoad);
		while(!r.isDone){
			yield return null;
		}
		callback (r.asset as GameObject);
	}
}
