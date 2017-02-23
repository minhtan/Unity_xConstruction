using UnityEngine;
using System.Collections;

public class LevelParams : MonoBehaviour {

	public float minPartScale;
	public float maxPartScale;
	public int maxPart;
	public float breakForce;
	public float maxTime;
	public GameObject vehicle;

	void OnEnable(){
		ConstructionManager.Instance.SetLevelParams (minPartScale, maxPartScale, maxPart, breakForce);
		GameManager.Instance.Init (vehicle, maxTime);
	}

	//for testing purpose only
	void Update () {
		ConstructionManager.Instance.SetLevelParams (minPartScale, maxPartScale, maxPart, breakForce);
	}
}
