using UnityEngine;
using System.Collections;

public class LevelParams : MonoBehaviour {

	public float minPartScale;
	public float maxPartScale;
	public int maxPart;

	void Start(){
		ConstructionManager.Instance.SetLevelParams (minPartScale, maxPartScale, maxPart);
	}

	//for testing purpose only
	void Update () {
		ConstructionManager.Instance.SetLevelParams (minPartScale, maxPartScale, maxPart);
	}
}
