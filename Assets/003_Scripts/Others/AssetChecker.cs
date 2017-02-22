using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class AssetChecker : AssetPostprocessor {
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
		//For debugging
//		foreach (string str in importedAssets)
//		{
//			Debug.Log("Reimported Asset: " + str);
//		}
//		foreach (string str in deletedAssets) 
//		{
//			Debug.Log("Deleted Asset: " + str);
//		}
//
//		for (int i=0; i<movedAssets.Length; i++)
//		{
//			Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
//		}

		var lvlInfo = Resources.Load<LevelsInfo> (ConstantData.LevelsInfoPath);

		if (lvlInfo == null) {
			lvlInfo = ScriptableObject.CreateInstance<LevelsInfo>();
			AssetDatabase.CreateAsset (lvlInfo, "Assets/Resources/" + ConstantData.LevelsInfoPath + ".asset");
			AssetDatabase.SaveAssets ();
		}


		foreach (string str in deletedAssets) 
		{
			if (str.Contains(ConstantData.LevelsPath)) {
				var newStr = str.Replace (ConstantData.LevelsPath, "").Replace(".prefab", "");
				lvlInfo.names.RemoveIfExist(newStr);
			}
		}

		foreach (string str in movedFromAssetPaths) {
			if (str.Contains(ConstantData.LevelsPath)) {
				var newStr = str.Replace (ConstantData.LevelsPath, "").Replace(".prefab", "");
				lvlInfo.names.RemoveIfExist(newStr);
			}
		}

		foreach (string str in importedAssets) {
			if (str.Contains(ConstantData.LevelsPath)) {
				var newStr = str.Replace (ConstantData.LevelsPath, "").Replace(".prefab", "");
				lvlInfo.names.AddIfNotExist(newStr);
			}
		}

//		lvlInfo.names.Clear ();
//		var levels = Resources.LoadAll (ConstantData.LevelsFolder);
//		for (int i = 0; i < levels.Length; i++) {
//			lvlInfo.names.Add (levels [i].name);
//		}
	}
}
