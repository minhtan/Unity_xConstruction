using UnityEngine;
using System.Collections;
using Lean;

public class LevelFiller : MonoBehaviour {
	public GameObject lvlButton;

	// Use this for initialization
	void OnEnable () {
		LoadLevels ();
	}
	
	// Update is called once per frame
	void LoadLevels () {
		var lvlInfo = Resources.Load<LevelsInfo> (ConstantData.LevelsInfoPath);

		for (int i = 0; i < lvlInfo.names.Count; i++) {
			var btn = LeanPool.Spawn (lvlButton);
			btn.transform.SetParent (transform, false); 
			btn.GetComponent<ButtonPickLevel>().RegisterButton(lvlInfo.names [i]);
		}
	}

	void ClearChildren(){
		foreach (Transform child in transform) {
			LeanPool.Despawn (child.gameObject);
		}
	}
}
