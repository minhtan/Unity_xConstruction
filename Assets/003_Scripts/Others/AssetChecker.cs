using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class AssetChecker : AssetPostprocessor {
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
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
	}
}
