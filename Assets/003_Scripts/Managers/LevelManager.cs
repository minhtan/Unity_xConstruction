using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	GameObject currentLvl;

	void Start(){
		LoadLevel ("Level_001");
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
		var r = Resources.LoadAsync<GameObject> ("Levels/" + lvlToLoad);
		while(!r.isDone){
			yield return null;
		}
		callback (r.asset as GameObject);
	}
}
