using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public bool debug = true;

	void OnEnable(){
		Messenger.AddListener <GameObject> (Events.Background.LEVEL_LOADED, SetCamera);
	}

	void OnDisable(){
		Messenger.RemoveListener <GameObject> (Events.Background.LEVEL_LOADED, SetCamera);
	}

	public void SetCamera(GameObject level){
		var sprite = level.GetComponent<SpriteRenderer> ().sprite;
		var scale = level.transform.localScale.x;

		if (debug) {
			Debug.Log ("Sprite bounds: " + sprite.bounds.extents.x + " " + sprite.bounds.extents.y + " " + sprite.bounds.extents.z);
			Debug.Log ("Sprite rect: " + sprite.rect.width + " " + sprite.rect.height);
			Debug.DrawLine (transform.position, new Vector3 (sprite.bounds.extents.x, 0, 0), Color.red, Mathf.Infinity);
			Debug.DrawLine (transform.position, new Vector3 (0, -sprite.bounds.extents.y, 0), Color.red, Mathf.Infinity);
		}

		Camera camera = GetComponent<Camera>();
		if (camera == null) {
			camera = Camera.main;
		}

		//Set camera orth size
		float spriteHeight = sprite.rect.height;
		float spriteRatio = sprite.rect.width / sprite.rect.height;
		float screenRatio = (float)Screen.width / (float)Screen.height;
		camera.orthographicSize = Mathf.Floor(((spriteHeight * scale) / (100 * 2)) * (spriteRatio / screenRatio) * 100) / 100;

		//Set camera y postiton
		float extendY = sprite.bounds.extents.y * scale;
		float disposition = Camera.main.orthographicSize - extendY;
		camera.transform.Translate (0f, disposition, 0f);
	}
}
