using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	GameObject currentLvl;

	void Start(){
		if (!PlayerPrefs.HasKey (ConstantData.PP_LevelPicked)) {
			LoadLevel ("Level_001");
		} else {
			LoadLevel (PlayerPrefs.GetString(ConstantData.PP_LevelPicked));
		}
	}

	// Use this for initialization
	public void LoadLevel (string lvlToLoad) {
		if (currentLvl != null) {
			DestroyImmediate (currentLvl);
			ConstructionManager.Instance.ClearAll ();
		}

		StartCoroutine (LoadLevel (lvlToLoad, (go) => {
			currentLvl = GameObject.Instantiate (go);
			CameraManager.Instance.SetCamera (currentLvl);
			ConstructionManager.Instance.Init ();
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
