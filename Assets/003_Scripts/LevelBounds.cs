using UnityEngine;
using System.Collections;

public class LevelBounds : MonoBehaviour {
	void Start () {
		GetBounds ();
	}


	void GetBounds(){
		var sprite = GetComponent<SpriteRenderer> ().sprite;
		Debug.Log (sprite.bounds.extents.x + " " + sprite.bounds.extents.y + " " + sprite.bounds.extents.z);
		Debug.DrawLine (transform.position, new Vector3 (sprite.bounds.extents.x, 0, 0), Color.red, Mathf.Infinity);
		Debug.DrawLine (transform.position, new Vector3 (0, -sprite.bounds.extents.y, 0), Color.red, Mathf.Infinity);

		Debug.Log (sprite.rect.width + " " + sprite.rect.height);
	}
}
