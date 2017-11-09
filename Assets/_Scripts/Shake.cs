using UnityEngine;
using System.Collections;

// THIS SCRIPT IS NOT MINE!
// I USED THE CODE FROM THIS TUTORIAL: http://unitytipsandtricks.blogspot.si/2013/05/camera-shake.html

public class Shake : MonoBehaviour {
	public float duration;
	public float magnitude;

	public IEnumerator CameraShake() {

		float elapsed = 0.0f;

		Vector3 originalPos = transform.position;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(x, y, originalPos.z);

			yield return null;
		}

		transform.position = originalPos;
	}

}
