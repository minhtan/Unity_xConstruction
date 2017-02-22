using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListExtension {
	public static void RemoveIfExist(this List<string> list, string name){
		for (int i = 0; i < list.Count; i++) {
			if (list[i].Equals(name)) {
				list.RemoveAt (i);
				return;
			}
		}
	}

	public static void AddIfNotExist(this List<string> list, string name){
		for (int i = 0; i < list.Count; i++) {
			if (list[i].Equals(name)) {
				return;
			}
		}
		list.Add (name);
	}
}
